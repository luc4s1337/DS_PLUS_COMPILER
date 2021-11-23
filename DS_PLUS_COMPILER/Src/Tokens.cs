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
            ID, //done
            FIM, //done

            LIT_INT, //done
            LIT_FLT, //done
            LIT_CHAR, //done
            LIT_STR, //done

            PR_BOOL, //done
            PR_CHAR, //done
            PR_DO, //done
            PR_ELSE, //done
            PR_END, //done
            PR_FOR, //done
            PR_FLT, //done
            PR_IF, //done
            PR_INT, //done
            PR_MAIN, //done
            PR_LOOP, //done
            PR_PRINT, //done
            PR_RTN, //done
            PR_SCN, //done
            PR_STR, //done
            PR_THEN, //done
            PR_VOID, //done
            PR_WHILE, //done
            

            OP_SOMA, //done
            OP_SUB, //done 
            OP_MULT, //done
            OP_DIV, //done
            OP_MOD, //done
            OP_MAIOR, //done
            OP_MENOR, //done
            OP_ATRI, //done
            OP_IGUAL, //done
            OP_NEGA, //done
            OP_DIFERENTE, //done
            OP_MAIOR_IGUAL, //done
            OP_MENOR_IGUAL, //done

            VIRGULA, //done
            PONTO_VIRGULA, //done
            ABRE_COLCHETES, //done
            FECHA_COLCHETES,//done
            ABRE_CHAVES,//done
            FECHA_CHAVES, //done
            ABRE_PARENTESES, //done
            FECHA_PARENTESES //done
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
