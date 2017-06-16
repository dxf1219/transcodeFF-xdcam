using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace transcodeFF
{
    static class WriteLogNew
    {
        public  static void Log(String logMessage, TextWriter w,string type)  //type: info error warn
        {

            string s = DateTime.Now.ToString("HH:mm:ss:fff") + "  " + type +" " +logMessage;
            w.WriteLine(s);
            if (logMessage == "软件关闭!")
            {

                w.WriteLine("                 ");

            }
            // Update the underlying file.
            w.Flush();
        }
        public static  void writeLog(string log,string path,string type)
        {
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string s = path + "\\" + filename;
            try
            {
                StreamWriter w = File.AppendText(s);
                Log(log, w,type);
                w.Close();
            }
            catch (Exception)
            { 
               //
            }
        
        }
    }
}
