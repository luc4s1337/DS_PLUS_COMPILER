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

		public int Estado { get; set; } = 1;

		public string LexemaAtual { get; set; } = "";

		public LEXICA(string _buffer)
		{
			this.Buffer = _buffer;
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
							this.Estado = 1;
						}
						else
						{
							if (IsAlpha(ch))
							{
								this.Estado = 2;
							}
							else
							{
								switch (ch)
								{
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
									case '\t':
										break;
									case '\0':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.FIM);
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
						break;
					case 2:								
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
						else
						{
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
				}

				this.BufferIndex++;
			}
		}

		private void InsertToken(Enums.Tokens _token)
		{
			Token token = new(this.LexemaAtual);
			token.TokenCodigo = _token;

			this.Tokens.Add(token);
			this.Estado = 0;
			this.LexemaAtual = "";
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

		public void PrintAnalise() 
		{
			Console.WriteLine("---------- (INICIO) PRINT LEXICO ---------------\n\n");

			foreach (var item in this.Tokens) 
			{
				Console.WriteLine(item.Lexema + "    |     "+item.TokenCodigo );
			}

			Console.WriteLine("\n\n-------- (FIM) PRINT LEXICO ---------------");
		}		
	}	
}
