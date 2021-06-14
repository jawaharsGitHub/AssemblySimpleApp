
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace Common
{
    public static class General
    {

        public static (MailMessage, SmtpClient) GetMailMessage(string subject, string mailBody)
        {

            //var myEmail = "ramnadntk211@gmail.com";
            //var fromEmail = "ntkthiruvadanai@gmail.com";
            //var pwd = "ntkthiruvadanai210"; 

            var fromEmail = "jawahar.subramanian83@gmail.com";
            var pwd = "nainamarbus";


            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(fromEmail);
            message.To.Add(new MailAddress(fromEmail));

            message.CC.Add(new MailAddress("jawahar.subramanian83@gmail.com"));
            message.Subject = subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = mailBody;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromEmail, pwd);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            return (message, smtp);


        }


        public static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static string GetDataFolder(string oldValue, string newValue)
        {
            string exeFile = (new Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);
            //string dataFolder = exeDir.Replace("AssemblyApp\\bin\\Debug", newValue);  

            string dataFolder = exeDir.Replace("NTK_Support\\bin\\Debug", newValue);

            return dataFolder;
        }

        public static void WriteToFile(string path, string content)
        {
            if (File.Exists(path))
                File.Delete(path);

            File.WriteAllText(path, content);

        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        public static void CreateFileIfNotExist(string path)
        {
            if (File.Exists(path) == false)
            {
                var myFile = File.Create(path);
                myFile.Close();
            }

        }

        public static void CreateFolderIfNotExist(string folderPath)
        {
            if (Directory.Exists(folderPath) == false)
            {
                var myFile = Directory.CreateDirectory(folderPath);

            }

        }

        public static void RecreateFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath);
            }

            Directory.CreateDirectory(folderPath);

        }



        public static void WriteLog(string path, string content, string assNo, string partNo, int pn)
        {
            CreateFileIfNotExist(path);
            File.AppendAllText(path, $"{assNo}-{partNo}-{pn} : {content}{Environment.NewLine}");
        }

        public static void ReplaceLog(string path, string content)
        {
            CreateFileIfNotExist(path);
            File.WriteAllText(path, $"{content}{Environment.NewLine}");
        }

    }
}
