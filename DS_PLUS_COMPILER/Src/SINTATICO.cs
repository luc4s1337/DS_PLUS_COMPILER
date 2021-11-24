using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class SINTATICO
    {
        #region inicializacao do analisador        
        public List<Token> Tokens { get; set; }
        public int TokensIndex { get; set; } = 0;
        public string Log { get; set; } = "";        

        public SINTATICO(List<Token> _tokens)
        {
            this.Tokens = _tokens;
        }

        public void StartAnaliseSintatica()
        {
            Programa();
        }
        #endregion

        #region analiseSintaticaSemantica
        public void Programa()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_VAR || Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_MAIN)
            {
                Decl();
            }
        }

        private void Decl()
        {
            switch (Tokens[TokensIndex].TokenCodigo)
            {
                case Enums.Tokens.PR_VAR:
                    TokensIndex++;
                    DeclVar();
                    DeclMain();
                    break;
                case Enums.Tokens.PR_MAIN:
                    TokensIndex++;
                    DeclMain();
                    break;
                default:
                    Erro("var ou main", "Main", Tokens[TokensIndex].Lexema);
                    break;
            }
        }
        private void DeclMain()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES)
            {
                TokensIndex++;
                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                {
                    TokensIndex++;
                    Validado("Main");
                    Bloco();
                }
                else
                {
                    Erro(")", "Main", Tokens[TokensIndex].Lexema);
                }
            }
            else
            {
                Erro("(", "Main", Tokens[TokensIndex].Lexema);
            }
        }

        private void DeclVar()
        {
            EspecTipo();
            Var();
        }

        private void EspecTipo()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_FLT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_CHAR)
            {
                TokensIndex++;
            }
            else 
            {
                Erro("int, float, string, char","Main", Tokens[TokensIndex].Lexema);
            }
        }

        private void Var()
        {
            if (Tokens[TokensIndex-1].TokenCodigo == Enums.Tokens.PR_MAIN)
            {
                TokensIndex++;
            }
            else 
            {
                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID)
                {
                    Validado("Decl_var");                    
                    TokensIndex++;
                }
                else
                {
                    Erro("ID", "VAR", Tokens[TokensIndex].Lexema);
                }               
            }
        }              

        private void Bloco()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_SCN ||
            Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_PRINT ||
            Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_IF ||
            Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_WHILE ||
            Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_VAR ||
            Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID ||
            Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_END) 
            {
                ListaCom();
            }                
        }

        private void ListaCom()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_SCN ||
              Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_PRINT ||
              Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_IF ||
              Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_WHILE ||
              Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_VAR ||
              Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID ||
              Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_END)
            {
                Comando();
                ListaCom();
            }          
        }

        private void Comando()
        {
            switch (Tokens[TokensIndex].TokenCodigo) 
            {
                case Enums.Tokens.PR_VAR:
                    TokensIndex++;
                    DeclVar();
                    break;
                case Enums.Tokens.PR_IF:
                    TokensIndex++;
                    ComSelec();
                    break;
                case Enums.Tokens.PR_WHILE:
                    TokensIndex++;
                    ComRepeticao();
                    break;
                case Enums.Tokens.PR_SCN:
                    TokensIndex++;
                    ComLeitura();
                    break;
                case Enums.Tokens.PR_PRINT:
                    TokensIndex++;
                    ComEscrita();
                    break;
                case Enums.Tokens.ID:
                    TokensIndex++;
                    ComAtribui();
                    break;
                default:
                    Erro("um dos seguintes comandos var, if, while, scan, print, =", "Comando", Tokens[TokensIndex].Lexema);
                    break;
            }
            //if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_SCN ||)
        }

        private void ComAtribui()
        {

        }
        private void ComEscrita()
        {
        }
        private void ComLeitura()
        {
        }
        private void ComRepeticao()
        {
        }
        private void ComSelec()
        {
        }
        #endregion

        #region prints
        private void Validado(string bloco)
        {
            string str = string.Format("Bloco {0} validado com sucesso.\n\n", bloco);

            Console.WriteLine(str);
            this.Log += str;
        }

        private void Erro(string esperado, string estrutura, string lexema)
        {
            Console.WriteLine(string.Format("Erro na estrutura '{1}', Esperado '{0}' encontrado o token '{2}'.", esperado, estrutura, lexema));
        }
        #endregion
    }
}
