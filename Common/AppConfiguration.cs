﻿using System;
using System.Configuration;
using System.IO;

namespace Common
{
    public class AppConfiguration
    {

        public static string ZonalFile { get { return GetFullPath("zonal");  } }

        public static string AssemblyFile { get; } = GetFullPath("assembly");

        public static string VotePercDetailFile { get { return GetFullPath("VotePercDetail"); } }

        public static string AssemblyBoothLinkFile { get { return GetFullPath("AssemblyBoothLink"); } }

        public static string BaseDataFile { get; } = GetFullPath("basedata");

        public static string VoterFile { get; } = GetFullPath("voter");

        public static string PanchayatFile { get; } = GetFullPath("panchayat");

        public static string DistrictFile { get; } = GetFullPath("district");

        public static string BlockFile { get; } = GetFullPath("block");

        public static string VoteDataFile { get { return GetFullPath("votedata"); } }

        public static string PollingStationFile { get { return GetFullPath("pollingStation"); } }

        public static string TvdMemberFile { get { return GetFullPath("TvdMemberFile"); } }

        public static string OtherPartyFile { get { return GetFullPath("OtherPartyFile"); } }

        public static string InvestmentFile { get; } = GetFullPath("InvestmentFile");

        public static string ClosedNotesFile { get; } = GetFullPath("ClosedNotesFile");

        public static string ExpenditureFile { get; } = GetFullPath("ExpenditureFile");

        public static string InHandFile { get; } = GetFullPath("InHandAndBankFile");

        public static string DailyTxnFile { get; } = GetFullPath("DailyTxn");

        public static bool usingMenu { get; } = Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]);

        public static string CollectionPerDay { get; } = GetFullPath("CollectionPerDay");

        public static string DailyBatchFile { get; } = GetFullPath("DailyBatchFile");

        public static string OutstandingFile { get; } = GetFullPath("OutstandingFile");

        public static string PetrolFile { get; } = GetFullPath("PetrolFile");

        public static string MemberVerifyFile { get { return GetFullPath("memberverify"); } }

        public static string MemberNonVerifyFile { get { return GetFullPath("ntknonApprovalMembers"); } }

        public static string AssemblyVotersFolder { get { return GetFullPath("AssemblyVotersFolder"); } }


        private static string GetFullPath(string configKey)
        {
            if (ConfigurationManager.AppSettings[configKey] == null) return null;
            return Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings[configKey]);
        }

        public static string GetDynamicPath(string fullPath)
        {
            return General.CombinePath(ConfigurationManager.AppSettings["SourceFolder"], fullPath);
        }

        public static string GetSourceFolderPath()
        {
            return ConfigurationManager.AppSettings["SourceFolder"];
        }

        public static bool AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //Console.WriteLine("Error writing app settings");
                //return false;
            }
        }
    }
}
