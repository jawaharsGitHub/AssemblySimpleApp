using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class MemberVerify : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.MemberVerifyFile;
        private static string JsonFilePathNonVerify = AppConfiguration.MemberNonVerifyFile;

        public string Email { get; set; }
        public string Mid { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        

        public static List<MemberVerify> GetAllVerified()
        {
            return ReadFileAsObjects<MemberVerify>(JsonFilePath);
        }

        public static List<MemberVerify> GetAllNonVerified()
        {
            return ReadFileAsObjects<MemberVerify>(JsonFilePathNonVerify);
        }
    }
}
