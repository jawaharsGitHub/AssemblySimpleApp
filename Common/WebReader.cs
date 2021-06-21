using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public class WebReader
    {

        public static string CallHttpWebRequest(string URL)
        {

            string sAddress = URL;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sAddress);
            req.Accept = "text/xml,text/plain,text/html";
            req.Method = "GET";
            HttpWebResponse result = (HttpWebResponse)req.GetResponse();
            Stream ReceiveStream = result.GetResponseStream();
            StreamReader reader = new StreamReader(ReceiveStream, System.Text.Encoding.ASCII);
            string respHTML = reader.ReadToEnd();
            //Response.Write(respHTML);
            reader.Close();
            return respHTML;

        }


        public static string xmlTojson(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return JsonConvert.SerializeXmlNode(doc);
        }


        public static List<ComboData> xmlToDynamic(string xml)
        {
            try
            {
                List<ComboData> data = new List<ComboData>();
                dynamic taluks = JObject.Parse(xmlTojson(xml));

                var d = ((JArray)taluks["root"]["taluk"]).ToList();

                for (int i = 0; i < d.Count - 1; i++)
                {
                    //var tc = ((JObject)d[i])["talukcode"];
                    //var tn = ((JObject)d[i])["talukname"];

                    data.Add(new ComboData()
                    {

                        Value = Convert.ToInt32((((JObject)d[i])["talukcode"]).ToString()),
                        Display = (((JObject)(((JObject)d[i])["talukname"]))["#cdata-section"]).ToString().Trim()
                    });
                }

                    return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
