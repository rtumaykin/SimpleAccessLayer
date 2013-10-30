namespace RomanTumaykin.SimpleDataAccessLayer
{
	public class EnumData
	{
		private string key;
		public string Key { get { return key; } }

		private long value;
		public long Value { get { return value; } }

		public EnumData(string key, long value)
		{
			this.key = key;
			this.value = value;
		}
	}
}
