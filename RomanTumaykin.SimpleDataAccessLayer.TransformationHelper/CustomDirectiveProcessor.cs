using Microsoft.VisualStudio.TextTemplating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanTumaykin.SimpleDataAccessLayer.TransformationHelper
{
	public class CustomDirectiveProcessor : DirectiveProcessor
	{
		public override void FinishProcessingRun()
		{
		}

		public override string GetClassCodeForProcessingRun()
		{
			return "";
		}

		public override string[] GetImportsForProcessingRun()
		{
			return new string[] { "RomanTumaykin.SimpleDataAccessLayer.Transformation" };
		}

		public override string GetPostInitializationCodeForProcessingRun()
		{
			return "";
		}

		public override string GetPreInitializationCodeForProcessingRun()
		{
			return "";
		}

		public override string[] GetReferencesForProcessingRun()
		{
			return new string[] { this.GetType().Assembly.Location };
		}

		public override bool IsDirectiveSupported(string directiveName)
		{
			return !String.IsNullOrWhiteSpace(directiveName) && directiveName == "IncludeHelperAssembly";
		}

		public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
		{
			// do nothing - all I need is to add using and include assembly
		}
	}
}
