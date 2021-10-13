using DS_PLUS_COMPILER.Src;
using DS_PLUS_COMPILER.Utils;
using System;

namespace DS_PLUS_COMPILER
{

    class DS_PLUS_COMPILER
    {      
        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("BEM VINDO AO {0}! \n\n", Config.Aplicacao));

            //Le o arquivo de entrada
            File fileReader = new (Config.InputPath);

            //Cria o analisador lexico com o buffer do arquivo de entrada
            LEXICA analisadorLexico = new(fileReader.Buffer);

            //Inicia a analise
            analisadorLexico.StartAnaliseLexica();

            Console.WriteLine("");
            Console.ReadKey();
        }
    }
}