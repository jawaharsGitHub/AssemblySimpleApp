using System.IO;

namespace Common
{
    public static class FileContentReader
    {
        public static string EmailBodyHtml { get; } = GetHtmlFileContent("EmailBody.htm");

        private static string GetHtmlFileContent(string fileName)
        {
            var dataFolder = General.GetDataFolder("AssemblySimpleApp\\bin\\Debug", "Common\\HTMLTemplate\\");
            return File.ReadAllText($"{dataFolder}{fileName}");
        }

    }
}
