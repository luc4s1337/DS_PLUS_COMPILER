using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS_PLUS_COMPILER;

namespace DS_PLUS_COMPILER.Utils
{
    class File
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string Buffer { get; set; }

        public File(string _filePath) 
        {
            this.FilePath = _filePath;

            OpenFileStream();
        }

        private void OpenFileStream() 
        {         
            using var fileStream = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream, Encoding.UTF8);

            this.Buffer = streamReader.ReadToEnd();
        }

        //TODO
        public void PrintFile(string print) 
        {

        }
    }
}
