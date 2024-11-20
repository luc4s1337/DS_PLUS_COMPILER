using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DS_PLUS_COMPILER;

namespace DS_PLUS_COMPILER.Utils
{
    class FileManager
    {
        private string Path;
        private string Buffer;

        public FileManager SetFilePath(string Path) 
        {
            this.Path = Path;
            return this;
        }

        public string GetFilePath()
        {
            return this.Path;
        }

        public string GetFileBuffer()
        {
            return this.Buffer;
        }

        public FileManager OpenFileStream() 
        {         
            using var fileStream = new FileStream(this.Path, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream, Encoding.UTF8);

            this.Buffer = streamReader.ReadToEnd();

            return this;
        }

        public static void PrintFile(string print, string outputFileName) 
        {
            using (StreamWriter sw = System.IO.File.CreateText(Config.OutputPath+ outputFileName))
            {
                sw.Write(print);
            }
        }
    }
}
