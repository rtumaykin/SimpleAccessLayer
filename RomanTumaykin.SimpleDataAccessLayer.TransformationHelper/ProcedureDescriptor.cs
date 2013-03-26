using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanTumaykin.SimpleDataAccessLayer.Transformation
{
	public class ProcedureParameter
	{
		private string parameterName;
		public string ParameterName { get { return parameterName; } }

		private int maxByteLength;
		public int MaxByteLength { get { return maxByteLength; } }

		private byte precision;
		public byte Precision { get { return precision; } }

		private byte scale;
		public byte Scale { get { return scale; } }

		private bool isOutputParameter;
		public bool IsOutputParameter { get { return isOutputParameter; } }

		private string sqlTypeName;
		public string SqlTypeName { get { return sqlTypeName; } }

		public string ClrTypeName { get { return Tools.ClrTypeName(sqlTypeName); } }

		public ProcedureParameter(string parameterName, int maxByteLength, byte precision, byte scale, bool isOutputParameter, string sqlTypeName)
		{
			this.parameterName = parameterName;
			this.maxByteLength = maxByteLength;
			this.precision = precision;
			this.scale = scale;
			this.isOutputParameter = isOutputParameter;
			this.sqlTypeName = sqlTypeName;
		}
	}

	public class ProcedureResultSetColumn
	{
		private string columnName;
		public string ColumnName { get { return columnName; } }

		private string clrTypeName;
		public string ClrTypeName { get { return clrTypeName; } }

		public ProcedureResultSetColumn(string columnName, string clrTypeName)
		{
			this.columnName = columnName;
			if ("System.Int64 System.Boolean System.DateTime System.DateTimeOffset System.Decimal System.Double Microsoft.SqlServer.Types.SqlHierarchyId System.Int32 System.Single System.Int16 System.TimeSpan System.Byte System.Guid".Split(' ').Contains(clrTypeName))
			{
				clrTypeName += "?";
			}
			this.clrTypeName = clrTypeName;
		}
	}

	public class ProcedureDescriptor
	{
		private string objectSchemaName;
		public string ObjectSchemaName { get { return objectSchemaName; } }

		private string objectName;
		public string ObjectName { get { return objectName; } }

		public string FullObjectName { get { return Tools.QuoteName(objectSchemaName) + "." + Tools.QuoteName(objectName); } }

		private string alias;
		public string Alias { get { return alias; } }

		public ProcedureDescriptor(string objectSchemaName, string objectName, string alias)
		{
			this.objectName = objectName;
			this.objectSchemaName = objectSchemaName;
			this.alias = alias;
		}
	}
}
