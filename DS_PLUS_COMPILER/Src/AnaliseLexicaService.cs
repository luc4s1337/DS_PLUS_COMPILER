﻿using DS_PLUS_COMPILER.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DS_PLUS_COMPILER.Src.Enums;

namespace DS_PLUS_COMPILER.Src
{
	class AnaliseLexicaService
	{
        private string Buffer { get; set; }
        private int BufferIndex = 0;
        private List<Token> Tokens { get; set; } = new List<Token>();

        private int Estado { get; set; } = 0;

        private string LexemaAtual { get; set; } = "";

        private int Linha { get; set; } = 1;

        public void IncrementBufferIndex()
        {
            this.BufferIndex = this.BufferIndex++;
        }

        public int GetBufferIndex()
        {
            return this.BufferIndex;
        }

        public AnaliseLexicaService SetBuffer(string _buffer) {
			this.Buffer = _buffer.ToLower() + " ";
			return this;
		}

		public List<Token> GetTokens() { 
			return this.Tokens; 
		}

        #region CODIGO
        public AnaliseLexicaService Execute()
		{
			while (BufferIndex < this.Buffer.Length-1) 
			{
				char ch = this.Buffer[BufferIndex];
			
				switch (Estado)
				{
					case 0:
						if (Utils.IsDigit(ch))
						{
							this.LexemaAtual += ch;
							this.Estado = 1;
						}
						else
						{
							if (Utils.IsAlpha(ch))
							{
								this.LexemaAtual += ch;

								//palavras reservadas
								switch (ch)
								{
									//bool
									case 'b':
										this.Estado = 13;
										break;
									//char
									case 'c':
										this.Estado = 16;
										break;
									//do
									case 'd':
										this.Estado = 63;
										break;
									//else
									case 'e':
										this.Estado = 20;
										break;
									//float
									case 'f':
										this.Estado = 23;
										break;
									//if, int
									case 'i':
										this.Estado = 28;
										break;
									//main
									case 'm':
										this.Estado = 56;
										break;
									//loop
									case 'l':
										this.Estado = 64;
										break;
									//print
									case 'p':
										this.Estado = 32;
										break;
									//return
									case 'r':
										this.Estado = 36;
										break;
									//scan, string
									case 's':
										this.Estado = 41;
										break;
									//then
									case 't':
										this.Estado = 60;
										break;
									//void, var
									case 'v':
										this.Estado = 49;
										break;
									//while
									case 'w':
										this.Estado = 52;
										break;
								}
							}
							else
							{
								switch (ch)
								{
									//ID
									case '_':
										this.LexemaAtual += ch;
										this.Estado = 19;

										break;
									case '=':
										this.LexemaAtual += ch;

										if (Buffer[BufferIndex + 1] != '=')
											InsertToken(Enums.Tokens.OP_ATRI);
										else
											this.Estado = 3;

										break;
									case '!':
										this.LexemaAtual += ch;

										if (Buffer[BufferIndex + 1] != '=')
											InsertToken(Enums.Tokens.OP_NEGA);
										else
											this.Estado = 4;

										break;
									case '<':
										this.LexemaAtual += ch;

										if (Buffer[BufferIndex + 1] != '=')
											InsertToken(Enums.Tokens.OP_MENOR);
										else
											this.Estado = 5;

										break;
									case '>':
										this.LexemaAtual += ch;

										if (Buffer[BufferIndex + 1] != '=')
											InsertToken(Enums.Tokens.OP_MAIOR);
										else
											this.Estado = 6;

										break;
									case '+':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.OP_SOMA);
										break;
									case '-':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.OP_SUB);
										break;
									case '/':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.OP_DIV);
										break;
									case '*':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.OP_MULT);
										break;
									case '%':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.OP_MOD);
										break;
									case ',':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.VIRGULA);
										break;
									case ';':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.PONTO_VIRGULA);
										break;
									case '{':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.ABRE_CHAVES);
										break;
									case '}':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.FECHA_CHAVES);
										break;
									case '(':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.ABRE_PARENTESES);
										break;
									case ')':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.FECHA_PARENTESES);
										break;
									case '[':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.ABRE_COLCHETES);
										break;
									case ']':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.FECHA_COLCHETES);
										break;
									case '\'':
										this.LexemaAtual += ch;
										this.Estado = 7;

										break;
									case '"':
										this.LexemaAtual += ch;
										this.Estado = 9;

										break;
									case '\t':
									case '\0':									
									case '\n':
									case ' ':
										break;
									case '\r':
										this.Linha++;
										break;
									default:
										PrintMessage("Erro", "Caractere inválido.", this.LexemaAtual);
										break;
								}
							}
						}
						break;
					case 1:
						if (!Utils.IsDigit(this.Buffer[this.BufferIndex]) && this.Buffer[this.BufferIndex] != '.')
						{
							InsertToken(Enums.Tokens.LIT_INT);
							BufferIndex--;
						}
						else 
						{
							this.LexemaAtual += ch;

							//se for numero real
							if (this.Buffer[this.BufferIndex + 1] == '.' || this.Buffer[this.BufferIndex] == '.')
							{
								this.Estado = 2;
							}
							else
							{
								//se o proximo caractere nao for um numero, gera o token
								if (!Utils.IsDigit(this.Buffer[this.BufferIndex + 1]))
									InsertToken(Enums.Tokens.LIT_INT);
							}
						}			

						break;
					case 2:
						this.LexemaAtual += ch;

						if (Utils.IsDigit(ch))
						{
							if (!Utils.IsDigit(this.Buffer[this.BufferIndex + 1]) && this.Buffer[this.BufferIndex + 1] != '.')
								InsertToken(Enums.Tokens.LIT_FLT);
						}

						break;
					case 3:
						if (ch == '=')
						{
							this.LexemaAtual += ch;
							InsertToken(Enums.Tokens.OP_IGUAL);
						}
						else
						{
							PrintMessage("Erro", "Operador inválido.", this.LexemaAtual);
						}
						break;
					case 4:
						if (ch == '=')
						{
							this.LexemaAtual += ch;
							InsertToken(Enums.Tokens.OP_DIFERENTE);
						}
						else {
							PrintMessage("Erro", "Operador inválido.", this.LexemaAtual);
						}
						break;
					case 5:
						if (ch == '=')
						{
							this.LexemaAtual += ch;
							InsertToken(Enums.Tokens.OP_MENOR_IGUAL);
						}
						else
						{
							PrintMessage("Erro", "Operador inválido.", this.LexemaAtual);
						}
						break;
					case 6:
						if (ch == '=')
						{
							this.LexemaAtual += ch;
							InsertToken(Enums.Tokens.OP_MAIOR_IGUAL);
						}
						else
						{
							PrintMessage("Erro", "Operador inválido.", this.LexemaAtual);
						}
						break;
					case 7:
						this.LexemaAtual += ch;

						if (this.Buffer[BufferIndex + 1] == '\'')
						{
							this.Estado = 8;
						}
						else
						{
							PrintMessage("Erro", "Char inválido.", this.LexemaAtual);
						}

						break;
					case 8:
						this.LexemaAtual += ch;
						InsertToken(Enums.Tokens.LIT_CHAR);
						break;
					case 9:
						this.LexemaAtual += ch;

						if (this.Buffer[BufferIndex + 1] == '"')
						{
							this.Estado = 10;
						}
						break;
					case 10:
						this.LexemaAtual += ch;
						InsertToken(Enums.Tokens.LIT_STR);
						break;
					case 13:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							this.Estado = 14;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 14:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							this.Estado = 15;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 15:
						this.LexemaAtual += ch;

						if (ch == 'l')
						{
							InsertToken(Enums.Tokens.PR_BOOL);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 16:
						this.LexemaAtual += ch;

						if (ch == 'h')
						{
							this.Estado = 17;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 17:
						this.LexemaAtual += ch;

						if (ch == 'a')
						{
							this.Estado = 18;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 18:
						this.LexemaAtual += ch;

						if (ch == 'r')
						{
							InsertToken(Enums.Tokens.PR_CHAR);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 19:
						this.LexemaAtual += ch;

						if (!Utils.IsDigit(this.Buffer[this.BufferIndex + 1]) &&
							 !Utils.IsAlpha(this.Buffer[this.BufferIndex + 1]) &&
							 this.Buffer[this.BufferIndex + 1] != '_')
						{
							InsertToken(Enums.Tokens.ID);
						}

						break;
					case 20:
						this.LexemaAtual += ch;

						switch (ch)
						{
							case 'l':
								this.Estado = 21;
								break;
							case 'n':
								this.Estado = 30;
								break;
							default:
								PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
								break;
						}

						break;
					case 21:
						this.LexemaAtual += ch;

						if (ch == 's') 
						{
							this.Estado = 22;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}
						
						break;
					case 22:
						this.LexemaAtual += ch;

						if (ch == 'e')
						{
							InsertToken(Enums.Tokens.PR_ELSE);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 23:
						this.LexemaAtual += ch;

						if (ch == 'l')
						{
							this.Estado = 24;
						}
						else 
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 24:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							this.Estado = 25;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 25:
						this.LexemaAtual += ch;

						if (ch == 'a')
						{
							this.Estado = 26;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 26:
						this.LexemaAtual += ch;

						if (ch == 't')
						{
							InsertToken(Enums.Tokens.PR_FLT);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;					
					case 28:
						this.LexemaAtual += ch;

						switch (ch)
						{
							case 'n':
								this.Estado = 29;
								break;
							case 'f':
								InsertToken(Enums.Tokens.PR_IF);
								break;
							default:
								PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
								break;
						}

						break;
					case 29:
						this.LexemaAtual += ch;

						if (ch == 't')
						{
							InsertToken(Enums.Tokens.PR_INT);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 30:
						this.LexemaAtual += ch;

						if (ch == 'd')
						{
							InsertToken(Enums.Tokens.PR_END);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 32:
						this.LexemaAtual += ch;

						if (ch == 'r')
						{
							this.Estado = 33;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 33:
						this.LexemaAtual += ch;

						if (ch == 'i')
						{
							this.Estado = 34;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 34:
						this.LexemaAtual += ch;

						if (ch == 'n')
						{
							this.Estado = 35;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 35:
						this.LexemaAtual += ch;

						if (ch == 't')
						{
							InsertToken(Enums.Tokens.PR_PRINT);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 36:
						this.LexemaAtual += ch;

						if (ch == 'e')
						{
							this.Estado = 37;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 37:
						this.LexemaAtual += ch;

						if (ch == 't')
						{
							this.Estado = 38;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 38:
						this.LexemaAtual += ch;

						if (ch == 'u')
						{
							this.Estado = 39;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 39:
						this.LexemaAtual += ch;

						if (ch == 'r')
						{
							this.Estado = 40;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 40:
						this.LexemaAtual += ch;

						if (ch == 'n')
						{
							InsertToken(Enums.Tokens.PR_RTN);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 41:
						this.LexemaAtual += ch;

						switch (ch)
						{
							case 'c':
								this.Estado = 42;
								break;
							case 't':
								this.Estado = 45;
								break;
							default:
								PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
								break;
						}

						break;
					case 42:
						this.LexemaAtual += ch;

						if (ch == 'a')
						{
							this.Estado = 44;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 44:
						this.LexemaAtual += ch;

						if (ch == 'n')
						{
							InsertToken(Enums.Tokens.PR_SCN);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 45:
						this.LexemaAtual += ch;

						if (ch == 'r')
						{
							this.Estado = 46;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 46:
						this.LexemaAtual += ch;
						
						if (ch == 'i')
						{
							this.Estado = 47;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 47:
						this.LexemaAtual += ch;

						if (ch == 'n')
						{
							this.Estado = 48;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 48:
						this.LexemaAtual += ch;

						if (ch == 'g')
						{
							InsertToken(Enums.Tokens.PR_STR);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 49:
						this.LexemaAtual += ch;

						switch (ch) 
						{
							case 'o':
								this.Estado = 50;
								break;
							case 'a':
								this.Estado = 67;
								break;
							default:
								PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
								break;
						}

						break;
					case 50:
						this.LexemaAtual += ch;

						if (ch == 'i')
						{
							this.Estado = 51;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 51:
						this.LexemaAtual += ch;

						if (ch == 'd')
						{
							InsertToken(Enums.Tokens.PR_VOID);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 52:
						this.LexemaAtual += ch;

						if (ch == 'h')
						{
							this.Estado = 53;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 53:
						this.LexemaAtual += ch;

						if (ch == 'i')
						{
							this.Estado = 54;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 54:
						this.LexemaAtual += ch;

						if (ch == 'l')
						{
							this.Estado = 55;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 55:
						this.LexemaAtual += ch;

						if (ch == 'e')
						{
							InsertToken(Enums.Tokens.PR_WHILE);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 56:
						this.LexemaAtual += ch;

						if (ch == 'a')
						{
							this.Estado = 57;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 57:
						this.LexemaAtual += ch;

						if (ch == 'i')
						{
							this.Estado = 58;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 58:
						this.LexemaAtual += ch;

						if (ch == 'n')
						{
							InsertToken(Enums.Tokens.PR_MAIN);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 60:
						this.LexemaAtual += ch;

						if (ch == 'h')
						{
							this.Estado = 61;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 61:
						this.LexemaAtual += ch;

						if (ch == 'e')
						{
							this.Estado = 62;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 62:
						this.LexemaAtual += ch;

						if (ch == 'n')
						{
							InsertToken(Enums.Tokens.PR_THEN);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 63:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							InsertToken(Enums.Tokens.PR_DO);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 64:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							this.Estado = 65;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
                        }

                        break;
					case 65:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							this.Estado = 66;
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 66:
						this.LexemaAtual += ch;

						if (ch == 'p')
						{
							InsertToken(Enums.Tokens.PR_LOOP);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
					case 67:
						this.LexemaAtual += ch;

						if (ch == 'r')
						{
							InsertToken(Enums.Tokens.PR_VAR);
						}
						else
						{
							PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual);
						}

						break;
				}

                this.BufferIndex++;
			}

            LastToken();
			return this;
		}
		#endregion

		#region BASIC METHODS
		private void InsertToken(Enums.Tokens _token)
		{
			if ((int)_token >= 6 && (int)_token <= 17) 
			{
				if (Buffer[this.GetBufferIndex() + 1] != ' ' && Buffer[this.GetBufferIndex() + 1] != '\r') 
				{
                    PrintMessage("Erro", "Comando não identificado.", this.LexemaAtual + Buffer[this.GetBufferIndex() + 1]);
                }
            }
				
			this.Tokens.Add(
                new Token()
					.SetLexema(this.LexemaAtual)
					.SetCodigo(_token)
					.SetLinha(this.Linha)
            );

			this.Estado = 0;
			this.LexemaAtual = "";
		}

		private void LastToken()
		{
			this.Tokens.Add(
				new Token()
					.SetLexema("")
					.SetCodigo(Enums.Tokens.FIM)
			);
		}		

		private void PrintMessage(string tipo, string erro, string lexema)
		{
			if (tipo == "Erro")
			{
				string message = PrintLog();
				string erroMessage = string.Format("'{0}': {1}, Lexema: {2}, Linha -> {3}", tipo, erro, lexema, this.Linha);

                message += erroMessage;

				Console.WriteLine(erroMessage);

				//Gera arquivo de log da analise semantico
				FileManager.PrintFile(message, "AnaliseLexicaLog.txt");

				Environment.Exit(1);
			}				
		}

		public string PrintLog() 
		{
			string print = "---------------(INICIO)-PRINT-LEXICO----------------\n\n";
			
			print += "              LEXEMA";
			print += "                                   TOKEN\n\n";

			foreach (var item in this.Tokens) 
			{
				print += "|             ";
				print += item.GetLexema();

                for (int i = 0; i < 30 - item.GetLexema().Length; i++)
				{
					print += " ";
				}

				print += "|          " + item.GetCodigo() +      "\n";
			}

			print += "\n\n";

			Console.Write(print);

			return print;
		}
        #endregion
    }
}