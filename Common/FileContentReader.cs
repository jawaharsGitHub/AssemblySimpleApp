using System.IO;

namespace Common
{
    public static class FileContentReader
    {
        public static string EmailBodyHtml { get; } = GetHtmlFileContent("EmailBody.htm");
        public static string DataPageHtml { get; } = GetHtmlFileContent("adangal-DataPage.htm");

        public static string pageListHtml { get; } = GetHtmlFileContent("adangal-PageList.htm");

        public static string MemberContactHtml { get; } = GetHtmlFileContent("MemberContact.htm");


        private static string GetHtmlFileContent(string fileName)
        {
            // var dataFolder = General.GetDataFolder("AssemblySimpleApp\\bin\\Debug", "Common\\HTMLTemplate\\");

            var dataFolder = General.GetDataFolder("NTK_support\\bin\\Debug", "Common\\HTMLTemplate\\");
            return File.ReadAllText($"{dataFolder}{fileName}");
        }

    }
}
