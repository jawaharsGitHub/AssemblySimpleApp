using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public bool JustTalk { get; set; }

        public bool VVIP { get; set; }

        public bool MultiplePlace { get; set; }

        public bool WantsMeet { get; set; }

        public bool Money { get; set; }

        public int Vote { get; set; }

        public bool IsFemale { get; set; }

        public DateTime UpdatedTime { get; set; }

        [JsonIgnore]
        public bool NeedUpdatePagEng { get; set; }

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

        public static void BulkUpdateFemaleFlag(List<string> femMemIds)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

            var u = list.Where(c => femMemIds.Contains(c.MemberId)).ToList();

            u.ForEach(fe => { fe.IsFemale = true; });

            WriteObjectsToFile(list, JsonFilePath);

        }

        public static void BulkUpdatePaguthiEng(List<TvdMember> mems)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

            mems.ForEach(fe => {
                var d = list.Where(w => w.MemberId == fe.MemberId).First();

                d.Paguthi = fe.Paguthi;
                d.PaguthiEng = fe.PaguthiEng;
            });

            WriteObjectsToFile(list, JsonFilePath);

        }

        public static void UpdateMeets(string memId, bool wantMee)
        {
                List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

                var u = list.Where(c => c.MemberId == memId).First();
                u.WantsMeet = wantMee;
                u.UpdatedTime = DateTime.Now;

                WriteObjectsToFile(list, JsonFilePath);
            
        }

        public static void UpdateVVIP(string memId, bool vvip)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

            var u = list.Where(c => c.MemberId == memId).First();
            u.VVIP = vvip;
            u.UpdatedTime = DateTime.Now;

            WriteObjectsToFile(list, JsonFilePath);

        }

        public static void UpdateMoney(string memId, bool money)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

            var u = list.Where(c => c.MemberId == memId).First();
            u.Money = money;
            u.UpdatedTime = DateTime.Now;

            WriteObjectsToFile(list, JsonFilePath);

        }

        public static void UpdateVotes(string memId, int votes)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);
            var u = list.Where(c => c.MemberId == memId).First();
            u.Vote = votes;
            u.UpdatedTime = DateTime.Now;

            WriteObjectsToFile(list, JsonFilePath);

        }

        public static void UpdateTalk(string memId, bool votes)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);
            var u = list.Where(c => c.MemberId == memId).First();
            u.JustTalk = votes;
            u.UpdatedTime = DateTime.Now;

            WriteObjectsToFile(list, JsonFilePath);

        }

        public static void UpdateUtEngPaguthi(string memId, string utPaguthiEng)
        {
            List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);
            var u = list.Where(c => c.MemberId == memId).First();
            u.UtPaguthiEng = utPaguthiEng;

            WriteObjectsToFile(list, JsonFilePath);

        }


        public static void UpdateMemberDetails(string memberId, string Paguthi, string utPaguthi, string pagEng, string utPagEng)
        {
            try
            {

                List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

                var u = list.Where(c => c.MemberId == memberId).First();

                if (u.UtPaguthiEng == null || u.UtPaguthiEng.Trim() == string.Empty)
                {
                    u.Paguthi = Paguthi;
                    u.UtPaguthi = utPaguthi;
                    u.PaguthiEng = pagEng;
                    u.UtPaguthiEng = utPagEng;

                    WriteObjectsToFile(list, JsonFilePath);
                }
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

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string TotalExpectedVote()
        {
            try
            {

                List<TvdMember> list = GetAll();

                var confirmedVote = list.Where(w => w.Vote > 0).Sum(s => s.Vote);
                var expectedVote = list.Where(w => w.Vote == 0).Sum(s => s.Vote) * 1;

                return (confirmedVote + expectedVote).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public static string MeetCount()
        {
            try
            {

                List<TvdMember> list = GetAll();
                var wantsMeet = list.Where(w => w.WantsMeet).Count();

                return wantsMeet.ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<TvdMember> MeetContact()
        {
            try
            {

                List<TvdMember> list = GetAll();
                var wantsMeet = list.Where(w => w.WantsMeet).ToList();

                return wantsMeet;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string NithiCount()
        {
            try
            {

                List<TvdMember> list = GetAll();
                var Nithi = list.Where(w => w.Money).Count();

                return Nithi.ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<TvdMember> NithiContact()
        {
            try
            {

                List<TvdMember> list = GetAll();
                var Nithi = list.Where(w => w.Money).ToList();

                return Nithi;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool isContacted(TvdMember tvd)
        {
            return tvd.UpdatedTime.ToString()!="01-01-0001 00:00:00";
        }

        public static (string C,string NC) ConAndNot()
        {
            try
            {

                List<TvdMember> list = GetAll();

                var c = list.Where(w => isContacted(w)).Count().ToString();

                var nc = list.Where(w => isContacted(w) == false).Count().ToString();

                return (c, nc);

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
