using System;
using System.IO;
using System.Collections.Generic;

namespace NamesiloAuto
{
    public class FileProcessor
    {
        public Dictionary<string, string> UrlsAndBids;

        public FileProcessor(string folderPath)
        {
            if(UrlsAndBids == null)
                UrlsAndBids = new Dictionary<string, string>();
            var files = Directory.GetFiles(folderPath, "*.txt");
            int i = 1;
            foreach (var file in files)
            {                
                string fileName = file.Substring(file.LastIndexOf('\\') + 1);
                int ind = fileName.IndexOf("___", StringComparison.Ordinal);
                string url = (ind != -1) ? fileName.Substring(0, ind) : "Incorrect format: " + fileName;
                string bid = (ind != -1) ? fileName.Substring(ind + 3, fileName.IndexOf('.') - ind - 3) : "Incorrect format: " + fileName;
                if (UrlsAndBids.ContainsKey(url))
                    url = "Duplicate url: " + url + " #" + i;
                UrlsAndBids.Add(url, bid);
                i++;
            }
        }

        public void MoveFile(string fileName, string folderPath, string status = "DONE")
        {
            string sourceFile = Path.Combine(folderPath, fileName);
            string newFolderPath = folderPath + "/" + status;
            string destFile = Path.Combine(newFolderPath, fileName);
            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);
            }
            try
            {
                if (!File.Exists(destFile)) File.Move(sourceFile, destFile);
                else File.Delete(sourceFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot move file from '{0}' to '{1}': {2}.", sourceFile, destFile, ex.Message);
            }
        }
    }
}
