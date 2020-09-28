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
        public string Paguthi { get; set; }
        public string UtPaguthi { get; set; }

        public string PaguthiEng { get; set; }
        public string UtPaguthiEng { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Assembly { get; set; }
       

        public string MemberId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public static List<TvdMember> GetAll()
        {
            return ReadFileAsObjects<TvdMember>(JsonFilePath);
        }

        public static void UpdateMemberDetails(string memberId, string Paguthi, string utPaguthi, string pagEng, string utPagEng)
        {
            try
            {

                List<TvdMember> list = ReadFileAsObjects<TvdMember>(JsonFilePath);

                var u = list.Where(c => c.MemberId == memberId).First();

                u.Paguthi = Paguthi;
                u.UtPaguthi = utPaguthi;
                u.PaguthiEng = pagEng;
                u.UtPaguthiEng = utPagEng;

                WriteObjectsToFile(list, JsonFilePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
