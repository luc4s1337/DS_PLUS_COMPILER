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

            FileManager.PrintFile(
                analisadorLexico.GetLog(),
                "AnaliseLexicaLog.txt"
            );

            AnaliseSintaticaService analisadorSintatico = new AnaliseSintaticaService()
                .SetTokens(analisadorLexico.GetTokens())
                .Execute();

            FileManager.PrintFile(
                analisadorSintatico.Log,
                "AnaliseSintaticoLog.txt"
            );
                        
            Console.ReadKey();

            return 0;
        }
    }
}