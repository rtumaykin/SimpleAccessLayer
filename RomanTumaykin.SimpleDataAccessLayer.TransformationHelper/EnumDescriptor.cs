using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanTumaykin.SimpleDataAccessLayer.Transformation
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
