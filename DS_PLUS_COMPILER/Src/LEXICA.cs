using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DS_PLUS_COMPILER.Src.Enums;

namespace DS_PLUS_COMPILER.Src
{
	class LEXICA
	{
		public string Buffer { get; set; }
		public int BufferIndex { get; set; } = 0;
        public List<Token> Tokens { get; set; }

		public int Estado { get; set; } = 0;

		public string LexemaAtual { get; set; } = "";

		public LEXICA(string _buffer)
		{
			this.Buffer = _buffer.ToLower() + " ";
			this.Tokens = new List<Token>();
		}

		public void StartAnaliseLexica()
		{
			foreach (char ch in this.Buffer)
			{
				switch (Estado)
				{
					case 0:
						if (IsDigit(ch))
						{
							this.LexemaAtual += ch;
							this.Estado = 1;
						}
						else
						{
							if (IsAlpha(ch))
							{
								//Palavras reservadas
								this.Estado = 11;
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
										break;
									case '\0':
										break;
									case ' ':
										break;
                                    default:
										Erro("Caractere inválido.");
										break;
								}
							}
						}
						break;
					case 1:
						this.LexemaAtual += ch;

						//se for numero real
						if (this.Buffer[this.BufferIndex + 1] == '.')
						{
							this.Estado = 2;
						}
						else 
						{
							//se o proximo caractere nao for um numero, gera o token
							if (!IsDigit(this.Buffer[this.BufferIndex + 1]))
								InsertToken(Enums.Tokens.LIT_INT);
						}						

						break;
					case 2:
						this.LexemaAtual += ch;

						if (IsDigit(ch)) 
						{
							if (!IsDigit(this.Buffer[this.BufferIndex + 1]) && this.Buffer[this.BufferIndex + 1] != '.')
								InsertToken(Enums.Tokens.LIT_FLT);
						}						

						break;
					case 3:
						if(ch == '=')
						{
							this.LexemaAtual += ch;
							InsertToken(Enums.Tokens.OP_IGUAL);
						}
                        else
						{
							Erro("Caractere inválido.");
						}
						break;
					case 4:
						if (ch == '=')
						{
							this.LexemaAtual += ch;
							InsertToken(Enums.Tokens.OP_DIFERENTE);
                        }
						else{
							Erro("Caractere inválido.");
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
							Erro("Caractere inválido.");
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
							Erro("Caractere inválido.");
						}
						break;
					case 7:						
						this.LexemaAtual += ch;

						if (this.Buffer[BufferIndex+1] == '\'')
						{
							this.Estado = 8;
						}
						else 
						{
							Erro("Caractere inválido.");
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

					//PALAVRAS RESERVADAS
					case 11:
						this.LexemaAtual += ch;

						switch (ch)
						{
							//bool
							case 'b':
								this.Estado = 12;
								break;
							//char
							case 'c':
								this.Estado = 16;
								break;
							//else
							case 'e':
								this.Estado = 20;
								break;
							//for, float
							case 'f':
								this.Estado = 23;
								break;
							//if, int
							case 'i':
								this.Estado = 28;
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
							//void
							case 'v':
								this.Estado = 49;
								break;
							//while
							case 'w':
								this.Estado = 52;
								break;
						}
					
						break;
					case 13:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							this.Estado = 14;
						}
						else
						{
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
						}

						break;
					case 19:
						this.LexemaAtual += ch;

						if (!IsDigit(this.Buffer[this.BufferIndex+1]) && !IsAlpha(this.Buffer[this.BufferIndex + 1]))
						{
							InsertToken(Enums.Tokens.ID);
						}

						break;
					case 20:
						this.LexemaAtual += ch;

						if (ch == 'l')
						{
							this.Estado = 21;
						}
						else
						{
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
						}

						break;
					case 23:
						this.LexemaAtual += ch;

						switch (ch) 
						{
							case 'l':
								this.Estado = 24;
								break;
							case 'o':
								this.Estado = 27;
								break;
                            default:
								Erro("Erro ao gerar token.");
								break;
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
						}

						break;
					case 27:
						this.LexemaAtual += ch;

						if (ch == 'r')
						{
							InsertToken(Enums.Tokens.PR_FOR);
						}
						else
						{
							Erro("Erro ao gerar token.");
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
								this.Estado = 31;
								break;
							default:
								Erro("Erro ao gerar token.");
								break;
						}

						break;
					case 29:
						this.LexemaAtual += ch;

						if (ch == 't')
						{
							this.Estado = 30;
						}
						else
						{
							Erro("Erro ao gerar token.");
						}

						break;
					case 30:
						this.LexemaAtual += ch;

						if (ch == 'r')
						{
							InsertToken(Enums.Tokens.PR_INT);
						}
						else
						{
							Erro("Erro ao gerar token.");
						}

						break;
					case 31:
						this.LexemaAtual += ch;

						if (ch == 'f')
						{
							InsertToken(Enums.Tokens.PR_IF);
						}
						else
						{
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
								Erro("Erro ao gerar token.");
								break;
						}

						break;
					case 42:
						this.LexemaAtual += ch;

						if (ch == 'a')
						{
							this.Estado = 43;
						}
						else
						{
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
						}

						break;
					case 49:
						this.LexemaAtual += ch;

						if (ch == 'o')
						{
							this.Estado = 50;
						}
						else
						{
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
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
							Erro("Erro ao gerar token.");
						}

						break;
				}

				this.BufferIndex++;
			}

			Acabou();
		}

		private void InsertToken(Enums.Tokens _token)
		{
			Token token = new(this.LexemaAtual);
			token.TokenCodigo = _token;

			this.Tokens.Add(token);
			this.Estado = 0;
			this.LexemaAtual = "";
		}

		private void Acabou()
		{
			Token token = new("");
			token.TokenCodigo = Enums.Tokens.FIM;

			this.Tokens.Add(token);
		}

		private bool IsDigit(char c)
		{
			bool isDigit = false;

			int cCode = (int)c;

			if (cCode >= 48 && cCode <= 57)
				isDigit = true;

			return isDigit;
		}

		private bool IsAlpha(char c)
		{
			bool isAlpha = false;

			int cCode = (int)c;

			if (cCode >= 97 && cCode <= 122 || cCode >= 65 && cCode <= 90)
				isAlpha = true;

			return isAlpha;
		}

		private void Erro(string erro)
		{
			Console.WriteLine(erro);
		}

		public string PrintAnalise() 
		{
			string print = "---------- (INICIO) PRINT LEXICO ---------------\n\n";


			print += "         LEXEMA";
			print += "               TOKEN\n\n";

			foreach (var item in this.Tokens) 
			{
				print += "|        ";
				print += item.Lexema;

                for (int i = 0; i < 15 - item.Lexema.Length; i++)
				{
					print += " ";
				}

			print += "|     " +item.TokenCodigo+ "\n";
			}

			print +="\n\n-------- (FIM) PRINT LEXICO ---------------\n\n";

			Console.Write(print);

			return print;
		}		
	}	
}