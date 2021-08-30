using System;
using System.Collections.Generic;
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
        public byte[] Buffer { get; set; }

        public File(string _filePath, string _fileName, long _fileSize, byte[] _buffer) 
        {
            this.FilePath = _filePath;
            this.FileName = _fileName;
            this.FileSize = _fileSize;
            this.Buffer = _buffer;
        }

        public string GetStringBuffer() 
        {
            return System.IO.File.ReadAllText(this.FilePath);
        }
    }
}
