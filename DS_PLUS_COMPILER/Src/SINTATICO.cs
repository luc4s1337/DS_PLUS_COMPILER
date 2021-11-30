using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DS_PLUS_COMPILER.Src;
using DS_PLUS_COMPILER.Utils;

namespace DS_PLUS_COMPILER.Src
{
    class SINTATICO
    {
        #region INICIALIZA       
        public List<Token> Tokens { get; set; }
        public int TokensIndex { get; set; } = 0;
        public string Log { get; set; } = "";
        public string EstruturaAtual { get; set; } = "Main";
        public SEMANTICO Semantico { get; set; } = new SEMANTICO();

        public SINTATICO(List<Token> _tokens)
        {
            this.Tokens = _tokens;
        }

        public void StartAnaliseSintatica()
        {
            string inicio = "-----------(INICIO)-PRINT-SINTATICO------------------\n\n";
            Log += inicio;
            Console.Write(inicio);

            string cabecalho = "";
            cabecalho += "              BLOCO";
            cabecalho += "                    LEXEMA";
            cabecalho += "                           SITUAÇÃO\n\n";

            Log += cabecalho;
            Console.Write(cabecalho);

            Programa();

            string valido = "\nPROGRAMA VÁLIDO SINTATICAMENTE!\n";
            Log += valido;

            Console.Write(valido);

            string fim = "\n\n";
            Log += fim;

            Console.Write(fim);

            Semantico.PrintLogTabela();
            Semantico.LogSemantico += "\nPROGRAMA VÁLIDO SEMANTICAMENTE!\n";
            Semantico.PrintLogSemantico();            
        }
        #endregion
                
