using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DS_PLUS_COMPILER.Src.Enums;

namespace DS_PLUS_COMPILER.Src
{
    class LEXICA
    {
        public string Buffer { get; set; }
        public List<Token> Tokens { get; set; }

        public LEXICA(string _buffer)
        {
            this.Buffer = _buffer;
        }

        public void StartAnaliseLexica() 
        {
            //IMPLEMENTAR LOGICA LEXICA
        }
    }
}
