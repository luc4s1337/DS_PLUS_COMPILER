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

            FileManager fileReader = new FileManager()
                .SetFilePath(Config.InputPath)
                .OpenFileStream();

            AnaliseLexicaService analisadorLexico = new AnaliseLexicaService()
                .SetBuffer(fileReader.GetFileBuffer())
                .Execute();

            string logAnaliseLexica = analisadorLexico.PrintLog();

            FileManager.PrintFile(
                logAnaliseLexica,
                "AnaliseLexicaLog.txt"
            );

            SINTATICO analisadorSintatico = new(analisadorLexico.GetTokens());

            analisadorSintatico.StartAnaliseSintatica();
            string logAnaliseSintatica = analisadorSintatico.Log;

            FileManager.PrintFile(logAnaliseSintatica, "AnaliseSintaticoLog.txt");
                        
            Console.ReadKey();

            return 0;
        }
    }
}