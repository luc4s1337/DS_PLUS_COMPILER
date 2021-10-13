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
					case 1:
						if (IsDigit(ch))
						{
							this.Estado = 2;
						}
						else
						{
							if (IsAlpha(ch))
							{
								this.Estado = 3;
							}
							else
							{
								switch (ch)
								{
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
									case '\t':
										break;
									case '\0':
										this.LexemaAtual += ch;
										InsertToken(Enums.Tokens.FIM);
										break;
									case ' ':
										break;
								}
							}
						}
						break;
						/*else
									Erro("Caractere invalido");
								break;

							case 2:
								while (isdigit(ch))
								{
									ch = proxChar();
									printf("%c", ch);
								}

								if (ch == '.')
								{
									ch = proxChar();

									while (isdigit(ch))
									{
										ch = proxChar();
										printf("%c", ch);
									}

									voltaUm();
									return NUM_FLOAT;
								}
								else
								{
									voltaUm();
									return NUM_INT;
								}

								break;

							case 3:
								while (isdigit(ch) or isalpha(ch) or ch == '_'){
									ch = proxChar();
									printf("%c", ch);
								}
								voltaUm();
								//token.TokenCodigo = Enums.Tokens.ID; // nome variavel

								break;*/
				}
			}
		}

		private void InsertToken(Enums.Tokens _token)
		{
			Token token = new Token(this.LexemaAtual);
			token.TokenCodigo = _token;

			this.Tokens.Add(token);
			this.Estado = 1;
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
