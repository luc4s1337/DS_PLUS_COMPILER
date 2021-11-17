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
        public int TokensIndex { get; set; } = 0;

        public SINTATICO(List<Token> _tokens) 
        {
            this.Tokens = _tokens;
        }

        public void StartAnaliseSintatica() 
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_VOID)
            {
                TokensIndex++;
                Main();                
            }
            else 
            {
                Erro("void", "Main", Tokens[TokensIndex].Lexema);
            }                
        }

        private void Main() 
        {
            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.PR_MAIN)
            {
                TokensIndex++;
                if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_PARENTESES)
                {
                    TokensIndex++;
                    if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_PARENTESES)
                    {
                        TokensIndex++;
                        if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.ABRE_COLCHETES)
                        {
                            TokensIndex++;
                            if (Tokens[TokensIndex].TokenCodigo == Enums.Tokens.FECHA_COLCHETES)
                            {
                                TokensIndex++;
                                Validado("Main");
                                Bloco();
                            }
                            else
                            {
                                Erro("}", "Main", Tokens[TokensIndex].Lexema);
                            }
                        }
                        else
                        {
                            Erro("{", "Main", Tokens[TokensIndex].Lexema);
                        }
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
            else
            {
                Erro("main", "Main", Tokens[TokensIndex].Lexema);
            }
        }

        private void Bloco()
        { 
        }

        private void Validado(string bloco) 
        {
            Console.WriteLine(string.Format("Bloco {0} validado com sucesso.", bloco));
        }

        private void Erro(string esperado, string estrutura, string lexema) 
        {
            Console.WriteLine(string.Format("Erro na estrutura {1}, Esperado '{0}' encontrado o token '{2}'.", esperado, estrutura, lexema));
        }

        public string PrintAnalise()
        {
            return "";
        }
    }
}
