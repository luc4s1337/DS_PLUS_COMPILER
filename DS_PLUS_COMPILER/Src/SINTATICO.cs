using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class SINTATICO
    {
        #region INICIALIZA       
        public List<Token> Tokens { get; set; }
        public int TokensIndex { get; set; } = 0;
        public string Log { get; set; } = "";        

        public SINTATICO(List<Token> _tokens)
        {
            this.Tokens = _tokens;
        }

        public void StartAnaliseSintatica()
        {
            Console.WriteLine("-----------(INICIO)-PRINT-SINTATICO------------------\n\n");

            Programa();

            Console.WriteLine("\n\n-----------(FIM)----PRINT-SINTATICO------------------\n\n");
        }
        #endregion
                
        #region DECLARACAO
        public void Programa()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_VAR || Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_MAIN)
            {
                Validado("programa", Tokens[TokensIndex].Lexema);
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
                    Validado("decl", Tokens[TokensIndex].Lexema);
                    TokensIndex++;
                    DeclVar();
                    DeclMain();
                    break;
                case Enums.Tokens.PR_MAIN:                    
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
                Validado("decl-main", Tokens[TokensIndex].Lexema);
                TokensIndex++;                
                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                {
                    Validado("decl-main", Tokens[TokensIndex].Lexema);
                    TokensIndex++;                    
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

            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_ATRI)
            {
                ComAtribui();
            }
            else 
            {
                switch (Tokens[TokensIndex].TokenCodigo) 
                {
                    case Enums.Tokens.PONTO_VIRGULA:
                        Validado("decl-var", Tokens[TokensIndex].Lexema);
                        TokensIndex++;
                        break;
                    case Enums.Tokens.VIRGULA:
                        Validado("decl-var", Tokens[TokensIndex].Lexema);
                        TokensIndex++;
                        break;
                    default:
                        Erro("; ou ,", "decl-var", Tokens[TokensIndex].Lexema);
                        break;
                }
            }
        }

        private void EspecTipo()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_FLT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_CHAR)
            {
                Validado("espec-tipo", Tokens[TokensIndex].Lexema);
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
                Validado("decl-main", Tokens[TokensIndex].Lexema);
                TokensIndex++;
            }
            else 
            {
                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID)
                {
                    Validado("decl-var", Tokens[TokensIndex].Lexema);                    
                    TokensIndex++;
                }
                else
                {
                    Erro("ID", "decl-var", Tokens[TokensIndex].Lexema);
                }               
            }
        }

        private void Literal() 
        {
            if (
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_CHAR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_FLT
                )
            {
                Validado("literal", Tokens[TokensIndex].Lexema);
                TokensIndex++;
            }
            else 
            {
                Erro("literal (int, float, string, char)","literal", Tokens[TokensIndex].Lexema);
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
        #endregion

        #region COMANDOS
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
                    Validado("decl-var", Tokens[TokensIndex].Lexema);
                    TokensIndex++;                    
                    DeclVar();
                    break;
                case Enums.Tokens.PR_IF:
                    Validado("com-sel", Tokens[TokensIndex].Lexema);
                    TokensIndex++;                    
                    ComSelec();
                    break;
                case Enums.Tokens.PR_WHILE:                    
                    Validado("com-rep", Tokens[TokensIndex].Lexema);
                    TokensIndex++;
                    ComRepeticao();
                    break;
                case Enums.Tokens.PR_SCN:                    
                    Validado("com-lei", Tokens[TokensIndex].Lexema);
                    TokensIndex++;
                    ComLeitura();
                    break;
                case Enums.Tokens.PR_PRINT:                    
                    Validado("com-esc", Tokens[TokensIndex].Lexema);
                    TokensIndex++;
                    ComEscrita();
                    break;
                case Enums.Tokens.ID:                    
                    Validado("com-atr", Tokens[TokensIndex].Lexema);
                    TokensIndex++;
                    ComAtribui();
                    break;
                case Enums.Tokens.FIM:
                    Validado("FIM", Tokens[TokensIndex].TokenCodigo.ToString());
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
                Validado("com-sel", Tokens[TokensIndex].Lexema);
                TokensIndex++;
                Bloco();

                switch (Tokens[TokensIndex].TokenCodigo) 
                {
                    case Enums.Tokens.PR_ELSE:
                        Validado("com-sel", Tokens[TokensIndex].Lexema);
                        TokensIndex++;
                        
                        Bloco();

                        if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_END)
                        {
                            Validado("com-sel", Tokens[TokensIndex].Lexema);
                            TokensIndex++;

                            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                            {
                                Validado("com-sel", Tokens[TokensIndex].Lexema);
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
                        Validado("com-sel", Tokens[TokensIndex].Lexema);
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
                Validado("com-rep", Tokens[TokensIndex].Lexema);
                TokensIndex++;                
                Bloco();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_LOOP)
                {
                    Validado("com-rep", Tokens[TokensIndex].Lexema);

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                    {
                        Validado("com-rep", Tokens[TokensIndex].Lexema);
                        TokensIndex++;                        
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
                Validado("com-lei", Tokens[TokensIndex].Lexema);
                Var();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                {
                    Validado("com-lei", Tokens[TokensIndex].Lexema);
                    TokensIndex++;                    

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                    {
                        Validado("com-lei", Tokens[TokensIndex].Lexema);
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
                Validado("com-esc", Tokens[TokensIndex].Lexema);
                Exp();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                {
                    Validado("com-esc", Tokens[TokensIndex].Lexema);
                    TokensIndex++;                    

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                    {
                        Validado("com-esc", Tokens[TokensIndex].Lexema);
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
                Validado("com-atr", Tokens[TokensIndex].Lexema);
                TokensIndex++;

                Exp();

                switch (Tokens[TokensIndex].TokenCodigo)
                {
                    case Enums.Tokens.PONTO_VIRGULA:
                        Validado("decl-var", Tokens[TokensIndex].Lexema);
                        TokensIndex++;
                        break;
                    case Enums.Tokens.VIRGULA:
                        Validado("decl-var", Tokens[TokensIndex].Lexema);
                        TokensIndex++;
                        break;
                    default:
                        Erro("; ou ,", "decl-var", Tokens[TokensIndex].Lexema);
                        break;
                }
            }
            else 
            {
                Erro("=", "com-atr", Tokens[TokensIndex].Lexema);
            }
        }
        #endregion

        #region EXPRESSOES
        private void Exp()
        {
            if (
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_FLT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_CHAR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID                
                )
            {
                ExpSoma();
                Exp1();
            }
        }

        private void Exp1()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MENOR_IGUAL ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MENOR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MAIOR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MAIOR_IGUAL ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_DIFERENTE ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_IGUAL) 
            {
                OpRelac();
            }

            if (
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_FLT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_CHAR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID
                )
            {
                ExpSoma();
            }
        }
                
        private void ExpSimples() 
        {
            switch (Tokens[TokensIndex].TokenCodigo) 
            {
                case Enums.Tokens.ABRE_PARENTESES:
                    Validado("exp-simples", Tokens[TokensIndex].Lexema);
                    TokensIndex++;
                    Exp();

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                    {
                        Validado("exp-simples", Tokens[TokensIndex].Lexema);
                        TokensIndex++;
                    }
                    else 
                    {
                        Erro(")","exp-simples", Tokens[TokensIndex].Lexema);
                    }
                    break;
                case Enums.Tokens.ID:
                    Var();
                    break;
                case Enums.Tokens.LIT_INT:
                case Enums.Tokens.LIT_FLT:
                case Enums.Tokens.LIT_CHAR:
                case Enums.Tokens.LIT_STR:
                    Literal();
                    break;
                default:
                    Erro("esperado um dos seguintes tokens: (, ID, int, float, char ou string","exp-simples", Tokens[TokensIndex].Lexema);
                    break;
            }
        }

        private void ExpSoma()
        {
            if (
               Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES ||
               Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_FLT ||
               Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_INT ||
               Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_CHAR ||
               Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_STR ||
               Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID
               )
            {
                ExpMult();
                ExpSoma1();
            }            
        }

        private void ExpSoma1()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_SOMA ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_SUB)
            {
                OpSoma();
            }

            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_FLT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_CHAR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID
                )
            {               
                ExpSoma();
            }
        }
                
        private void ExpMult()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_FLT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_CHAR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID)
            {
                ExpSimples();
                ExpMult1();
            }
        }

        private void ExpMult1()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MULT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_DIV ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MOD)            
            {
                OpMult();                
            }

            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_FLT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_INT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_CHAR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.LIT_STR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID)
            {               
                ExpMult();
            }
        }

        private void OpSoma()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_SUB ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_SOMA)
            {
                Validado("op-soma", Tokens[TokensIndex].Lexema);
                TokensIndex++;
            }
            else
            {
                Erro("+ ou -", "op-soma", Tokens[TokensIndex].Lexema);
            }
        }

        private void OpRelac()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MENOR_IGUAL ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MENOR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MAIOR ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MAIOR_IGUAL ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_DIFERENTE ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_IGUAL)
            {
                Validado("op-relac", Tokens[TokensIndex].Lexema);
                TokensIndex++;
            }
            else
            {
                Erro("um dos seguintes operadores: <=, <, >, >=, ==, != ", "op-relac", Tokens[TokensIndex].Lexema);
            }
        }

        private void OpMult() 
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MULT ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_DIV ||
                Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_MOD)
            {
                Validado("op-mult", Tokens[TokensIndex].Lexema);
                TokensIndex++;
            }
            else
            {
                Erro("esperado um dos seguintes operadores: *, / ou %", "op-soma", Tokens[TokensIndex].Lexema);
            }
        }               
        #endregion

        #region PRINTS
        private void Validado(string bloco, string lexema)
        {
            string str = string.Format("Bloco {0} token {1} OK.\n", bloco, lexema);

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
