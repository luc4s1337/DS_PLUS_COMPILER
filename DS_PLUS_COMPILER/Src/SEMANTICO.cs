using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class SEMANTICO
    {
        public List<Simbolo> TabelaDeSimbolos { get; set; } = new List<Simbolo>();
        public List<Token> Tokens { get; set; }
        public int SimbolosIndex { get; set; } = 0;
        public string Log { get; set; } = "";        

        public SEMANTICO(List<Token> _tokens) 
        {
            Tokens = _tokens;
        }

        public void StartAnaliseSemantica() 
        {
        }

        #region funcoesTabelaSimbolo

        //private Simbolo BuscarSimbolo()
        private void BuscarSimbolo()
        {
            //TODO
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

        private void InserirSimbolo(string tipo, string nomeVariavel)
        {
            TipoVariavel tipoSimbolo = RetornaTipo(tipo).Value;

            Simbolo simboloToInsert = new Simbolo(SimbolosIndex, nomeVariavel, tipoSimbolo, true, false, false);
            SimbolosIndex++;

            TabelaDeSimbolos.Add(simboloToInsert);
        }

        private void AtualizarSimbolo()
        {
            //TODO
        }

        private void RemoverSimbolo()
        {
            //TODO
        }

        #endregion

        #region Prints
        public string PrintAnaliseSemantica() 
        {
            return "";
        }
        #endregion
    }
}
