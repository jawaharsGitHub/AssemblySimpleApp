﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public class VoterList : BaseClass
    {

        public string Name { get; set; }
        public string HorFName { get; set; }

        public string HomeAddress { get; set; }

        public int Age { get; set; }

        public string Sex { get; set; }

        public int PageNo { get; set; }

        public int RowNo { get; set; }

        public bool IsDeleted { get; set; }

        public bool MayError { get; set; }

        public override string ToString()
        {
            return $"{Name}-{HorFName}-{HomeAddress}-{Age}-{Sex}";
        }


        public static void Save(List<VoterList>  voterList, string path)
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
    }
}
