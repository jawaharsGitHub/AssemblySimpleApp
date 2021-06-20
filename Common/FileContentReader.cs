using System.IO;

namespace Common
{
    public static class FileContentReader
    {
        public static string EmailBodyHtml { get; } = GetHtmlFileContent("EmailBody.htm");
        public static string DataPageHtml { get; } = GetHtmlFileContent("adangal-DataPage.htm");

        public static string pageListHtml { get; } = GetHtmlFileContent("adangal-PageList.htm");

        public static string MemberContactHtml { get; } = GetHtmlFileContent("MemberContact.htm");


        public static string FirstPageTemplate { get; } = GetHtmlFileContent("FirstPageTemplate.htm");

        public static string MainHtml { get; } = GetHtmlFileContent("MainHtml.htm");

        public static string LeftPageTableTemplate { get; } = GetHtmlFileContent("LeftPageTableTemplate.htm");
        
        public static string RightPageTableTemplate { get; } = GetHtmlFileContent("RightPageTableTemplate.htm");

        public static string LeftPageRowTemplate { get; } = GetHtmlFileContent("LeftPageRowTemplate.htm");
        public static string RightPageRowTemplate { get; } = GetHtmlFileContent("RightPageRowTemplate.htm");

        public static string LeftPageTotalTemplate { get; } = GetHtmlFileContent("LeftPageTotalTemplate.htm");
        public static string RightPageTotalTemplate { get; } = GetHtmlFileContent("RightPageTotalTemplate.htm");
        


        public static string LeftPageCertTableTemplate { get; } = GetHtmlFileContent("LeftPageCertTableTemplate.htm");
        public static string RightPageTableCertTemplate { get; } = GetHtmlFileContent("RightPageTableCertTemplate.htm");

        public static string PageTotalTableTemplate { get; } = GetHtmlFileContent("PageTotalTableTemplate.htm");
        public static string PageTotalRowTemplate { get; } = GetHtmlFileContent("PageTotalRowTemplate.htm");

        public static string PageOverallTotalTableTemplate { get; } = GetHtmlFileContent("PageOverallTotalTableTemplate.htm");
        public static string PageOverallTotalRowTemplate { get; } = GetHtmlFileContent("PageOverallTotalRowTemplate.htm");


        private static string GetHtmlFileContent(string fileName)
        {
            // var dataFolder = General.GetDataFolder("AssemblySimpleApp\\bin\\Debug", "Common\\HTMLTemplate\\");

            var dataFolder = General.GetDataFolder("NTK_support\\bin\\Debug", "Common\\HTMLTemplate\\");
            return File.ReadAllText($"{dataFolder}{fileName}");
        }

    }
}
