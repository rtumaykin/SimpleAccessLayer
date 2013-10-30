using System.Collections.Generic;

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public class ConstantData
	{
		public string Key { get; private set; }
		public string Value { get; private set; }
		public IList<ConstantData> Children { get; private set; }
		public ConstantData(string key, string value)
		{
			Key = key;
			Value = value;
			Children = new List<ConstantData>();
		}
	}
}
