using DS_PLUS_COMPILER.Src;
using DS_PLUS_COMPILER.Utils;
using System;

namespace DS_PLUS_COMPILER
{

    class DS_PLUS_COMPILER
    {      
        static int Main(string[] args)
        {
            Console.WriteLine(string.Format("BEM VINDO AO {0}! \n\n", Config.Aplicacao));

            //Le o arquivo de entrada
            File fileReader = new (Config.InputPath);

            //Cria o analisador lexico com o buffer do arquivo de entrada
            LEXICA analisadorLexico = new(fileReader.Buffer);

            //Inicia a analise lexica
            analisadorLexico.StartAnaliseLexica();
            string lexicoPrint = analisadorLexico.PrintAnalise();

            //Gera arquivo de log da analise lexica
            fileReader.PrintFile(lexicoPrint, "AnaliseLexicaLog.txt");

            //Cria o analisador sintatico e utiliza a lista de tokens gerados no lexico
            SINTATICO analisadorSintatico = new(analisadorLexico.Tokens);

            //Inicia a analise sintatica
            analisadorSintatico.StartAnaliseSintatica();
            string sintaticoPrint = analisadorSintatico.Log;

            //Gera arquivo de log da analise sintatica
            fileReader.PrintFile(sintaticoPrint, "AnaliseSintaticoLog.txt");
                        
            Console.ReadKey();

            return 0;
        }
    }
}