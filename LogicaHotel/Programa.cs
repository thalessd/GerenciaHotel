using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;

using Newtonsoft.Json;

namespace LogicaHotel
{
    class Programa
    {
        private static void Menu()
        {
            string nome = "";

            nome = Console.ReadLine();

            Console.WriteLine(nome);
        }

        private static void ConfiguraPrograma()
        {
            string dirNome = "Data";
            string arqConfNome = "HotelLogicaConf.cript";
            string arqDadoNome = "HotelLogicaDados.cript";

            string caminhoDir = @Path.GetFullPath(dirNome);
            string caminhoArqConf = @Path.Combine(caminhoDir, arqConfNome);
            string caminhoArqDado = @Path.Combine(caminhoDir, arqDadoNome);

            Stream StrmConf;
            Stream StrmDado;

            StreamReader Leitor;
            StreamWriter Escritor;

            dynamic ConfStruct;
            object DadoStruct;

            string NomeHotel;
            int QuantidadeQuartos;
            ArrayList Servicos = new ArrayList();

            bool AdicionarServico = true;
            int ContadorServico = 1;

            string AuxNomeServico;
            double AuxPrecoServico;
            string AuxDecisaoAdcServico;


            if (!Directory.Exists(caminhoDir)) Directory.CreateDirectory(caminhoDir);

            if (!File.Exists(caminhoArqConf)) File.Create(caminhoArqConf);
            if (!File.Exists(caminhoArqDado)) File.Create(caminhoArqDado);

            StrmConf = File.Open(caminhoArqConf, FileMode.Open);
            StrmDado = File.Open(caminhoArqDado, FileMode.Open);

            Leitor = new StreamReader(StrmConf);


            if (Leitor.ReadToEnd() == "")
            {   
                Console.Write("Escreva o nome do Hotel: ");
                NomeHotel = Console.ReadLine();

                Console.Write("Quantidade de Quartos do Hotel: ");
                QuantidadeQuartos = int.Parse(Console.ReadLine());

                do // Gera os Serviços
                {
                    if (Servicos.Count > 0) // Cabeçalho com os serviços cadastrados
                    {
                        Console.WriteLine("---------------");
                        Console.WriteLine("NOME : PREÇO");
                        foreach (dynamic servico in Servicos)
                        {
                            Console.Write(servico.Nome);
                            Console.Write(" : ");
                            Console.WriteLine(servico.Preco);
                        }
                        Console.WriteLine("---------------");
                    }
                    
                    // Corpo do cadastro
                    Console.WriteLine("\nAdicionar o " + ContadorServico + "° Serviço:\n");

                    Console.Write("Nome do Serviço: ");
                    AuxNomeServico = Console.ReadLine();

                    Console.Write("Preço do Serviço: ");
                    AuxPrecoServico = double.Parse(Console.ReadLine());

                    Servicos.Add( new { Nome = AuxNomeServico, Preco = AuxPrecoServico } );

                    Console.Write("Deseja adicionar outro serviço? (S/N): ");
                    AuxDecisaoAdcServico = Console.ReadLine();
                    Console.WriteLine(AuxDecisaoAdcServico);

                    // Verificação de teclas
                    if (AuxDecisaoAdcServico == "N" || AuxDecisaoAdcServico == "n") AdicionarServico = false; 

                    ContadorServico++;

                } while (AdicionarServico);

                ConfStruct = new
                {
                    NomeHotel = NomeHotel,
                    QuantidadeQuartos = QuantidadeQuartos,
                    Servico = Servicos
                };

                // Escrevendo arquivo
                
                // Console.WriteLine(ConfStruct.Servico);

                Console.WriteLine(JsonConvert.SerializeObject(ConfStruct));

                // Escritor = new StreamWriter(StrmConf);

                // Escritor.Write(JsonConvert.SerializeObject(ConfStruct));

                // Escritor.Close();
            }




                // Console.WriteLine(ConfData.asd);

            Leitor.Close();

            StrmConf.Close();
            StrmDado.Close();



        }

        public static void Main(string[] args)
        {
            Console.Title = "Hotel Lógica";
            Console.ForegroundColor = ConsoleColor.Green;

            // Menu();

            ConfiguraPrograma();

            Console.ReadKey();
        }
    }
}
