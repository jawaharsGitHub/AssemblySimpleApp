using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class TvdMember : BaseClass
    {
        private static string JsonFilePath = AppConfiguration.TvdMemberFile;

        public int Sno { get; set; }
        public string Address { get; set; }
        public string Paguthi { get; set; } = string.Empty;
        public string UtPaguthi { get; set; } = string.Empty;

        public string PaguthiEng { get; set; } = string.Empty;
        public string UtPaguthiEng { get; set; } = string.Empty;
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Assembly { get; set; }


        public string MemberId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public bool MultiplePlace { get; set; }

        public bool WantsMeet { get; set; }

        public bool Money { get; set; }

        public int Vote { get; set; }

        public static List<TvdMember> GetAll()
        {
            return ReadFileAsObjects<TvdMember>(JsonFilePath);
        }

        public static TvdMember GetMember(string mid)
        {
            return GetAll().Where(w => w.MemberId == mid).First();
        }

        public static void AddTvdMembers(List<TvdMember> mem)
        {
            //investment.CreatedDate = DateTime.Today.ToLongTimeString();
            InsertObjectsToJson(JsonFilePath, mem);
        }

        public static void UpdateMeets(string memId, bool wantMee)
        {
                List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

                var u = list.Where(c => c.MemberId == memId).First();
                u.WantsMeet = wantMee;

                WriteObjectsToFile(list, JsonFilePath);
            
        }

        public static void UpdateMoney(string memId, bool money)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

            var u = list.Where(c => c.MemberId == memId).First();
            u.Money = money;

            WriteObjectsToFile(list, JsonFilePath);

        }

        public static void UpdateVotes(string memId, int votes)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);
            var u = list.Where(c => c.MemberId == memId).First();
            u.Vote = votes;

            WriteObjectsToFile(list, JsonFilePath);

        }


        public static void UpdateMemberDetails(string memberId, string Paguthi, string utPaguthi, string pagEng, string utPagEng)
        {
            try
            {

                List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

                var u = list.Where(c => c.MemberId == memberId).First();

                if (u.UtPaguthi.Trim() == string.Empty)
                {
                    u.Paguthi = Paguthi;
                    u.UtPaguthi = utPaguthi;
                    u.PaguthiEng = pagEng;
                    u.UtPaguthiEng = utPagEng;

                    WriteObjectsToFile(list, JsonFilePath);
                }


                //if (u.UtPaguthi.Trim() == string.Empty)
                //{
                //    u.Paguthi = Paguthi;
                //    u.UtPaguthi = utPaguthi;
                //    u.PaguthiEng = pagEng;
                //    u.UtPaguthiEng = utPagEng;
                //}
                //else
                //{
                //    u.Paguthi = Paguthi;
                //    u.UtPaguthi = $"{u.UtPaguthi},{utPaguthi}";
                //    u.PaguthiEng = pagEng;
                //    u.UtPaguthiEng = $"{u.UtPaguthiEng},{utPagEng}";
                //    u.MultiplePlace = true;
                //}

                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetCount()
        {
            try
            {

                List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

                var tr = list.Count();

                var erc = list.Where(c => c.UtPaguthi.Trim() == string.Empty).Count();

                return $"TR: {tr} ERC: {erc}";

                //WriteObjectsToFile(list, JsonFilePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ClearAllUpdate()
        {
            try
            {

                List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

                list.ForEach(u =>
                {

                    u.Paguthi = "";
                    u.UtPaguthi = "";
                    u.PaguthiEng = "";
                    u.UtPaguthiEng = "";

                });




                WriteObjectsToFile(list, JsonFilePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
