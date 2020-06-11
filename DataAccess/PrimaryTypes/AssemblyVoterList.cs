using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class VoterList : BaseClass
    {
        public int PageNo { get; set; }

        public int RowNo { get; set; }
        public int SNo { get; set; }

        public string Name { get; set; }
        public string HorFName { get; set; }

        public string HomeAddress { get; set; }

        public int Age { get; set; }

        public string Sex { get; set; }



        public bool IsDeleted { get; set; }

        public bool MayError { get; set; }

        public bool IsManualEdit { get; set; }

        public override string ToString()
        {
            return $"{Name}-{HorFName}-{HomeAddress}-{Age}-{Sex}";
        }


        public static void Save(List<VoterList> voterList, string path)
        {

            try
            {
                WriteObjectsToFile(voterList, path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<VoterList> GetAll(string path)
        {

            try
            {
                return ReadFileAsObjects<VoterList>(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateVoterDetails(VoterList updatedTransaction, string jsonFilePath)
        {
            try
            {
                List<VoterList> list = ReadFileAsObjects<VoterList>(jsonFilePath);

                var u = list.Where(c => c.PageNo == updatedTransaction.PageNo && c.SNo == updatedTransaction.SNo).First();

                u.Name = updatedTransaction.Name;
                u.HorFName = updatedTransaction.HorFName;
                u.HomeAddress = updatedTransaction.HomeAddress;
                u.Age = updatedTransaction.Age;
                u.Sex = SetGender(u.Sex,  updatedTransaction.Sex), //.Trim().ToLower() == "m" ? "ஆண்" : (updatedTransaction.Sex.Trim().ToLower() == "f" ? "பெண்" : "other");
                u.IsManualEdit = true;
                u.IsDeleted = updatedTransaction.IsDeleted;
                u.MayError = updatedTransaction.MayError;


                WriteObjectsToFile(list, jsonFilePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string SetGender(string oldsex, string newSex)
        {
            var s = newSex.Trim().ToLower();

            if (oldsex.Trim().ToLower() == s)
            {
                return oldsex;
            }
            else
            {
                if (s == "m")
                    return "ஆண்";
                else if (s == "f")
                    return "பெண்";
                else
                    return "Third";
            }

        }


    }
}
