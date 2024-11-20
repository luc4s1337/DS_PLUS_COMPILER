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
            FIM,

            LIT_INT,
            LIT_FLT,
            LIT_CHAR,
            LIT_STR,

            PR_BOOL,
            PR_CHAR,
            PR_DO, 
            PR_ELSE,
            PR_STR,
            PR_VOID,
            PR_VAR,
            PR_THEN,
            PR_FLT,
            PR_IF,
            PR_INT,
            PR_WHILE,

            PR_MAIN,
            PR_LOOP,
            PR_PRINT,
            PR_RTN,
            PR_SCN,     
            PR_END,

            OP_SOMA,
            OP_SUB,
            OP_MULT,
            OP_DIV,
            OP_MOD,
            OP_MAIOR,
            OP_MENOR,
            OP_ATRI,
            OP_IGUAL,
            OP_NEGA,
            OP_DIFERENTE,
            OP_MAIOR_IGUAL,
            OP_MENOR_IGUAL,

            VIRGULA,
            PONTO_VIRGULA,
            ABRE_COLCHETES, 
            FECHA_COLCHETES,
            ABRE_CHAVES,
            FECHA_CHAVES,
            ABRE_PARENTESES,
            FECHA_PARENTESES
        }
    }

    class Token
    {
        private Tokens TokenCodigo;
        private string Lexema;
        private int Linha;

        public Tokens GetCodigo()
        {
            return this.TokenCodigo;
        }

        public Token SetCodigo(Tokens codigo) 
        {
            this.TokenCodigo = codigo;
            return this;
        }        

        public Token SetLexema(string lexema)
        {
            this.Lexema = lexema;
            return this;
        }

        public string GetLexema()
        {
            return this.Lexema;
        }

        public Token SetLinha(int linha)
        {
            this.Linha = linha;
            return this;
        }

        public int GetLinha()
        {
            return this.Linha;
        }
    }
}
