namespace upKeeper2Helpdesk_New
{
	public static class FormatHelper
	{
		public static long ConvertToBytes(this string value)
		{
			if (long.TryParse(value.Replace(" GB", ""), out long v))
			{
				return v * 1024 * 1024 * 1024;
			}
			else {
				return 0;
			}
		}
	}
}
