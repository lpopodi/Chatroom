using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ChatLog : ILogger
    {
        public void Log(string message)
        {
           // string path = @"";

            //if (!File.Exists(path))
            //{
            //    string createText = "!!!! Beginning of Log !!!!\n" + Environment.NewLine;
            //    File.WriteAllText(path, createText);
            //}

            //File.AppendAllText(path, DateTime.Now.ToString() + "\n" + message);
        }
    }
}
