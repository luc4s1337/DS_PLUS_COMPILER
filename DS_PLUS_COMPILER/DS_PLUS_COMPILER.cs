using DS_PLUS_COMPILER.Utils;
using System;

namespace DS_PLUS_COMPILER
{
    class DS_PLUS_COMPILER
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("BEM VINDO AO {0}! \n\n", Config.Aplicacao));

            Console.WriteLine("Foram encontrados os seguintes arquivos na pasta de entrada. \n\n");

            FileReader fileReader = new FileReader();

            fileReader.ListaArquivos();
        }
    }
}