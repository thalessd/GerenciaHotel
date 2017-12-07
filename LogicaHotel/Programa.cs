using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading;
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

        /*
         * Funções para trabalhar com a data
         */

        // Verifica se o ano é bisexto
        private int AnoBisexto(int ano)
        {
            if (ano % 400 == 0) return 1;
            if (ano % 100 == 0) return 0;
            if (ano % 4 == 0)   return 1;

            return 0;
        }

        // Verifica qual é o dia do ano de acordo com a data
        private int DiaDoAno(int[] data)
        {
            int dias = 0;
            int[] numDiaMes = new int[12]
            {
                31, 28 + AnoBisexto(data[2]),
                31, 30,
                31, 30, 31, 31,
                30, 31, 30, 31
            };

            for (int i = 0; i < data[1]; i++)
            {
                dias += numDiaMes[i];
            }
            dias += data[0];

            return dias;
        }

        // Calcula duas strings de datas
        private int CalculaData(string DataInicial, string DataFinal)
        {
            int[] PrimeiraData = new int[3];
            int[] SegundaData = new int[3];

            int index = 0;

            int diasInicio;
            int diasFim;
            int difDias;
            
            foreach (var data in DataInicial.Split('/'))
            {
                PrimeiraData[index] = int.Parse(data);
                index++;
            }

            index = 0;

            foreach (var data in DataFinal.Split('/'))
            {
                SegundaData[index] = int.Parse(data);
                index++;
            }

            diasInicio = DiaDoAno(PrimeiraData);
            diasFim = DiaDoAno(SegundaData);

            difDias = diasFim - diasInicio;

            for (int i = PrimeiraData[2]; i < SegundaData[2]; i++)
            {
                difDias += 365 + AnoBisexto(PrimeiraData[2]);
            }

            return difDias;
        }

        // Pega as Configurações do Hotel
        private dynamic PegaConfigHotel()
        {
            Stream StrmConf = File.Open(caminhoArqConf, FileMode.Open, FileAccess.Read);
            StreamReader StrmRe;
            dynamic ObjConf = new ExpandoObject();


            StrmRe = new StreamReader(StrmConf);

            ObjConf = JsonConvert.DeserializeObject(StrmRe.ReadToEnd());

            StrmRe.Close();
            StrmConf.Close();

            return ObjConf;

        }

        // Pega Lista de Clientes do Hotel
        private dynamic PegaClientesHotel()
        {
            Stream StrmDado = File.Open(caminhoArqDado, FileMode.Open, FileAccess.Read);
            StreamReader StrmRe;
            dynamic ObjDado = new ExpandoObject();


            StrmRe = new StreamReader(StrmDado);

            ObjDado = JsonConvert.DeserializeObject(StrmRe.ReadToEnd());

            StrmRe.Close();
            StrmDado.Close();

            return ObjDado;

        }

        // Menu Da Aplicação
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
            if (!Directory.Exists(caminhoDir)) Directory.CreateDirectory(caminhoDir);

            // Verifica se os arquivos de dados e configurações existem
            if (!File.Exists(caminhoArqConf))
            {
                StrmConf = File.Open(caminhoArqConf, FileMode.Create);
                StrmConf.Close();
            }

            if (!File.Exists(caminhoArqDado))
            {
                StrmDado = File.Open(caminhoArqDado, FileMode.Create);
                StrmDado.Close();
            }
            
            // Abre os dois arquivos
            StrmConf = File.Open(caminhoArqConf, FileMode.Open);
            StrmDado = File.Open(caminhoArqDado, FileMode.Open);

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
                    NomeHotel,
                    QuantidadeQuartos,
                    PrecoDiaria,
                    Servico = new ArrayList()
                };

                // Corpo do arquivo de dados
                DadoStruct = new
                {
                    Clientes = new ArrayList()
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
            Stream StrmConf = File.Open(caminhoArqConf, FileMode.Open);

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
                ConteudoConf.NomeHotel,
                ConteudoConf.QuantidadeQuartos,
                ConteudoConf.PrecoDiaria,
                Servico = ListaServico
            };

            StrRe.Close();
            StrmConf.Close();

            StrmConf = File.Open(caminhoArqConf, FileMode.Create);

            StrWr = new StreamWriter(StrmConf);

            StrWr.Write(JsonConvert.SerializeObject(NewConteudoConf));

            StrWr.Close();
            StrmConf.Close();

            Console.WriteLine("Cadastrado com Sucesso!");
            Thread.Sleep(1500);
            Console.Clear();

        }

        /**
        private void AdicionaServicoAoUsuario()
        {
            dynamic ObjDado = this.PegaClientesHotel();
            ArrayList ClientesHotel = new ArrayList();

            List<int> IdClientes = new List<int>();
            int Contador = 0;
            int UsuarioSelecionado;
            bool UsuarioValido = false;

            if (ObjDado.Clientes.Count > 0)
            {
                foreach (var cliente in ObjDado.Clientes)
                {
                    Contador++;
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("ID: " + Contador);
                    Console.WriteLine("NOME: " + cliente.Nome);
                    Console.WriteLine("-------------------------------------");
                    ClientesHotel.Add(cliente);
                    IdClientes[Contador] = Contador;
                }

                Console.WriteLine("\nSelecionar Usuário (ID): ");

                do
                {
                    UsuarioSelecionado = int.Parse(Console.ReadLine());
                    UsuarioValido = Array.Exists(IdClientes, b => b == UsuarioSelecionado );

                } while (UsuarioValido);
                


            }
            else
            {
                Console.WriteLine("Não há Clientes");
            }
        }
        **/ // Função incompleta para adição de serviço solicitado ao cliente

        //Faz o Check-in do cliente
        private void FazerCheckIn()
        {
            Stream StrmDado = File.Open(caminhoArqDado, FileMode.Open);
            StreamReader StrmRd;
            StreamWriter StrmWr;

            dynamic DadoObj;
            dynamic NewDadoObj;
            ArrayList ListDado = new ArrayList();
            ArrayList ServicoInicial = new ArrayList();

            string NomeCliente;
            string DataCheckIn;

            string QuantidadeQuartosStr = PegaConfigHotel().QuantidadeQuartos;
            float PrecoDiaria = PegaConfigHotel().PrecoDiaria;

            StrmRd = new StreamReader(StrmDado);
            DadoObj = JsonConvert.DeserializeObject(StrmRd.ReadToEnd());

            StrmRd.Close();
            StrmDado.Close();

            if (DadoObj.Clientes.Count > 0)
            {
                foreach (var Cliente in DadoObj.Clientes)
                {
                    ListDado.Add(Cliente);
                }
            }

            if (DadoObj.Clientes.Count != int.Parse(QuantidadeQuartosStr))
            {
                ServicoInicial.Add(new {Nome = "CheckIn", Preco = PrecoDiaria});

                Console.Write("Nome do Cliente: ");
                NomeCliente = Console.ReadLine();

                Console.Write("Data de CheckIn (DD/MM/AAAA): ");
                DataCheckIn = Console.ReadLine();

                ListDado.Add( new {Nome = NomeCliente, CheckIn = DataCheckIn, Servicos = ServicoInicial});

                NewDadoObj = new {Clientes = ListDado};


                StrmDado = File.Open(caminhoArqDado, FileMode.Create);

                StrmWr = new StreamWriter(StrmDado);

                StrmWr.Write(JsonConvert.SerializeObject(NewDadoObj));

                StrmWr.Close();
                StrmDado.Close();

            }
            else
            {
                Console.WriteLine("Não há mais vagas");
            }


        }

        //Faz o Check-Out do cliente
        private void FazerCheckOut()
        {
            dynamic ObjConf = PegaConfigHotel();
            dynamic ObjDado = PegaClientesHotel();
            dynamic NewObjDado;
            ArrayList ClientesHotel = new ArrayList();

            List<int> IdClientes = new List<int>();
            int Contador = 0;
            int UsuarioSelecionado;
            bool UsuarioValido;

            string dataCheckIn;
            string dataCheckOut;
            string precoDiariaStr;
            string nomeCliente;
            int diasDecorridos;
            List<dynamic> servicosUt = new List<dynamic>();
            float totalGasto = 0.0F;

            string AuxEnd;

            Stream StrmDado;
            StreamWriter StrmWr;

            string saidaCheckOut;

            if (ObjDado.Clientes.Count > 0)
            {
                foreach (var cliente in ObjDado.Clientes)
                {
                    Contador++;
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("ID: " + Contador);
                    Console.WriteLine("NOME: " + cliente.Nome);
                    Console.WriteLine("-------------------------------------");
                    ClientesHotel.Add(cliente);
                    IdClientes.Add(Contador);
                }

                Console.Write("Selecione um usuário (ID): ");
                do
                {
                    UsuarioSelecionado = int.Parse(Console.ReadLine());
                    UsuarioValido = !Array.Exists(IdClientes.ToArray(), b => b == UsuarioSelecionado);
                    if(UsuarioValido) Console.Write("Usuário inválido, selecione outro (ID): ");
                } while (UsuarioValido);

                dataCheckIn = ObjDado.Clientes[UsuarioSelecionado - 1].CheckIn;
                precoDiariaStr = ObjDado.Clientes[UsuarioSelecionado - 1].Servicos[0].Preco;
                nomeCliente = ObjDado.Clientes[UsuarioSelecionado - 1].Nome;
                
                Console.WriteLine("Cliente Selecionado: {0}\n", nomeCliente);

                Console.Write("Data de CheckOut (DD/MM/AAAA): ");
                dataCheckOut = Console.ReadLine();

                diasDecorridos = CalculaData(dataCheckIn, dataCheckOut);

                foreach (var servico in ObjDado.Clientes[UsuarioSelecionado - 1].Servicos)
                {
                    servicosUt.Add(servico);
                }
                servicosUt.RemoveAt(0);

                Console.Clear();
                saidaCheckOut = "---------------------- " + ObjConf.NomeHotel + " ----------------------\n";
                saidaCheckOut += "Cliente: " + nomeCliente + "\n";
                saidaCheckOut += "Data CheckIn: " + dataCheckIn + " | Data CheckOut: " + dataCheckOut + "\n";
                saidaCheckOut += "Dias no Hotel: " + diasDecorridos + "\n";
                saidaCheckOut += "serviços Utilizados: " + servicosUt.Count + "\n";
                saidaCheckOut += "----------------------------------------------------\n";
                saidaCheckOut += "Nome: Diária do Hotel\n";
                saidaCheckOut += "Perço: R$" + precoDiariaStr + "\n";
                if (servicosUt.Count > 0)
                {
                    saidaCheckOut += "----------------------------------------------------\n";
                    foreach (dynamic servico in servicosUt)
                    {
                        dynamic precoIn = servico.Preco;
                        saidaCheckOut += "Nome: " + servico.Nome + "\n";
                        saidaCheckOut += "Preço: R$" + precoIn + "\n";
                        totalGasto += float.Parse(precoIn.ToString());
                        saidaCheckOut += "----------------------------------------------------\n";
                    }
                    
                }
                else
                {
                    saidaCheckOut += "----------------------------------------------------\n";
                }
                totalGasto += diasDecorridos * float.Parse(precoDiariaStr);
                
                saidaCheckOut += "Total: " + totalGasto + " ---------------------------------\n\n";
                

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(saidaCheckOut);
                Console.ForegroundColor = ConsoleColor.Green;

                Console.Write("Deseja Concluir o CheckOut de {0} (S/N): ", nomeCliente);
                AuxEnd = Console.ReadLine();

                if (AuxEnd == "S" || AuxEnd == "s")
                {
                    ClientesHotel.RemoveAt(UsuarioSelecionado - 1);

                    NewObjDado = new { Clientes = ClientesHotel };

                    StrmDado = File.Create(caminhoArqDado);
                    StrmWr = new StreamWriter(StrmDado);

                    StrmWr.Write(JsonConvert.SerializeObject(NewObjDado));

                    StrmWr.Close();
                    StrmDado.Close();
                    Console.WriteLine("CheckOut Concluido!");
                    Thread.Sleep(1500);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Saiu Checkout");
                }

            }
            else
            {
                Console.WriteLine("Não há clientes para fazer CheckOut");
            }
        }

        public static void Main(string[] args)
        {
            Programa Prog = new Programa();

            Console.Title = "Hotel Lógica";
            Console.ForegroundColor = ConsoleColor.Green;

            Prog.caminhoDir = Path.GetFullPath(Prog.dirNome);
            Prog.caminhoArqConf = Path.Combine(Prog.caminhoDir, Prog.arqConfNome);
            Prog.caminhoArqDado = Path.Combine(Prog.caminhoDir, Prog.arqDadoNome);

            // Menu();

            Prog.ConfiguraPrograma();
            Prog.FazerCheckOut();
            // Prog.CriaServico();
            // Prog.FazerCheckIn();

            Console.ReadKey();
        }
    }
}
