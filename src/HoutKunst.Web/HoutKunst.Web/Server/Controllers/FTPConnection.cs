using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HoutKunst.Web.Server.Controllers
{
    public class FTPConnection
    {
        private static readonly string Server = "ftp.h1-projecten3be.webhosting.be";
        private static readonly int Port = 21;
        private static readonly string User = "projectadmin@h1-projecten3be";
        private static readonly string Pass = "jo4LN2T68fd5GhZB5hp9";

        public static string Download(string url)
        {
            FtpClient client = new FtpClient(Server);
            client.Credentials = new NetworkCredential(User, Pass);
            client.Connect();
            client.DownloadFile(System.IO.Directory.GetCurrentDirectory() + "\\" + url, url);
            client.Dispose();
            return System.IO.Directory.GetCurrentDirectory() + "\\" + url;
        }

        public static void UploadBijlages(Byte[] bytes, string naam)
        {
            FtpClient client = new FtpClient(Server);
            client.Credentials = new NetworkCredential(User, Pass);
            client.Connect();
            client.Upload(bytes, naam);
            client.Dispose();
        }
    }
}
