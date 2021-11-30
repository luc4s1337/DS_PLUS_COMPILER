using DS_PLUS_COMPILER.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class SEMANTICO
    {
        public Dictionary<int, Simbolo> TabelaDeSimbolos { get; set; } = new Dictionary<int, Simbolo>();
        public int SimbolosId { get; set; } = 0;
        public string LogTabelaSimbolo { get; set; } = "";
        public string LogSemantico { get; set; } = "";
        public string EscopoAtual { get; set; } = "Global";

        public SEMANTICO() 
        {
            InicializaLog();
        }

        #region funcoesTabelaSimbolo
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
            TipoVariavel tipoSimbolo = RetornaTipo(tipo).Value;

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

        private TipoVariavel? RetornaTipo(string tipo)
        {
            TipoVariavel? tipoToReturn;

            switch (tipo)
            {
                case "int":
                    tipoToReturn = TipoVariavel.INT;
                    break;
                case "float":
                    tipoToReturn = TipoVariavel.FLOAT;
                    break;
                case "string":
                    tipoToReturn = TipoVariavel.STRING;
                    break;
                case "char":
                    tipoToReturn = TipoVariavel.CHAR;
                    break;
                default:
                    tipoToReturn = null;
                    break;
            }

            return tipoToReturn;
        }
        #endregion

        #region Prints
        private void InicializaLog() 
        {
            string inicio = "-----------(INICIO)-PRINT-TABELA-DE-SIMBOLOS------------------\n\n";
            LogTabelaSimbolo += inicio;

            string cabecalho = "NOME           TIPO        ESCOPO        INICIALIZADA    ATIVO      STATUS\n\n";
            LogTabelaSimbolo += cabecalho;
        }

        private void GravarLogTabela(string status, Simbolo simbolo) 
        {
            string log = simbolo.NomeVariavel;

            for (int i = 0; i < 15 - simbolo.NomeVariavel.Length; i++)
            {
                log += " ";
            }

            log += simbolo.Tipo.ToString();

            for (int i = 0; i < 12 - simbolo.Tipo.ToString().Length; i++)
            {
                log += " ";
            }

            log += simbolo.Escopo;

            for (int i = 0; i < 14 - simbolo.Escopo.Length; i++)
            {
                log += " ";
            }

            log += simbolo.Inicializada.ToString();

            for (int i = 0; i < 16 - simbolo.Inicializada.ToString().Length; i++)
            {
                log += " ";
            }

            log += simbolo.Ativo.ToString();

            for (int i = 0; i < 11 - simbolo.Ativo.ToString().Length; i++)
            {
                log += " ";
            }
            log += status;

            this.LogTabelaSimbolo += log+"\n";
        }

        public void PrintLogTabela()
        {            
            Console.Write(LogTabelaSimbolo+"\n\n");

            //Le o arquivo de entrada
            File fileReader = new(Config.InputPath);

            //Gera arquivo de log da analise da tabela de simbolos
            fileReader.PrintFile(this.LogTabelaSimbolo, "TabelaSimbolosLog.txt");
        }

        public void PrintLogSemantico()
        {
            string inicio = "-----------(INICIO)-PRINT-ANALISE-SEMANTICA------------------\n\n";
            LogTabelaSimbolo += inicio;
            Console.Write(inicio);

            string cabecalho = "";
            cabecalho += "              BLOCO";
            cabecalho += "                    LEXEMA";
            cabecalho += "                           SITUAÇÃO\n\n";

            LogTabelaSimbolo += cabecalho;
            Console.Write(cabecalho);

            //Le o arquivo de entrada
            File fileReader = new(Config.InputPath);

            //Gera arquivo de log da analise da tabela de simbolos
            fileReader.PrintFile(this.LogTabelaSimbolo, "AnaliseSemanticoLog.txt");
        }
        #endregion
    }
}
