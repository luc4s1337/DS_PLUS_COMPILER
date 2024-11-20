using DS_PLUS_COMPILER.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class AnaliseSemanticaService
    {
        public Dictionary<int, Simbolo> TabelaDeSimbolos { get; set; } = new Dictionary<int, Simbolo>();
        public int SimbolosId { get; set; } = 0;
        public string LogTabelaSimbolo { get; set; } = "";
        public string LogSemantico { get; set; } = "";
        public string EscopoAtual { get; set; } = "Global";

        public AnaliseSemanticaService() 
        {
            InicializaLogTS();
            InicializaLogSEMANTICO();
        }

        #region operacoesTabelaSimbolo
        public Simbolo BuscarSimbolo(int id)
        {
            return TabelaDeSimbolos.ElementAt(id).Value;
        }              

        public Simbolo BuscarSimbolo(string nomeVariavel)
        {
            Simbolo simboloToReturn = null;

            foreach (var item in TabelaDeSimbolos) 
            {
                if (item.Value.NomeVariavel == nomeVariavel && item.Value.Ativo) 
                {
                    simboloToReturn = item.Value;
                }
            }

            return simboloToReturn;
        }

        public List<Simbolo> BuscarSimboloPorEscopo(string escopo)
        {
            List<Simbolo> simboloToReturn = new List<Simbolo>();

            foreach (var item in TabelaDeSimbolos)
            {
                if (item.Value.Escopo == escopo)
                {
                    simboloToReturn.Add(item.Value);
                }
            }

            return simboloToReturn;
        }

        public void InserirSimbolo(string tipo, string nomeVariavel, string status)
        {
            Enums.Tokens tipoSimbolo = RetornaTipo(tipo).Value;

            Simbolo simboloToInsert = new Simbolo(SimbolosId, nomeVariavel, tipoSimbolo, EscopoAtual, false, true);            

            TabelaDeSimbolos.Add(SimbolosId, simboloToInsert);
            GravarLogTabela(status, simboloToInsert);
            SimbolosId++;
        }

        public void AtualizarSimbolo(int id, bool _inicializada)
        {
            Simbolo simbolo = BuscarSimbolo(id);
                
            TabelaDeSimbolos.ElementAt(id).Value.Inicializada = _inicializada;

            GravarLogTabela("Inicializa a variavel", simbolo);
        }

        public void RemoverSimbolo(int id, string saida)
        {
            TabelaDeSimbolos.ElementAt(id).Value.Ativo = false;
            GravarLogTabela(saida, TabelaDeSimbolos.ElementAt(id).Value);
        }

        private Enums.Tokens? RetornaTipo(string tipo)
        {
            Enums.Tokens? tipoToReturn;

            switch (tipo)
            {
                case "int":
                    tipoToReturn = Enums.Tokens.PR_INT;
                    break;
                case "float":
                    tipoToReturn = Enums.Tokens.PR_FLT;
                    break;
                case "string":
                    tipoToReturn = Enums.Tokens.PR_STR;
                    break;
                case "char":
                    tipoToReturn = Enums.Tokens.PR_CHAR;
                    break;
                default:
                    tipoToReturn = null;
                    break;
            }

            return tipoToReturn;
        }

        public Enums.Tokens? TipoCast(Enums.Tokens tipo)
        {
            Enums.Tokens? tipoToReturn = null;

            switch (tipo) 
            {
                case Enums.Tokens.PR_INT:
                    tipoToReturn = Enums.Tokens.LIT_INT;
                    break;
                case Enums.Tokens.PR_FLT:
                    tipoToReturn = Enums.Tokens.LIT_FLT;
                    break;
                case Enums.Tokens.PR_STR:
                    tipoToReturn = Enums.Tokens.LIT_STR;
                    break;
                case Enums.Tokens.PR_CHAR:
                    tipoToReturn = Enums.Tokens.LIT_CHAR;
                    break;
            }

            return tipoToReturn;
        }
        #endregion

        #region Prints
        private void GravarLogTabela(string status, Simbolo simbolo) 
        {
            string log = simbolo.NomeVariavel;
            for (int i = 0; i < 15 - simbolo.NomeVariavel.Length; i++) log += " ";

            log += simbolo.Tipo.ToString();
            for (int i = 0; i < 12 - simbolo.Tipo.ToString().Length; i++) log += " ";

            log += simbolo.Escopo;
            for (int i = 0; i < 14 - simbolo.Escopo.Length; i++) log += " ";

            log += simbolo.Inicializada.ToString();
            for (int i = 0; i < 16 - simbolo.Inicializada.ToString().Length; i++) log += " ";

            log += simbolo.Ativo.ToString();
            for (int i = 0; i < 11 - simbolo.Ativo.ToString().Length; i++) log += " ";

            log += status;

            this.LogTabelaSimbolo += log+"\n";
        }

        public void GravarLogSemantico(string estrutura, string lexemas, string tipo, string status)
        {
            string log = estrutura;
            for (int i = 0; i < 15 - estrutura.Length; i++) log += " ";

            log += lexemas;
            for (int i = 0; i < 20 - lexemas.Length; i++) log += " ";

            log += tipo;
            for (int i = 0; i < 20 - tipo.Length; i++) log += " ";

            log += status;           

            this.LogSemantico += log + "\n";
        }

        public void ErroSemantico(string erroMSG, int linha)
        {
            string erro = string.Format("\nErro na linha {1} -> {0} \n", erroMSG, linha);
            this.LogSemantico += erro;

            PrintLogTabela();
            PrintLogSemantico();

            Environment.Exit(1);
        }

        private void InicializaLogTS()
        {
            string inicio = "\n\n-----------(INICIO)-PRINT-TABELA-DE-SIMBOLOS------------------\n\n";
            LogTabelaSimbolo += inicio;

            string cabecalho = "NOME           TIPO        ESCOPO        INICIALIZADA    ATIVO      SITUAÇÃO\n\n";
            LogTabelaSimbolo += cabecalho;
        }

        public void PrintLogTabela()
        {            
            Console.Write(LogTabelaSimbolo+"\n\n");

            //Gera arquivo de log da analise da tabela de simbolos
            FileManager.PrintFile(this.LogTabelaSimbolo, "TabelaSimbolosLog.txt");
        }

        private void InicializaLogSEMANTICO()
        {
            string inicio = "-----------(INICIO)-PRINT-ANALISE-SEMANTICA------------------\n\n";
            LogSemantico += inicio;

            string cabecalho = "ESTRUTURA      LEXEMA(S)           TIPO                SITUAÇÃO\n\n";
            LogSemantico += cabecalho;
        }

        public void PrintLogSemantico()
        {           
            Console.Write(LogSemantico + "\n\n");

            //Gera arquivo de log da analise semantica
            FileManager.PrintFile(this.LogSemantico, "AnaliseSemanticoLog.txt");
        }
        #endregion
    }
}
