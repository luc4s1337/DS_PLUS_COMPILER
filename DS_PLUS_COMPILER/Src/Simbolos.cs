using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_PLUS_COMPILER.Src
{
    class Simbolo
    {
        //Attr
        public int ID { get; set; }
        public string NomeVariavel { get; set; }
        public Enums.Tokens Tipo { get; set; }
        public string Escopo { get; set; }
        public bool Inicializada { get; set; }
        public bool Ativo { get; set; }

        public Simbolo(int _id, string _nomeVariavel, Enums.Tokens _tipo, string _escopo, bool _inicializada, bool _ativo) 
        {
            ID = _id;
            NomeVariavel = _nomeVariavel;
            Tipo = _tipo;
            Escopo = _escopo;
            Inicializada = _inicializada;
            Ativo = _ativo;
        }
    }
}
