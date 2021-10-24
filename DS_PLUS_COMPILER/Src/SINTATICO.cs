using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class SINTATICO
    {
        public List<Token> Tokens { get; set; }
        public int TokensIndex { get; set; }

        public SINTATICO(List<Token> _tokens) 
        {
            this.Tokens = _tokens;
            this.TokensIndex = 1;
        }

        public void StartAnaliseSintatica() 
        {
        }

        public string PrintAnalise()
        {
            return "";
        }
    }
}
