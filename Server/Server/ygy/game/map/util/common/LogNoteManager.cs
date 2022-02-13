using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ygy.game.map.util.common
{
    public class LogNoteManager
    {
        private static LogNoteManager instance;
        private string logPath;
        public static LogNoteManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LogNoteManager();
                }
                return instance;
            }
        }
        private LogNoteManager()
        {
            logPath = @"\log\log.txt";
        }

        public void Log(string value)
        {
            if(value == null)
            {
                return;
            }
            if(value == "")
            {
                return;
            }
            Console.WriteLine(value);
            return;
            lock(this)
            {
                try
                {
                    FileStream fs = null;
                    using (fs = File.OpenWrite(logPath))
                    {
                        if (File.Exists(logPath) == false)
                        {
                            fs = File.Create(logPath);
                        }
                        Console.WriteLine(value);
                        fs.Write(Encoding.Default.GetBytes(value), 0, Encoding.Default.GetBytes(value).Length);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
