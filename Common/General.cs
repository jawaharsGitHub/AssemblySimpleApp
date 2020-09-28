
using System;
using System.IO;
using System.Reflection;

namespace Common
{
    public static class General
    {
        public static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static string GetDataFolder(string oldValue, string newValue)
        {
            string exeFile = (new Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);
            string dataFolder = exeDir.Replace("AssemblyApp\\bin\\Debug", newValue);  

            //string dataFolder = exeDir.Replace("NTK_Support\\bin\\Debug", newValue);

            return dataFolder;
        }

        public static void WriteToFile(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllText(path, content);

        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static void CreateFileIfNotExist(string path)
        {
            if (File.Exists(path) == false)
            {
                var myFile = File.Create(path);
                myFile.Close();
            }

        }

        public static void CreateFolderIfNotExist(string folderPath)
        {
            if (Directory.Exists(folderPath) == false)
            {
                var myFile = Directory.CreateDirectory(folderPath);

            }

        }

        public static void RecreateFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath);
            }

            Directory.CreateDirectory(folderPath);

        }



        public static void WriteLog(string path, string content, string assNo, string partNo, int pn)
        {
            CreateFileIfNotExist(path);
            File.AppendAllText(path, $"{assNo}-{partNo}-{pn} : {content}{Environment.NewLine}");
        }

        public static void ReplaceLog(string path, string content)
        {
            CreateFileIfNotExist(path);
            File.WriteAllText(path, $"{content}{Environment.NewLine}");
        }

    }
}
