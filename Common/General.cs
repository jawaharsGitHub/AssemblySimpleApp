
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using System.Media;

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

        public static string GetDataFolder(string dataAccessPath)
        {
            string exeFile = (new Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
            return General.CombinePath(Path.GetDirectoryName(exeFile).Replace("\\bin\\Debug", ""), dataAccessPath);

            //string exeDir = Path.GetDirectoryName(exeFile);
            //string dataFolder = exeDir.Replace("AssemblyApp\\bin\\Debug", newValue);  
            //File.AppendAllText(path, $"exeFile: {exeFile}");
            //var projectName = new FileInfo(exeFile).Name.Split('.')[0];
            //File.AppendAllText(path, $"projectName: {projectName}");

            //string dataFolder = exeDir.Replace($"{projectName}\\bin\\Debug", dataAccessPath);


            //File.AppendAllText(path, $"dataFolder: {dataFolder}");
            //return dataFolder;
        }

        public static string ShowPrompt(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
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

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2).Replace("%20", " ");
        }

        public static string SelectFolderPath()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (DialogResult.OK == fbd.ShowDialog())
                return fbd.SelectedPath;
            else
                return null;

        }

        public static List<string> SelectFilesInDialog(string defaultPath = null, string pattern = null)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.InitialDirectory = defaultPath;
            fbd.Filter = $"json files (*.*)|*{pattern}*"; // pattern;

            if (DialogResult.OK == fbd.ShowDialog())
                return fbd.FileNames.ToList();
            else
                return null;

        }

        public static List<string> SelectSingleFileDialog(string defaultPath = null, string pattern = null)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.InitialDirectory = defaultPath;
            //fbd.Filter = $"json files (*.*)|*{pattern}*"; // pattern;
            fbd.Multiselect = false;
            if (DialogResult.OK == fbd.ShowDialog())
                return fbd.FileNames.ToList();
            else
                return null;

        }

        public static void Play(string file)
        {
            SoundPlayer player = new SoundPlayer();
            player.Stop();
            player.SoundLocation = file;
            player.Play();
        }
    }
}
