﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace TabularEditor.Dax
{
    public class DaxFormatterError
    {
        public int line;
        public int column;
        public string message;
    }

    public class DaxFormatterRequest
    {
        public string Dax { get; set; }
        public char ListSeparator { get; set; }
        public char DecimalSeparator { get; set; }
        public string CallerApp { get; set; }
        public string CallerVersion { get; set; }

        public DaxFormatterRequest()
        {
            this.ListSeparator = ',';
            this.DecimalSeparator = '.';

            // Save caller app and version
            var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName();
            this.CallerApp = assemblyName.Name;
            this.CallerVersion = assemblyName.Version.ToString();
        }
    }


    public class DaxFormatterResult
    {
        [JsonProperty(PropertyName = "formatted")]
        public string FormattedDax;
        public List<DaxFormatterError> errors;
    }

    public class DaxFormatterProxy
    {
        public const string DaxTextFormatUri = "http://daxtest02.azurewebsites.net/api/daxformatter/DaxFormat";
        public const int DaxFormatterRequestTimeout = 10;

        private static string redirectUrl = null;  // cache the redirected URL
        private static string redirectHost = null;

        public static DaxFormatterResult FormatDax(string query)
        {
            string output = CallDaxFormatter(DaxTextFormatUri, query);
            var res2 = new DaxFormatterResult();
            if (output.StartsWith("{"))
            {
                JsonConvert.PopulateObject(output, res2);
            } else
            {
                res2.FormattedDax = JsonConvert.DeserializeObject<string>(output);
            }
            return res2;
        }

        private static string CallDaxFormatter(string uri, string query)
        {
            try
            {

                DaxFormatterRequest req = new DaxFormatterRequest();
                req.Dax = query;

                var data = JsonConvert.SerializeObject(req);

                var enc = System.Text.Encoding.UTF8;
                var data1 = enc.GetBytes(data);

                // this should allow DaxFormatter to work through http 1.0 proxies
                // see: http://stackoverflow.com/questions/566437/http-post-returns-the-error-417-expectation-failed-c
                //System.Net.ServicePointManager.Expect100Continue = false;

                var wr = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
                wr.Proxy = WebRequest.GetSystemWebProxy();

                wr.Timeout = DaxFormatterRequestTimeout * 1000;
                wr.ContentType = "application/json";
                wr.Method = "POST";
                wr.Accept = "application/json, text/javascript, */*; q=0.01";
                wr.Headers.Add("Accept-Encoding", "gzip,deflate");
                wr.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                wr.ContentType = "application/json; charset=UTF-8";
                wr.AutomaticDecompression = DecompressionMethods.GZip;

                string output = "";
                using (var strm = wr.GetRequestStream())
                {
                    strm.Write(data1, 0, data1.Length);

                    using (var resp = wr.GetResponse())
                    {
                        //var outStrm = new System.IO.Compression.GZipStream(resp.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                        var outStrm = resp.GetResponseStream();
                        using (var reader = new System.IO.StreamReader(outStrm))
                        {
                            output = reader.ReadToEnd();
                        }
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
        }
    }
}