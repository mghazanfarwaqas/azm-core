using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Core.Extensions
{
	public static partial class StringExtensions
	{
		public static string SplitOnCapital(this string str)
		{
			var sb = new StringBuilder();

			char previousChar = char.MinValue; // Unicode '\0'
			foreach (char c in str)
			{
				if (char.IsUpper(c))
				{
					// If not the first character and previous character is not a space, insert a space before uppercase
					if (sb.Length != 0 && previousChar != ' ')
					{
						sb.Append(' ');
					}
				}
				sb.Append(c);

				previousChar = c;
			}

			return sb.ToString();
		}

		public static byte[] GetSha1Hash(this byte[] input)
		{
			return new SHA1Managed().ComputeHash(input);
		}

		public static string GetSha1Hash(this string input)
		{
			return string.Join("", (new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input))).Select(x => x.ToString("X2")).ToArray());
		}

		public static byte[] GetSha1HashedByteArray(this string input)
		{
			return new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
		}

		public static byte[] ToUtf8ByteArray(this string input)
		{
			return Encoding.UTF8.GetBytes(input);
		}

		public static byte[] FromBase64ToByteArray(this string imageString)
		{
			return Convert.FromBase64String(imageString);
		}


		public static string ToBase64String(this byte[] input)
		{
			return Convert.ToBase64String(input);
		}

		public static byte[] FromBase64String(this string input)
		{
			return Convert.FromBase64String(input);
		}

		/// <summary>
		/// To the integer. will return 0 if cannot convert.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static int ToInteger(this string value)
		{
			int rtnValue = 0;
			int.TryParse(value, out rtnValue);
			return rtnValue;

		}

		public static StringBuilder ToStringBuilder(this string value)
		{
			return new StringBuilder(value);
		}

	    public static bool ToBoolFromTrueFalse(this string value)
		{
			bool result = false;
			if (string.IsNullOrEmpty(value))
			{
				result = false;
			}
			else if (value.ToLower() == "true")
			{
				result = true;
			}
			else if (value.ToLower() == "false")
			{
				result = false;
			}

			return result;
		}

		public static int ToInt(this string value)
		{
			int result = 0;
			int.TryParse(value, out result);
			return result;
		}

		public static string ToStringNullChecked(this object value)
		{
			if (value == null)
				return string.Empty;
			else
				return Convert.ToString(value, CultureInfo.InvariantCulture);
		}

		public static string GetStringInBetween(this string source, string start, string end)
		{
			if (source.Contains(start) && source.Contains(end))
			{
				var startIndex = source.IndexOf(start, 0) + start.Length;
				var endIndex = source.IndexOf(end, startIndex);

				return source.Substring(startIndex, endIndex - startIndex);
			}
			else
			{
				return string.Empty;
			}
		}

		public static string RemoveString(this string source, string word)
		{
			if (source.Contains(word))
			{
				var startIndex = source.IndexOf(word, 0);

				return source.Substring(startIndex + word.Length);
			}
			else
			{
				return string.Empty;
			}
		}

		public static string GetProductPath(this string value)
		{
			string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images\Products", value);

			if (System.IO.File.Exists(filePath) && !string.IsNullOrEmpty(value))
				return filePath;

			return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images\Products", "default.png");
		}

		public static string FormatXmlString(this string xml)
		{
			try
			{
				XDocument doc = XDocument.Parse(xml);
				return doc.ToString();
			}
			catch (Exception)
			{
				return xml;
			}
		}

		public static string [] GetArrayFromCommaString(this string input)
		{
			List<string> list = new List<string>();
			string tmp = string.Empty;
			bool quoteStarted = false;

			for(int i=0; input != null && i < input.Length;i++)
			{
				if(input[i] == '"')
				{
					quoteStarted = !quoteStarted; continue;
				}
				else if(input[i] != ',' || (input[i] == ',' && quoteStarted == true))
				{
					tmp += input[i].ToString();
				}
				else if(input[i] == ',' && quoteStarted == false)
				{
					list.Add(tmp);
					tmp = string.Empty;
				}
			}

			list.Add(tmp);

			return list.ToArray();
		}

		public static IEnumerable<string> SplitBySize(this string str, int chunkLength)
		{
			if (String.IsNullOrEmpty(str)) throw new ArgumentException();
			if (chunkLength < 1) throw new ArgumentException();

			for (int i = 0; i < str.Length; i += chunkLength)
			{
				if (chunkLength + i > str.Length)
					chunkLength = str.Length - i;

				yield return str.Substring(i, chunkLength);
			}
		}

        public static bool IsPriceEmbededUpc(this string str)
        {
			return (str.ToStringNullChecked().StartsWith("20") || str.ToStringNullChecked().StartsWith("21") || str.ToStringNullChecked().StartsWith("22")) && str.Length == 6;

        }

        public static bool IsWeightEmbededUpc(this string str)
        {
            return (str.ToStringNullChecked().StartsWith("23") || str.ToStringNullChecked().StartsWith("24") || str.ToStringNullChecked().StartsWith("25")) && str.Length == 6;
        }

        public static bool IsPriceOrWeightEmbeded(this string str)
        {
            return str.ToStringNullChecked().IsPriceEmbededUpc() || str.ToStringNullChecked().IsWeightEmbededUpc();
        }


		public static string AvelynToLower(this string value)
		{
			if (value == null)
				return string.Empty;

			return value.ToLower();
		}

        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string ToStringEx(this Object givenObject)
        {
            PropertyInfo[] _PropertyInfos = null;

            if (_PropertyInfos == null)
                _PropertyInfos = givenObject.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(givenObject, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }

    }
}