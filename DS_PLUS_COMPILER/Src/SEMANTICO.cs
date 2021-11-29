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
        public string Log { get; set; } = "";
        public string EscopoAtual { get; set; } = "Global";

        public SEMANTICO() 
        {
        }

        public void StartAnaliseSemantica() 
        {
        }

        #region funcoesTabelaSimbolo
        private Simbolo BuscarSimbolo(int id)
        {
            return TabelaDeSimbolos.ElementAt(id).Value;
        }
               
        private void InserirSimbolo(string tipo, string nomeVariavel)
        {
            TipoVariavel tipoSimbolo = RetornaTipo(tipo).Value;

            Simbolo simboloToInsert = new Simbolo(SimbolosId, nomeVariavel, tipoSimbolo, EscopoAtual, false, false);            

            TabelaDeSimbolos.Add(SimbolosId, simboloToInsert);
            SimbolosId++;
        }

        private void AtualizarSimbolo(int id, bool _inicializada)
        {
            TabelaDeSimbolos.ElementAt(id).Value.Inicializada = _inicializada;
        }

        private void RemoverSimbolo(int id)
        {
            TabelaDeSimbolos.ElementAt(id).Value.Ativo = false;
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
            string erro = string.Format("\nErro semantico '{1}', Esperado '{0}' encontrado o token '{2}'.\n", esperado, estrutura, lexema);

            this.Log += erro;

            Console.Write(erro);

            //Le o arquivo de entrada
            File fileReader = new(Config.InputPath);

            //Gera arquivo de log da analise semantico
            fileReader.PrintFile(this.Log, "AnaliseSemanticoLog.txt");

            Environment.Exit(1);
        }
        #endregion
    }
}
