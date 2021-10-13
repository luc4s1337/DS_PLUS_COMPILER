using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DS_PLUS_COMPILER.Src.Enums;

namespace DS_PLUS_COMPILER.Src
{
    public class Enums
    {
        public enum Tokens
        {
            ID,
            NUM_INT,
            NUM_REAL,
            CARACTERE,
            STRING,
            OP_SOMA,
            OP_SUB,
            OP_MULT,
            OP_DIV,
            OP_MAIOR,
            OP_MENOR,
            OP_IGUAL,
            OP_DIFERENTE,
            OP_MAIOR_IGUAL,
            OP_MENOR_IGUAL
        }
    }

    class Token
    {
        public Tokens TokenCodigo { get; set; }
        public string Lexema { get; set; }

        public Token(Tokens _tokenCodigo, string _lexema)
        {
            this.TokenCodigo = _tokenCodigo;
            this.Lexema = _lexema;
        }
    }
}
