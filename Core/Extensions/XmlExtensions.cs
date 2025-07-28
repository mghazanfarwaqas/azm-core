using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace Azm.Core.Extensions
{
	public static class XmlExtensions
	{
		
		private static readonly XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

		public static T Deserialize<T>(this string toDeserialize)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                StringReader textReader = new StringReader(toDeserialize);
                return (T)xmlSerializer.Deserialize(textReader);
			}
            catch (Exception)
            {
                return (T) Activator.CreateInstance(typeof(T));

            }
		
		}


		public static string Serialize<T>(this T toSerialize)
		{

			string returnXml = string.Empty;
			try
			{
				XmlDocument doc = new XmlDocument();
				using (XmlWriter writer = doc.CreateNavigator().AppendChild())
				{
					new XmlSerializer(toSerialize.GetType()).Serialize(writer, toSerialize, Namespaces);
				}
				if (doc.DocumentElement != null) returnXml = doc.DocumentElement.OuterXml;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			return returnXml;
		}

		public static string ConvertToXml<T>(this T objectToConvert)
		{
			XmlDocument doc = new XmlDocument();
			XmlNode root = doc.CreateNode(XmlNodeType.Element, objectToConvert.GetType().Name, string.Empty);
			doc.AppendChild(root);
			XmlNode childNode;

			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
			foreach (PropertyDescriptor prop in properties)
			{
				if (prop.GetValue(objectToConvert) != null)
				{
					childNode = doc.CreateNode(XmlNodeType.Element, prop.Name, string.Empty);
					childNode.InnerText = prop.GetValue(objectToConvert).ToString();
					root.AppendChild(childNode);
				}
			}

			return doc.OuterXml;
		}


		public static string SerializeWithPrefix<T>(this T toSerialize)
		{
			string returnXml = string.Empty;
			try
			{
				XmlDocument doc = new XmlDocument();

				using (XmlWriter writer = doc.CreateNavigator().AppendChild())
				{
					new XmlSerializer(toSerialize.GetType()).Serialize(writer, toSerialize);
				}

				if (doc.DocumentElement != null)
				{
					var xmlDeclaration = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

					XmlDocument newDoc = new XmlDocument();
					CreatePrefixedXmlDoc(doc.DocumentElement, newDoc, null);
					returnXml = newDoc.OuterXml.FormatXmlString();
					if (!returnXml.StartsWith(xmlDeclaration))
						return xmlDeclaration + Environment.NewLine + returnXml;
					else
						return returnXml;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			return returnXml;
		}

		private static void CreatePrefixedXmlDoc(XmlElement documentElement, XmlDocument newDoc, XmlNode newNode)
		{
			XmlNode node;

			if (documentElement == null)
				return;
			else
			{
				if (newNode == null)
				{
					var dec = newDoc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
					newDoc.AppendChild(dec);
					node = newDoc.CreateElement("n1", documentElement.Name, "urn:StandardAuditFile-Taxation-CashRegister:NO");
					newDoc.AppendChild(node);
					//XmlElement element = node as XmlElement;
					//element.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
					//element.SetAttribute("xsi:schemaLocation", "urn:StandardAuditFile-Taxation-CashRegister:NO Norwegian_SAF-T_Cash_Register_Schema_v_1.00.xsd");
				}
				else
				{
					node = newDoc.CreateElement("n1", documentElement.Name, "urn:StandardAuditFile-Taxation-CashRegister:NO");
					newNode.AppendChild(node);
				}
			}

			foreach (XmlNode element in documentElement.ChildNodes)
			{
				if (element is XmlElement)
				{
					CreatePrefixedXmlDoc(element as XmlElement, newDoc, node);
				}
				else if (element is XmlText)
				{
					node.AppendChild(newDoc.CreateTextNode(element.InnerText));
				}
			}
		}
	}
}
