using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethod
{
    public static class PdfExtension
    {

        public static string GetPdfContent(this string fileName)
        {

            //return File.ReadAllText(@"E:\TN GOV\Achunthavayal\text.txt");
            PdfReader reader = new PdfReader(fileName);
            int intPageNum = reader.NumberOfPages;
            string[] words;
            string line;

            //var result = new StringBuilder();
            string result = "";

            var filePath = fileName.Replace(".pdf", ".txt");

            PdfContent page = null;

            File.WriteAllText(filePath, "");

            for (int i = 1; i <= intPageNum; i++)
            {
                //page = new PdfContent();
                //page.PageNo = i;
                //List<string> pdfPageContent = new List<string>();

                var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                File.AppendAllText(filePath, text);

                //result = (result + text);

                //words = text.Split('\n');

                //for (int j = 0, len = words.Length; j < len; j++)
                //{
                //    pdfPageContent.Add(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j])));
                //}

                //page.PageContentLines = pdfPageContent;

                //result.Add(page);

            }

            return File.ReadAllText(filePath);

            // return result.ToString();
        }



        public class PdfContent
        {

            public int PageNo { get; set; }

            public List<string> PageContentLines { get; set; }

        }


    }
}
