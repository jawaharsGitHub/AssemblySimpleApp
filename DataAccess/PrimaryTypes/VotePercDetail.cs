using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class VotePercDetail : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.VotePercDetailFile;

        public int AssemblyNo { get; set; }

        public int OndriumNo { get; set; }

        public int PanchayatNo { get; set; }

        public PaguthiType PaguthiType { get; set; }

        //public int PaguthiTypeId { get; set; }

        public int BoothNo { get; set; }

        public int Total { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Third { get; set; }

        public decimal MaleP { get; set; }
        public decimal FemaleP { get; set; }
        public decimal ThirdP { get; set; }

        public decimal to20 { get; set; }
        public decimal to30 { get; set; }
        public decimal to40 { get; set; }

        public decimal to50 { get; set; }
        public decimal to60 { get; set; }
        public decimal Above60 { get; set; }


        public static List<VotePercDetail> GetAll()
        {
            return ReadFileAsObjects<VotePercDetail>(JsonFilePath);
        }

        public static List<VotePercDetail> GetForAssembly(int assemblyNo)
        {
            var allData = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
            return allData.Where(w => w.AssemblyNo == assemblyNo).ToList();
        }

        public static List<VotePercDetail> GetForOndrium(int ondriumId)
        {
            var allData = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
            return allData.Where(w => w.OndriumNo == ondriumId).ToList();
        }

        public static List<VotePercDetail> GetForBooth(int boothId)
        {
            var allData = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
            return allData.Where(w => w.BoothNo == boothId).ToList();
        }

        public static void UpdatePaguthiDetails(int assemblyId, int ondriumId, int panchayatId, PaguthiType pt, List<int> boothNos)
        {

            try
            {
                List<VotePercDetail> list = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
                var data = list.Where(c => c.AssemblyNo == assemblyId && boothNos.Contains(c.BoothNo)).ToList();


                data.ForEach(u => {

                    u.PaguthiType = pt;
                    u.OndriumNo = ondriumId;
                    u.PanchayatNo = panchayatId;

                });


                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Save(VotePercDetail bd)
        {

            try
            {
                List<VotePercDetail> list = ReadFileAsObjects<VotePercDetail>(JsonFilePath);

                var u = list.Any(c => c.AssemblyNo == bd.AssemblyNo && c.BoothNo == bd.BoothNo);

                if (u)
                {
                    Delete(bd.AssemblyNo, bd.BoothNo);
                }

                InsertSingleObjectToListJson<VotePercDetail>(JsonFilePath, bd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Delete(int assNo, int boothNo)
        {
            try
            {
                var list = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
                list.RemoveAll((c) => c.AssemblyNo == assNo && c.BoothNo == boothNo);
                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    //public enum Paguthi
    //{
    //    Nagaratchi,
    //    Perooratchi,
    //    Ondrium,

    //}

    public enum PaguthiType
    {
        O,   // Ondrium
        P,  // Perooratchi
        N,  // Nagaratchi
        M,  // Managaratchi

    }

}
