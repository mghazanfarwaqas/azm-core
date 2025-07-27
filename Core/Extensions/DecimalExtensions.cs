namespace Core.Extensions
{
	public static class DecimalExtensions
	{
		public static string ToRoundedString(this decimal value)
		{
			return value.ToString("N");
		}

	    public static decimal ToDecimal(this object value)
	    {
	        decimal returnvalue = 0;
	        if (value == null)
	        {
	            returnvalue = 0;
	        }
	        else
	        {
	            try
	            {
	                returnvalue = Convert.ToDecimal(value);
	            }
	            catch (Exception)
	            {
	                returnvalue = 0;
	            }
	        }
	        return returnvalue;
	    }
    }
}
