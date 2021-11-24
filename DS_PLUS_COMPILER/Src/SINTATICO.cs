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
                Validado("programa", Tokens[TokensIndex].TokenCodigo);
                Decl();
            }
            else 
            {
                Erro("var ou main", "programa", Tokens[TokensIndex].Lexema);
            }
        }

        private void Decl()
        {
            switch (Tokens[TokensIndex].TokenCodigo)
            {
                case Enums.Tokens.PR_VAR:
                    TokensIndex++;
                    Validado("decl", Tokens[TokensIndex].TokenCodigo);
                    DeclVar();
                    DeclMain();
                    break;
                case Enums.Tokens.PR_MAIN:
                    Validado("decl", Tokens[TokensIndex].TokenCodigo);
                    TokensIndex++;
                    DeclMain();
                    break;
                default:
                    Erro("var ou main", "decl", Tokens[TokensIndex].Lexema);
                    break;
            }
        }
        private void DeclMain()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES)
            {
                TokensIndex++;
                Validado("decl-main", Enums.Tokens.ABRE_PARENTESES);
                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                {
                    TokensIndex++;
                    Validado("decl-main", Enums.Tokens.FECHA_PARENTESES);
                    Bloco();
                }
                else
                {
                    Erro(")", "decl-main", Tokens[TokensIndex].Lexema);
                }
            }
            else
            {
                Erro("(", "decl-main", Tokens[TokensIndex].Lexema);
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
                Erro("int, float, string, char","espec-tipo", Tokens[TokensIndex].Lexema);
            }
        }

        private void Var()
        {
            if (Tokens[TokensIndex-1].TokenCodigo == Enums.Tokens.PR_MAIN)
            {
                Validado("decl-main", Tokens[TokensIndex].TokenCodigo);
                TokensIndex++;
            }
            else 
            {
                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID)
                {
                    Validado("decl-var", Tokens[TokensIndex].TokenCodigo);                    
                    TokensIndex++;
                }
                else
                {
                    Erro("ID", "decl-var", Tokens[TokensIndex].Lexema);
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
                    Validado("decl-var", Tokens[TokensIndex].TokenCodigo);
                    DeclVar();
                    break;
                case Enums.Tokens.PR_IF:
                    TokensIndex++;
                    Validado("com-sel", Tokens[TokensIndex].TokenCodigo);
                    ComSelec();
                    break;
                case Enums.Tokens.PR_WHILE:
                    TokensIndex++;
                    Validado("com-rep", Tokens[TokensIndex].TokenCodigo);
                    ComRepeticao();
                    break;
                case Enums.Tokens.PR_SCN:
                    TokensIndex++;
                    Validado("com-lei", Tokens[TokensIndex].TokenCodigo);
                    ComLeitura();
                    break;
                case Enums.Tokens.PR_PRINT:
                    TokensIndex++;
                    Validado("com-esc", Tokens[TokensIndex].TokenCodigo);
                    ComEscrita();
                    break;
                case Enums.Tokens.ID:
                    TokensIndex++;
                    Validado("com-atr", Tokens[TokensIndex].TokenCodigo);
                    ComAtribui();
                    break;
                default:
                    Erro("um dos seguintes comandos var, if, while, scan, print, =", "comando", Tokens[TokensIndex].Lexema);
                    break;
            }
        }
        private void ComSelec()
        {
            Exp();

            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_THEN)
            {
                Validado("com-sel", Enums.Tokens.PR_THEN);
                TokensIndex++;
                Bloco();

                switch (Tokens[TokensIndex].TokenCodigo) 
                {
                    case Enums.Tokens.PR_ELSE:
                        TokensIndex++;
                        Validado("com-sel", Tokens[TokensIndex].TokenCodigo);
                        Bloco();

                        if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_END)
                        {
                            Validado("com-sel", Tokens[TokensIndex].TokenCodigo);
                            TokensIndex++;

                            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                            {
                                Validado("com-sel", Tokens[TokensIndex].TokenCodigo);
                                TokensIndex++;
                            }
                            else 
                            {
                                Erro(";", "com-sel", Tokens[TokensIndex].Lexema);
                            }
                        }
                        else 
                        {
                            Erro("end", "com-sel", Tokens[TokensIndex].Lexema);
                        }
                        break;
                    case Enums.Tokens.PR_END:
                        Validado("com-sel", Tokens[TokensIndex].TokenCodigo);
                        TokensIndex++;
                        break;
                    default:
                        Erro("else ou end", "com-sel", Tokens[TokensIndex].Lexema);
                        break;
                }
            }
            else
            {
                Erro("then", "com-sel", Tokens[TokensIndex].Lexema);
            }
        }

        private void ComRepeticao()
        {
            Exp();

            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_DO)
            {
                TokensIndex++;
                Validado("com-rep", Tokens[TokensIndex].TokenCodigo);
                Bloco();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_LOOP)
                {
                    Validado("com-rep", Tokens[TokensIndex].TokenCodigo);

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                    {
                        TokensIndex++;

                        Validado("com-rep", Tokens[TokensIndex].TokenCodigo);
                    }
                    else
                    {
                        Erro(";", "com-rep", Tokens[TokensIndex].Lexema);
                    }
                }
                else
                {
                    Erro("loop", "com-rep", Tokens[TokensIndex].Lexema);
                }
            }
            else
            {
                Erro("do", "com-rep", Tokens[TokensIndex].Lexema);
            }
        }

        private void ComLeitura()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES)
            {
                Validado("com-lei", Tokens[TokensIndex].TokenCodigo);
                Var();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                {
                    TokensIndex++;
                    Validado("com-lei", Tokens[TokensIndex].TokenCodigo);

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                    {
                        Validado("com-lei", Tokens[TokensIndex].TokenCodigo);
                        TokensIndex++;
                    }
                    else
                    {
                        Erro(";", "com-lei", Tokens[TokensIndex].Lexema);
                    }
                }
                else
                {
                    Erro(")", "com-lei", Tokens[TokensIndex].Lexema);
                }
            }
            else
            {
                Erro("(", "com-lei", Tokens[TokensIndex].Lexema);
            }
        }

        private void ComEscrita()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES)
            {
                Validado("com-esc", Tokens[TokensIndex].TokenCodigo);
                Exp();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                {
                    TokensIndex++;
                    Validado("com-esc", Tokens[TokensIndex].TokenCodigo);

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                    {
                        Validado("com-esc", Tokens[TokensIndex].TokenCodigo);
                        TokensIndex++;
                    }
                    else
                    {
                        Erro(";", "com-esc", Tokens[TokensIndex].Lexema);
                    }
                }
                else
                {
                    Erro(")", "com-esc", Tokens[TokensIndex].Lexema);
                }
            }
            else
            {
                Erro("(", "com-esc", Tokens[TokensIndex].Lexema);
            }
        }

        private void ComAtribui()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_ATRI)
            {
                TokensIndex++;
                Validado("com-atr", Tokens[TokensIndex].TokenCodigo);

                Exp();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                {
                    Validado("com-atr", Tokens[TokensIndex].TokenCodigo);
                    TokensIndex++;
                }
                else 
                {
                    Erro(";", "com-atr", Tokens[TokensIndex].Lexema);
                }
            }
            else 
            {
                Erro("=", "com-atr", Tokens[TokensIndex].Lexema);
            }
        }
                               
        private void Exp()
        {
        }
        #endregion

        #region prints
        private void Validado(string bloco, Enums.Tokens token)
        {
            string str = string.Format("Bloco {0} token {1} OK.\n\n", bloco, token);

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
