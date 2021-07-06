using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace Common
{
    public class WebReader
    {

        public static string CallHttpWebRequest(string URL)
        {
            if (General.CheckForInternetConnection() == false) return null;
            string sAddress = URL;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sAddress);
            req.Accept = "text/xml,text/plain,text/html";
            req.Method = "GET";
            HttpWebResponse result = (HttpWebResponse)req.GetResponse();
            Stream ReceiveStream = result.GetResponseStream();
            StreamReader reader = new StreamReader(ReceiveStream, System.Text.Encoding.ASCII);
            string respHTML = reader.ReadToEnd();
            reader.Close();
            return respHTML;

        }


        public static string xmlTojson(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return JsonConvert.SerializeXmlNode(doc);
        }


        public static List<ComboData> xmlToDynamic(string xml, string key)
        {
            try
            {
                List<ComboData> data = new List<ComboData>();
                dynamic taluks = JObject.Parse(xmlTojson(xml));
                //dynamic taluksTamil = JObject.Parse(xmlTojson(xmlTamil));

                var d = ((JArray)taluks["root"][key]).ToList();
                //var dTamil = ((JArray)taluksTamil["root"][key]).ToList();

                for (int i = 0; i <= d.Count - 1; i++)
                {
                    data.Add(new ComboData()
                    {
                        Value = Convert.ToInt32((((JObject)d[i])[key + "code"]).ToString()),
                        Display = (((JObject)(((JObject)d[i])[key + "name"]))["#cdata-section"]).ToString().Trim(),
                        DisplayTamil = (((JObject)(((JObject)d[i])[key + "name"]))["#cdata-section"]).ToString().Trim()
                    });
                }

                data.Insert(0,new ComboData() { Value = -1, Display = "--select--" });
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<string> GetSubdivisions(string xml, string key)
        {
            try
            {
                List<string> subdivisions = new List<string>();
                dynamic taluks = JObject.Parse(xmlTojson(xml));

                if ((((JObject)taluks["root"])["flag"]).ToString() == "false")
                {
                    return null;
                }

                List<JToken> d = new List<JToken>();

                var isFullfilled = (taluks["root"][key]).ToString().Contains("-");

                if (isFullfilled)
                    return new List<string>() { "-" };

                if (((JContainer)taluks["root"][key]).Count > 1)
                {
                    d = ((JArray)taluks["root"][key]).ToList();
                }
                else if (((JContainer)taluks["root"][key]).Count == 1)
                {
                    d.Add(((JObject)taluks["root"][key]));
                }


                for (int i = 0; i <= d.Count - 1; i++)
                    subdivisions.Add((((JObject)d[i])[key + "code"]).ToString());

                return subdivisions;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
