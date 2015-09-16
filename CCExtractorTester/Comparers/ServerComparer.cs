﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using CCExtractorTester.Analyzers;

namespace CCExtractorTester.Comparers
{
    public class ServerComparer : IFileComparable
    {
        private string reportUrl;

        public ServerComparer(string reportUrl)
        {
            this.reportUrl = reportUrl;
        }

        public void CompareAndAddToResult(CompareData data)
        {
            // Check for equality by hash
            if (!Hasher.filesAreEqual(data.CorrectFile, data.ProducedFile))
            {
                // Upload result
                using (var wb = new WebClient())
                {
                    // TODO: check if this works
                    var response = wb.UploadFile(reportUrl, data.ProducedFile);
                }
            }
            else
            {
                // Post equality status
                using (var wb = new WebClient())
                {
                    var d = new NameValueCollection();
                    d["equal"] = data.ProducedFile;
                    d["sample"] = data.SampleFile;

                    var response = wb.UploadValues(reportUrl, "POST", d);
                }
            }
        }

        public int GetSuccessNumber()
        {
            return -1; // Cannot be implemented in this class
        }

        public string SaveReport(string pathToFolder, ResultData data)
        {
            // Do nothing
            return "";
        }

        public void SendExitCodeAndRuntime(RunData rd, string sample)
        {
            // Post equality status
            using (var wb = new WebClient())
            {
                var d = new NameValueCollection();
                d["exitCode"] = rd.ExitCode+"";
                d["runTime"] = rd.Runtime+"";
                d["sample"] = sample;

                var response = wb.UploadValues(reportUrl, "POST", d);
            }
        }
    }
}
