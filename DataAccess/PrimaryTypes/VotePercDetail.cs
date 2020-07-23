using Common;
using Common.Attributes;
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



        //public int PaguthiTypeId { get; set; }

        public int BoothNo { get; set; }

        /// <summary>
        /// நமது  ஒன்றியத்தின் பிரிவுகள் - எ.கா. RMD, TK, TM
        /// </summary>
        public PaguthiEnum PaguthiEnum { get; set; }
        public int Total { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Third { get; set; }

        public int to20 { get; set; }
        public int to30 { get; set; }
        public int to40 { get; set; }
        public int to50 { get; set; }
        public int to60 { get; set; }
        public int Above60 { get; set; }


        public decimal MaleP { get; set; }
        public decimal FemaleP { get; set; }
        public decimal ThirdP { get; set; }

        public decimal to20P { get; set; }
        public decimal to30P { get; set; }
        public decimal to40P { get; set; }
        public decimal to50P { get; set; }
        public decimal to60P { get; set; }
        public decimal Above60P { get; set; }

        public int AssemblyNo { get; set; }
        public int OndriumNo { get; set; }
        public int PanchayatNo { get; set; }
        public PaguthiType PaguthiType { get; set; }


        public static List<VotePercDetail> GetAll()
        {
            return ReadFileAsObjects<VotePercDetail>(JsonFilePath);
        }

        public static List<VotePercDetail> GetForAssembly(int assemblyNo)
        {
            var allData = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
            return allData.Where(w => w.AssemblyNo == assemblyNo).OrderBy(o => o.BoothNo).ToList();
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


                data.ForEach(u =>
                {

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


        public static void UpdatePaguthiEnum(VotePercDetail vpd, string cellValue)
        {

            try
            {

                var pe = (PaguthiEnum)Enum.Parse(typeof(PaguthiEnum), cellValue);

                List<VotePercDetail> list = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
                var data = list.Where(c => c.AssemblyNo == vpd.AssemblyNo && c.BoothNo == vpd.BoothNo).First();

                data.PaguthiEnum = pe;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdatePaguthiType(BaseData bd, PaguthiEnum pe)
        {
            try
            {
                //var pt = (PaguthiType)Enum.Parse(typeof(PaguthiType), cellValue);

                List<VotePercDetail> list = ReadFileAsObjects<VotePercDetail>(JsonFilePath);
                var data = list.Where(c => c.AssemblyNo == 211 && c.PaguthiEnum  == pe).ToList();

                data.ForEach(fe => {
                  fe.PaguthiType = PaguthiType.P;
                });
                

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void UpdatePaguthiDetails(int assemblyId, int ondriumId, int panchayatId, PaguthiType pt, int boothNo)
        {

            UpdatePaguthiDetails(assemblyId, ondriumId, panchayatId, pt, new List<int>() { boothNo });
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

    public enum PaguthiEnum
    {
        NONE = 0,
        [StringValue("இராமநாதபுரம் நகர்")]
        RMD = 1,
        [StringValue("இராமேஸ்வரம் நகர்")]
        RMM = 2,
        [StringValue("கீழக்கரை நகர்")]
        KEE = 3,
        [StringValue("மண்டபம் பேரூர்")]
        MANP = 4,
        [StringValue("மண்டபம் கிழக்கு")]
        MK = 5,
        [StringValue("மண்டபம் மேற்கு")]
        MM = 6,
        [StringValue("திருப்புல்லாணி கிழக்கு")]
        TK = 7,
        [StringValue("திருப்புல்லாணி மேற்கு")]
        TM = 8
    }

    public enum PaguthiType
    {
        O,   // Ondrium
        P,  // Perooratchi
        N,  // Nagaratchi
        M,  // Managaratchi

    }

}
