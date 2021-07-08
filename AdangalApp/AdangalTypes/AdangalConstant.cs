using Common;
using System.IO;

namespace AdangalApp
{
    public static class AdangalConstant
    {
        
        public static readonly string dataPath = General.GetDataFolder("data");
        public static string villageName;
        //public static readonly string ResultPath = CreateAndReadPath("Result");//General.GetDataFolder("Result");
        //public static readonly string LogPath = CreateAndReadPath($"{villageName}-Log");

        public static string CreateAndReadPath(string folderName)
        {
            var path = AppConfiguration.GetDynamicPath($"AdangalJson/{villageName}/{folderName}");

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            return path;
        }

        public static string CreateAndReadPath(string folderName, string _villageName)
        {
            var path = AppConfiguration.GetDynamicPath($"AdangalJson/{_villageName}/{folderName}");

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
