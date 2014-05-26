﻿using System;
using System.Text;
using CCExtractorTester.DiffTool;
using System.IO;

namespace CCExtractorTester
{
	public class DiffToolComparer : IFileComparable
	{
		private StringBuilder Builder { get; set; }
		private StringBuilder BuilderDiff { get; set; }
		private SideBySideBuilder Differ { get; set; }
		private int Count { get; set; }
		private bool Reduce { get; set; }

		public DiffToolComparer (bool reduce=false)
		{
			Builder = new StringBuilder ();
			BuilderDiff = new StringBuilder ();
			Differ = new SideBySideBuilder (new DifferTool ());
			Count = 0;
			Reduce = reduce;
		}

		#region IFileComparable implementation

		public string GetReportFileName ()
		{
			return "Report_" + DateTime.Now.ToFileTime () + ".html";
		}

		public void CompareAndAddToResult (CompareData data)
		{
			string oldText = string.Empty;
			using (StreamReader streamReader = new StreamReader(data.CorrectFile, Encoding.UTF8))
			{            
				oldText = streamReader.ReadToEnd();
			}
			string newText = string.Empty;
			using (StreamReader streamReader = new StreamReader(data.ProducedFile, Encoding.UTF8))
			{            
				newText = streamReader.ReadToEnd();
			}

			SideBySideModel sbsm = Differ.BuildDiffModel (oldText, newText);
			int changes = sbsm.GetNumberOfChanges ();
			string onclick = "";
			string clss = "green";
			if (changes > 0) {
				BuilderDiff.Append (sbsm.GetDiffHTML (String.Format (@"style=""display:none;"" id=""{0}""", "entry_" + Count),Reduce));
				onclick = String.Format(@"onclick=""toggle('{0}');""","entry_"+Count);
				clss = "red";
			}
			Builder.AppendFormat (
				@"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td class=""{3}"" {4}>{5}</td></tr>",
				data.SampleFile,
				data.Command,
				data.RunTime.ToString(),
				clss,
				onclick,
				changes);
			Count++;
		}

		public string GetResult (ResultData data)
		{
			string additionalHeader = @"
				<script type=""text/javascript"">
					function toggleNext(elm){
						var next = elm.parentNode.nextElementSibling;
						if(next.style.display == ""none""){
							next.style.display = ""block"";
						} else {
							next.style.display = ""none"";
						}
					}
					function toggle(id){
						var next = document.getElementById(id);
						if(next.style.display == ""none""){
							next.style.display = ""block"";
						} else {
							next.style.display = ""none"";
						}
					}
				</script>
				<style type=""text/css"">
					.green {
						background-color: #00ff00;
					}
					.red {
						background-color: #ff0000;
					}
				</style>";
			string table = @"<table><tr><th>Sample</th><th>Command</th><th>Runtime</th><th>Changes (click to show)</th></tr>{0}</table>";
			string first = @"<p>Report generated for CCExtractor version {0}</p>";
			return SideBySideModel.GetHTML(
				String.Format(first,data.CCExtractorVersion)+String.Format(table,Builder.ToString ())+BuilderDiff.ToString(),
				"Report "+DateTime.Now.ToShortDateString(),
				additionalHeader
			);
		}
		#endregion
	}
}