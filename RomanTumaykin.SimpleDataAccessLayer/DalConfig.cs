using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RomanTumaykin.SimpleDataAccessLayer
{
	public enum AuthenticationType
	{
		Windows = 0,
		Sql = 1
	}
	//
	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "Authentication")]
	[KnownType(typeof(SqlAuthentication))]
	[KnownType(typeof(WindowsAuthentication))]
	public abstract class Authentication
	{
		private AuthenticationType type;
		public Authentication(AuthenticationType type)
		{
			this.type = type;
		}
	}
	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "SqlAuthentication")]
	public class SqlAuthentication : Authentication
	{
		private string userName, password;
		[DataMember(IsRequired = true)]
		public string UserName
		{
			get { return userName; }
			set { userName = value; }
		}

		[DataMember(IsRequired = true)]
		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		public SqlAuthentication(string userName, string password)
			: base(AuthenticationType.Sql)
		{
			this.userName = userName;
			this.password = password;
		}
	}

	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "WindowsAuthentication")]
	public class WindowsAuthentication : Authentication
	{
		public WindowsAuthentication()
			: base(AuthenticationType.Windows)
		{ }
	}
	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "DesignerConnection")]
	public class DesignerConnection
	{
		[DataMember]
		public Authentication Authentication { get; set; }
	}

	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "Enum")]
	public class Enum
	{
		[DataMember(IsRequired = true)]
		public string Schema { get; set; }
		[DataMember(IsRequired = true)]
		public string TableName { get; set; }
		[DataMember(IsRequired = true)]
		public string KeyColumn { get; set; }
		[DataMember(IsRequired = true)]
		public string ValueColumn { get; set; }
		[DataMember(IsRequired = true)]
		public string Alias { get; set; }
	}
	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "Constant")]
	public class Constant
	{
        [DataMember(IsRequired = true)]
        public string Schema { get; set; }
        [DataMember(IsRequired = true)]
        public string TableName { get; set; }
        [DataMember(IsRequired = true)]
        public string KeyColumn { get; set; }
        [DataMember(IsRequired = true)]
        public string ValueColumn { get; set; }
        [DataMember(IsRequired = true)]
        public string Alias { get; set; }
        [DataMember(IsRequired = true)]
        public bool IsExplicitlySelected { get; set; }
	}

	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "Procedure")]
	public class Procedure
	{
		[DataMember(IsRequired = true)]
		public string Schema { get; set; }
		[DataMember(IsRequired = true)]
		public string ProcedureName { get; set; }
		[DataMember(IsRequired = false)]
		public string Alias { get; set; }
	}

	[DataContract(Namespace = "RomanTumaykin.SimpleDataAcessLayer", Name = "DalConfig")]
	[KnownType(typeof(Enum))]
	[KnownType(typeof(Authentication))]
	[KnownType(typeof(DesignerConnection))]
	[KnownType(typeof(WindowsAuthentication))]
	[KnownType(typeof(SqlAuthentication))]
	[KnownType(typeof(Constant))]
    [KnownType(typeof(Procedure))]
    public class DalConfig
	{
		[DataMember(IsRequired = true)]
		public DesignerConnection DesignerConnection { get; set; }
		[DataMember(IsRequired = true)]
		public String Namespace { get; set; }
		[DataMember(IsRequired = true)]
		public String ApplicationConnectionString { get; set; }
		[XmlElement("Enums")]
		[DataMember(IsRequired = true)]
		public List<Enum> Enums { get; set; }
		[XmlElement("Procedures")]
		[DataMember(IsRequired = true)]
		public List<Procedure> Procedures { get; set; }
        [XmlElement("Constants")]
        [DataMember(IsRequired = true)]
        public List<Constant> Constants { get; set; }

        public DalConfig()
        {
            this.Constants = new List<Constant>();
            this.Procedures = new List<Procedure>();
            this.Enums = new List<Enum>();
        }
	}
}
