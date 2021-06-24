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
            PdfReader reader = new PdfReader(fileName);
            int intPageNum = reader.NumberOfPages;

            var filePath = fileName.Replace(".pdf", ".txt");

            File.WriteAllText(filePath, "");

            //for (int i = 1; i <= intPageNum; i++)
            //{
            //    FontProgramRenderFilter fontFilter = new FontProgramRenderFilter();
            //    ITextExtractionStrategy strategy = new FilteredTextRenderListener(
            //            new LocationTextExtractionStrategy(), fontFilter);

            //    var text = PdfTextExtractor.GetTextFromPage(reader, i, strategy);

            //    //var text2 = Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));

            //    //var text2 = PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy());

            //    File.AppendAllText(filePath, text);
            //}


            for (int i = 1; i <= intPageNum; i++)
            {
                var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());
                File.AppendAllText(filePath, text);
            }
            reader.Close();
            return File.ReadAllText(filePath);
        }



        public class PdfContent
        {

            public int PageNo { get; set; }

            public List<string> PageContentLines { get; set; }

        }


    }

    //class FontProgramRenderFilter : RenderFilter
    //{
    //    public override bool AllowText(TextRenderInfo renderInfo)
    //    {
    //        DocumentFont font = renderInfo.GetFont();
    //        PdfDictionary fontDict = font.FontDictionary;
    //        PdfName subType = fontDict.GetAsName(PdfName.SUBTYPE);
    //        if (PdfName.TYPE0.Equals(subType))
    //        {
    //            PdfArray descendantFonts = fontDict.GetAsArray(PdfName.DESCENDANTFONTS);
    //            PdfDictionary descendantFont = descendantFonts[0] as PdfDictionary;
    //            PdfDictionary fontDescriptor = descendantFont.GetAsDict(PdfName.FONTDESCRIPTOR);
    //            PdfStream fontStream = fontDescriptor.GetAsStream(PdfName.FONTFILE2);
    //            byte[] fontData = PdfReader.GetStreamBytes((PRStream)fontStream);
    //            MemoryStream dataStream = new MemoryStream(fontData);
    //            dataStream.Position = 0;
    //            MemoryPackage memoryPackage = new MemoryPackage();
    //            Uri uri = memoryPackage.CreatePart(dataStream);
    //            GlyphTypeface glyphTypeface = new GlyphTypeface(uri);
    //            memoryPackage.DeletePart(uri);
    //            ICollection<string> names = glyphTypeface.FamilyNames.Values;
    //            return names.Where(name => name.Contains("Arial")).Count() > 0;
    //        }
    //        else
    //        {
    //            // analogous code for other font subtypes
    //            return false;
    //        }
    //    }
    //}
}
