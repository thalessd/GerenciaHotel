using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;


namespace LogicaHotel
{
    class Programa
    {
        private string dirNome = "Data";
        private string arqConfNome = "HotelLogicaConf.cript";
        private string arqDadoNome = "HotelLogicaDados.cript";

        private string caminhoDir;
        private string caminhoArqConf;
        private string caminhoArqDado;


        private static void Menu()
        {
            string nome = "";

            nome = Console.ReadLine();

            Console.WriteLine(nome);
        }

        // Faz a configuração inicial do programa ou reseta
        private void ConfiguraPrograma(bool clear = false)
        {

            Stream StrmConf;
            Stream StrmDado;

            StreamReader Leitor;
            StreamWriter Escritor;

            dynamic ConfStruct;
            dynamic DadoStruct;

            string NomeHotel;
            int QuantidadeQuartos;
            float PrecoDiaria;
            
            

            // Verifica se o diretório existe, se não cria um
            if (!Directory.Exists(this.caminhoDir)) Directory.CreateDirectory(this.caminhoDir);

            // Verifica se os arquivos de dados e configurações existem
            if (!File.Exists(this.caminhoArqConf))
            {
                StrmConf = File.Open(this.caminhoArqConf, FileMode.Create);
                StrmConf.Close();
            }

            if (!File.Exists(this.caminhoArqDado))
            {
                StrmDado = File.Open(this.caminhoArqDado, FileMode.Create);
                StrmDado.Close();
            }
            
            // Abre os dois arquivos
            StrmConf = File.Open(this.caminhoArqConf, FileMode.Open);
            StrmDado = File.Open(this.caminhoArqDado, FileMode.Open);

            Leitor = new StreamReader(StrmConf);

            // Verifica se o Arquivo está vazio
            if (Leitor.ReadToEnd() == "" || clear)
            {   
                Console.Write("Nome do Hotel: ");
                NomeHotel = Console.ReadLine();

                Console.Write("Quantidade de Quartos do Hotel: ");
                QuantidadeQuartos = int.Parse(Console.ReadLine());

                Console.Write("Preço da Diária: R$");
                PrecoDiaria = float.Parse(Console.ReadLine());

                // Corpo do arquivo de Configurações
                ConfStruct = new
                {
                    NomeHotel = NomeHotel,
                    QuantidadeQuartos = QuantidadeQuartos,
                    PrecoDiaria = PrecoDiaria,
                    Servico = new ArrayList()
                };

                // Corpo do arquivo de dados
                DadoStruct = new
                {
                    Clientes = new { }
                };

                // Gerando arquivos padrões
                Escritor = new StreamWriter(StrmConf);
                Escritor.Write( JsonConvert.SerializeObject(ConfStruct) );
                Escritor.Close();

                Escritor = new StreamWriter(StrmDado);
                Escritor.Write( JsonConvert.SerializeObject(DadoStruct) );
                Escritor.Close();
            }

            Leitor.Close();

            StrmConf.Close();
            StrmDado.Close();
        }

        // Gera serviços
        private void CriaServico()
        {
            Stream StrmConf = File.Open(this.caminhoArqConf, FileMode.Open);

            StreamReader StrRe;
            StreamWriter StrWr;

            dynamic ConteudoConf;
            dynamic NewConteudoConf = new ExpandoObject();
            dynamic Servico;

            string AuxSair;
            string AuxNomeServico;
            float AuxPrecoServico;

            bool CadastraServico = true;

            ArrayList ListaServico = new ArrayList();

            StrRe = new StreamReader(StrmConf);

            ConteudoConf = JsonConvert.DeserializeObject(StrRe.ReadToEnd());

            Servico = ConteudoConf.Servico;

            Console.Clear();

            if (Servico.ToString() != "[]")
            {
                foreach (var Dado in Servico){
                    ListaServico.Add(Dado);
                }
            }
            

            do
            {
                if (ListaServico.Count > 0)
                {
                    Console.WriteLine("-----SERVIÇOS----|");
                    foreach (dynamic Dado in ListaServico)
                    {
                        Console.WriteLine("-----------------");
                        Console.WriteLine(" NOME: " + Dado.Nome);
                        Console.WriteLine(" PREÇO: R$" + Dado.Preco);
                        Console.WriteLine("-----------------");
                    }
                    Console.WriteLine("----------------|\n");
                }

                Console.Write("Nome do Serviço: ");
                AuxNomeServico = Console.ReadLine();
                Console.Write("Preço do Serviço: R$");
                AuxPrecoServico = float.Parse(Console.ReadLine());

                ListaServico.Add( new {Nome = AuxNomeServico, Preco = AuxPrecoServico} );

                Console.Write("Deseja cadastrar um serviço? (S/N): ");
                AuxSair = Console.ReadLine();
                if (AuxSair == "N" || AuxSair == "n") CadastraServico = false;
                Console.Clear();

            } while (CadastraServico);


            NewConteudoConf = new
            {
                NomeHotel = ConteudoConf.NomeHotel,
                QuantidadeQuartos = ConteudoConf.QuantidadeQuartos,
                PrecoDiaria = ConteudoConf.PrecoDiaria,
                Servico = ListaServico
            };

            StrRe.Close();
            StrmConf.Close();

            StrmConf = File.Open(this.caminhoArqConf, FileMode.Create);

            StrWr = new StreamWriter(StrmConf);

            StrWr.Write(JsonConvert.SerializeObject(NewConteudoConf));

            StrWr.Close();
            StrmConf.Close();

            Console.WriteLine("Cadastrado com Sucesso!");
            Thread.Sleep(1500);
            Console.Clear();

        }

        public static void Main(string[] args)
        {
            Programa Prog = new Programa();

            Console.Title = "Hotel Lógica";
            Console.ForegroundColor = ConsoleColor.Green;

            Prog.caminhoDir = @Path.GetFullPath(Prog.dirNome);
            Prog.caminhoArqConf = @Path.Combine(Prog.caminhoDir, Prog.arqConfNome);
            Prog.caminhoArqDado = @Path.Combine(Prog.caminhoDir, Prog.arqDadoNome);

            // Menu();

            Prog.ConfiguraPrograma();
            Prog.CriaServico();

            Console.ReadKey();
        }
    }
}
