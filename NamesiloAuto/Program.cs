using System;
using System.IO;

namespace NamesiloAuto
{
    public class Program
    {
        private const string MainUrl = @"https://www.namesilo.com/";

        public static void Main(string[] args)
        {            
            Console.WriteLine("Please, enter your login name:");
            string user = Console.ReadLine();
            Console.WriteLine("Please, enter password:");
            string pw = Console.ReadLine();            
            var apiManger = new ApiManager(MainUrl);
            apiManger.Login("/login.php", user, pw);
            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    //init log writer:
                    FileStream ostrm = null;
                    StreamWriter writer = null;
                    TextWriter oldOut = Console.Out;
                    string oldLogs = "";
                    try
                    {                        
                        if(File.Exists("./Log.txt"))
                            oldLogs = File.ReadAllText("./Log.txt");
                        ostrm = new FileStream("./Log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                        writer = new StreamWriter(ostrm) {AutoFlush = true};
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Cannot open Log.txt for writing: " + e.Message);
                    }
                    if (writer != null) Console.SetOut(writer);
                    
                    //work with files:
                    FileProcessor fileProc = new FileProcessor(Environment.CurrentDirectory + "\\BIG_DATA");
                    foreach (var urlAndBid in fileProc.UrlsAndBids)
                    {
                        string status = apiManger.SetBid("/Auctions?auction=" + urlAndBid.Key, urlAndBid.Value) ? "DONE" : "FAIL";
                        Console.WriteLine("\r\n#################\r\n\r\n");
                        string fileName = urlAndBid.Key.Contains("Incorrect format:")
                            ? urlAndBid.Key.Substring(18)
                            : urlAndBid.Key + "___" + urlAndBid.Value + ".txt";
                        fileProc.MoveFile(fileName, Environment.CurrentDirectory + "\\BIG_DATA", status);
                    }
                    Console.Write(oldLogs);
                    //close log writer:
                    Console.SetOut(oldOut);
                    if (writer != null) writer.Close();
                    if (ostrm != null) ostrm.Close();
                }
                
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            
        }

        
    }
}
