using DS_PLUS_COMPILER.Utils;
using System;

namespace DS_PLUS_COMPILER
{

    class DS_PLUS_COMPILER
    {
        private static readonly string inputPath = "..\\Entrada\\Programa.d";

        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("BEM VINDO AO {0}! \n\n", Config.Aplicacao));

            File fileReader = new File(inputPath);

            Console.WriteLine("");
        }
    }
}