        #region DECLARACAO
        public void Programa()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_VAR || Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_MAIN)
            {
                Validado("programa", Tokens[TokensIndex].Lexema);
                Decl();
                RemoveSimbolosMain();            
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
            Var(true);

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

        private void Var(bool isDecl)
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

                    if (!isDecl)
                    {
                        var simbolo = Semantico.BuscarSimbolo(Tokens[TokensIndex].Lexema);

                        if (simbolo == null)
                        {
                            Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex].Lexema, "declarada", "ERRO");
                            Semantico.ErroSemantico(string.Format("Váriavel {0} não declarada.", Tokens[TokensIndex].Lexema), Tokens[TokensIndex].Linha);
                        }
                        else
                        {
                            Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex].Lexema, "busca na tabela", "OK");

                            if (!simbolo.Inicializada)
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex].Lexema, "inicializada", "ERRO");
                                Semantico.ErroSemantico(string.Format("Váriavel {0} não inicializada.", Tokens[TokensIndex].Lexema), Tokens[TokensIndex].Linha);
                            }
                            else
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex].Lexema, "inicializada", "OK");
                            }
                        }
                    }
                    else
                    {
                        var simboloToInsert = Semantico.BuscarSimbolo(Tokens[TokensIndex].Lexema);

                        if (simboloToInsert == null)
                        {
                            string status = "Inserindo no " + this.Semantico.EscopoAtual;

                            Semantico.InserirSimbolo(Tokens[TokensIndex-1].Lexema, Tokens[TokensIndex].Lexema, status);
                            Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex].Lexema, "declarada", "OK");
                        }
                        else 
                        {
                            Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex].Lexema, "declarada", "ERRO");
                            Semantico.ErroSemantico(string.Format("Váriavel {0} já declarada.", Tokens[TokensIndex].Lexema), Tokens[TokensIndex].Linha);
                        }                       

                        //SEMANTICO -> valida se a Id que sera atribuida eh do mesmo tipo da variavel
                        if (Tokens[TokensIndex+2].TokenCodigo == Enums.Tokens.ID) 
                        {
                            var simbolo = Semantico.BuscarSimbolo(Tokens[TokensIndex+2].Lexema);

                            if (simbolo == null)
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex + 2].Lexema, "declarada", "ERRO");
                                Semantico.ErroSemantico(string.Format("Váriavel {0} não declarada.", Tokens[TokensIndex + 2].Lexema), Tokens[TokensIndex + 2].Linha);
                            }
                            else 
                            {
                                if (!simbolo.Inicializada)
                                {
                                    Semantico.ErroSemantico(string.Format("Váriavel {0} não inicializada.", Tokens[TokensIndex+2].Lexema), Tokens[TokensIndex+2].Linha);
                                }

                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex + 2].Lexema, "busca na tabela", "OK");

                                if (simbolo.Tipo != Tokens[TokensIndex-1].TokenCodigo) 
                                {
                                    Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex + 2].Lexema, "atribuicao", "ERRO");
                                    Semantico.ErroSemantico(string.Format("Tipo diferente da variavel declarada {0}.", Tokens[TokensIndex].Lexema), Tokens[TokensIndex].Linha);
                                }
                                else
                                {
                                    Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex + 2].Lexema, "Tipos", "OK");
                                }
                            }
                        }                                                          
                    }

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

                var simboloToVerify = Semantico.BuscarSimbolo(Tokens[TokensIndex-2].Lexema);

                if (simboloToVerify != null) 
                {
                    //SEMANTICO -> verifica se o literal eh do mesmo tipo da variavel
                    switch (simboloToVerify.Tipo)
                    {
                        case Enums.Tokens.PR_CHAR:
                            if (Tokens[TokensIndex].TokenCodigo != Enums.Tokens.LIT_CHAR)
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "ERRO");
                                Semantico.ErroSemantico(string.Format("Tipo diferente da variavel declarada {0}.", Tokens[TokensIndex-2].Lexema), Tokens[TokensIndex-2].Linha);
                            }
                            else
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "OK");
                            }

                            break;
                        case Enums.Tokens.PR_FLT:
                            if (Tokens[TokensIndex].TokenCodigo != Enums.Tokens.LIT_FLT)
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "ERRO");
                                Semantico.ErroSemantico(string.Format("Tipo diferente da variavel declarada {0}.", Tokens[TokensIndex-2].Lexema), Tokens[TokensIndex-2].Linha);
                            }
                            else
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "OK");
                            }

                            break;
                        case Enums.Tokens.PR_INT:
                            if (Tokens[TokensIndex].TokenCodigo != Enums.Tokens.LIT_INT)
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "ERRO");
                                Semantico.ErroSemantico(string.Format("Tipo diferente da variavel declarada {0}.", Tokens[TokensIndex-2].Lexema), Tokens[TokensIndex-2].Linha);
                            }
                            else
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "OK");
                            }

                            break;
                        case Enums.Tokens.PR_STR:
                            if (Tokens[TokensIndex].TokenCodigo != Enums.Tokens.LIT_STR)
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "ERRO");
                                Semantico.ErroSemantico(string.Format("Tipo diferente da variavel declarada {0}.", Tokens[TokensIndex-2].Lexema), Tokens[TokensIndex-2].Linha);
                            }
                            else
                            {
                                Semantico.GravarLogSemantico(EstruturaAtual, Tokens[TokensIndex-2].Lexema, "atribuicao", "OK");
                            }

                            break;
                    }
                }               

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
            Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID)
            {
                ListaCom();
            }

            if (Tokens[TokensIndex].TokenCodigo != Enums.Tokens.FIM &&
                 Tokens[TokensIndex].TokenCodigo != Enums.Tokens.PR_END &&
                 Tokens[TokensIndex].TokenCodigo != Enums.Tokens.PR_ELSE &&
                 Tokens[TokensIndex].TokenCodigo != Enums.Tokens.PR_LOOP) 
            {
                Erro("um dos seguintes comandos var, if, while, scan, print, Id", "bloco", Tokens[TokensIndex].Lexema);
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
              Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ID)
            {
                Comando();
                ListaCom();
            }
        }

        private void Comando()
        {
            this.EstruturaAtual = "Main";
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
            this.EstruturaAtual = "ComSelec";
            Exp();

            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_THEN)
            {
                Validado("com-sel", Tokens[TokensIndex].Lexema);
                TokensIndex++;
                Semantico.EscopoAtual = "Local";
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
                                Semantico.EscopoAtual = "Global";
                                RemoveSimbolosWHILE();
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

                        if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                        {
                            Validado("com-sel", Tokens[TokensIndex].Lexema);
                            TokensIndex++;
                            Semantico.EscopoAtual = "Global";
                        }
                        else
                        {
                            Erro(";", "com-sel", Tokens[TokensIndex].Lexema);
                        }
                        
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
            this.EstruturaAtual = "ComRepeticao";
            Exp();

            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_DO)
            {
                Validado("com-rep", Tokens[TokensIndex].Lexema);
                TokensIndex++;
                Semantico.EscopoAtual = "Local";
                Bloco();

                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_LOOP)
                {
                    Validado("com-rep", Tokens[TokensIndex].Lexema);
                    TokensIndex++;

                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PONTO_VIRGULA)
                    {
                        Validado("com-rep", Tokens[TokensIndex].Lexema);
                        Semantico.EscopoAtual = "Global";
                        RemoveSimbolosWHILE();
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
            this.EstruturaAtual = "ComLeitura";
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES)
            {
                Validado("com-lei", Tokens[TokensIndex].Lexema);
                TokensIndex++;
                Var(false);

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
                Erro("(", "com-lei", Tokens[TokensIndex].Lexema);
            }
        }

        private void ComEscrita()
        {
            this.EstruturaAtual = "ComEscrita";
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES)
            {
                Validado("com-esc", Tokens[TokensIndex].Lexema);
                Exp();     

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
                Erro("(", "com-esc", Tokens[TokensIndex].Lexema);
            }
        }

        private void ComAtribui()
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.OP_ATRI)
            {
                Validado("com-atr", Tokens[TokensIndex].Lexema);
                TokensIndex++;

                Simbolo simbolo = Semantico.BuscarSimbolo(Tokens[TokensIndex - 2].Lexema);

                if (simbolo != null && !simbolo.Inicializada)
                {
                    Semantico.AtualizarSimbolo(simbolo.ID, true);
                }                

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
                    Var(false);
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

                //SEMANTICO -> valida as operacoes + & -
                Enums.Tokens simboloEsquerdo, simboloDireito;

                if (Tokens[TokensIndex - 1].TokenCodigo == Enums.Tokens.ID)
                    simboloEsquerdo = Semantico.TipoCast(Semantico.BuscarSimbolo(Tokens[TokensIndex - 1].Lexema).Tipo).Value;
                else
                    simboloEsquerdo = Tokens[TokensIndex-1].TokenCodigo;

                if (Tokens[TokensIndex+1].TokenCodigo == Enums.Tokens.ID)
                    simboloDireito = Semantico.TipoCast(Semantico.BuscarSimbolo(Tokens[TokensIndex+1].Lexema).Tipo).Value;
                else
                    simboloDireito = Tokens[TokensIndex+1].TokenCodigo;

                string lexemaLog = string.Format("{0} {2} {1}", simboloEsquerdo, simboloDireito, Tokens[TokensIndex].Lexema);

                if (simboloEsquerdo != simboloDireito) 
                {                  
                    Semantico.GravarLogSemantico(EstruturaAtual, lexemaLog, "OpSoma", "ERRO");
                    Semantico.ErroSemantico(string.Format("Operação {0} inválida semânticamente.", lexemaLog), Tokens[TokensIndex].Linha);
                }
                else
                {
                    Semantico.GravarLogSemantico(EstruturaAtual, lexemaLog, "OpSoma", "OK");
                }

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

                //SEMANTICO -> valida as operacoes <=, <, >, >=, !=, ==
                Enums.Tokens simboloEsquerdo, simboloDireito;

                if (Tokens[TokensIndex - 1].TokenCodigo == Enums.Tokens.ID)
                    simboloEsquerdo = Semantico.TipoCast(Semantico.BuscarSimbolo(Tokens[TokensIndex - 1].Lexema).Tipo).Value;
                else
                    simboloEsquerdo = Tokens[TokensIndex - 1].TokenCodigo;

                if (Tokens[TokensIndex + 1].TokenCodigo == Enums.Tokens.ID)
                    simboloDireito = Semantico.TipoCast(Semantico.BuscarSimbolo(Tokens[TokensIndex + 1].Lexema).Tipo).Value;
                else
                    simboloDireito = Tokens[TokensIndex + 1].TokenCodigo;

                string lexemaLog = string.Format("{0} {2} {1}", simboloEsquerdo, simboloDireito, Tokens[TokensIndex].Lexema);

                if (Tokens[TokensIndex].TokenCodigo != Enums.Tokens.OP_DIFERENTE &&
                    Tokens[TokensIndex].TokenCodigo != Enums.Tokens.OP_IGUAL) 
                {
                    if (simboloEsquerdo == Enums.Tokens.LIT_CHAR || simboloEsquerdo == Enums.Tokens.LIT_STR ||
                        simboloDireito == Enums.Tokens.LIT_CHAR  || simboloDireito == Enums.Tokens.LIT_STR)
                    {
                        Semantico.GravarLogSemantico(EstruturaAtual, lexemaLog, "OpRelac", "ERRO");
                        Semantico.ErroSemantico(string.Format("Operação {0} inválida semânticamente.", lexemaLog), Tokens[TokensIndex].Linha);
                    }
                }

                if (simboloEsquerdo != simboloDireito)
                {
                    Semantico.GravarLogSemantico(EstruturaAtual, lexemaLog, "OpRelac", "ERRO");
                    Semantico.ErroSemantico(string.Format("Operação {0} inválida semânticamente.", lexemaLog), Tokens[TokensIndex].Linha);
                }
                else
                {
                    Semantico.GravarLogSemantico(EstruturaAtual, lexemaLog, "OpRelac", "OK");
                }

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

                //SEMANTICO -> valida as operacoes *, /, %
                Enums.Tokens simboloEsquerdo, simboloDireito;

                if (Tokens[TokensIndex - 1].TokenCodigo == Enums.Tokens.ID)
                    simboloEsquerdo = Semantico.TipoCast(Semantico.BuscarSimbolo(Tokens[TokensIndex - 1].Lexema).Tipo).Value;
                else
                    simboloEsquerdo = Tokens[TokensIndex - 1].TokenCodigo;

                if (Tokens[TokensIndex + 1].TokenCodigo == Enums.Tokens.ID)
                    simboloDireito = Semantico.TipoCast(Semantico.BuscarSimbolo(Tokens[TokensIndex + 1].Lexema).Tipo).Value;
                else
                    simboloDireito = Tokens[TokensIndex + 1].TokenCodigo;

                string lexemaLog = string.Format("{0} {2} {1}", simboloEsquerdo, simboloDireito, Tokens[TokensIndex].Lexema);

                if (simboloEsquerdo != simboloDireito ||
                    simboloEsquerdo != Enums.Tokens.LIT_FLT &&
                    simboloEsquerdo != Enums.Tokens.LIT_INT &&
                    simboloDireito != Enums.Tokens.LIT_FLT &&
                    simboloDireito != Enums.Tokens.LIT_INT)
                {
                    Semantico.GravarLogSemantico(EstruturaAtual, lexemaLog, "OpRelac", "ERRO");
                    Semantico.ErroSemantico(string.Format("Operação {0} inválida semânticamente.", lexemaLog), Tokens[TokensIndex].Linha);
                }
                else
                {
                    Semantico.GravarLogSemantico(EstruturaAtual, lexemaLog, "OpRelac", "OK");
                }

                TokensIndex++;
            }
            else
            {
                Erro("esperado um dos seguintes operadores: *, / ou %", "op-soma", Tokens[TokensIndex].Lexema);
            }
        }
        #endregion

        private void RemoveSimbolosMain() 
        {
            List<Simbolo> simbolosGlobais = Semantico.BuscarSimboloPorEscopo("Global");

            foreach (var item in simbolosGlobais)
            {
                Semantico.RemoverSimbolo(item.ID, "Saida no main.");
            }
        }

        private void RemoveSimbolosIF()
        {
            List<Simbolo> simbolosGlobais = Semantico.BuscarSimboloPorEscopo("Local");

            foreach (var item in simbolosGlobais)
            {
                Semantico.RemoverSimbolo(item.ID, "Saida no if.");
            }
        }

        private void RemoveSimbolosWHILE()
        {
            List<Simbolo> simbolosGlobais = Semantico.BuscarSimboloPorEscopo("Local");

            foreach (var item in simbolosGlobais)
            {
                Semantico.RemoverSimbolo(item.ID, "Saida no while.");
            }
        }

        #region PRINTS
        private void Validado(string bloco, string lexema)
        {
            string str = "|             " + bloco;

            for (int i = 0; i < 15 - bloco.Length; i++)
            {
                str += " ";
            }

            str += "|         " + lexema + "";

            for (int i = 0; i < 21 - lexema.Length; i++)
            {
                str += " ";
            }

            str += "|           OK\n";

            Console.Write(str);
            this.Log += str;
        }

        private void Erro(string esperado, string estrutura, string lexema)
        {
            string str = "|             " + estrutura;

            for (int i = 0; i < 15 - estrutura.Length; i++)
            {
                str += " ";
            }

            str += "|         " + lexema + "";

            for (int i = 0; i < 21 - lexema.Length; i++)
            {
                str += " ";
            }

            str += "|           ERRO\n";

            Console.Write(str);
            this.Log += str;

            string erro = string.Format("\nErro na estrutura '{1}', Esperado '{0}' encontrado o token '{2}'.\n", esperado, estrutura, lexema);

            this.Log += erro;

            Console.Write(erro);

            //Gera arquivo de log da analise Semantico
            File.PrintFile(this.Log, "AnaliseSintaticoLog.txt");

            Environment.Exit(1);
        }
        #endregion
    }
}
