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

            PR_VOID,
            PR_INT,
            PR_FLT,
            PR_CHAR,
            PR_BOOL,
            PR_IF,
            PR_THEN,
            PR_ELSE,
            PR_ENDIF,
            PR_FOR,
            PR_WHILE,
            PR_DO,
            PR_RETURN,
            PR_VAR,
            PR_SCAN,
            PR_PRINT,


            OP_SOMA, //
            OP_SUB, //
            OP_MULT, //
            OP_DIV, //
            OP_MOD, //
            OP_MAIOR, //
            OP_MENOR, //
            OP_ATRI, //
            OP_IGUAL, //
            OP_NEGA, //
            OP_DIFERENTE, //
            OP_MAIOR_IGUAL, //
            OP_MENOR_IGUAL, //

            VIRGULA, //
            PONTO_VIRGULA, //
            ABRE_COLCHETES, //
            FECHA_COLCHETES, //
            ABRE_CHAVES, //
            FECHA_CHAVES, //
            ABRE_PARENTESES, //
            FECHA_PARENTESES //
        }
    }

    class Token
    {
        public Tokens TokenCodigo { get; set; }
        public string Lexema { get; set; }

        public Token(string _lexema)
        {
            this.Lexema = _lexema;
        }
    }
}
