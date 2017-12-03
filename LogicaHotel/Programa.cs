using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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


            if (!Directory.Exists(caminhoDir)) Directory.CreateDirectory(caminhoDir);

            if (!File.Exists(caminhoArqConf)) File.Create(caminhoArqConf);
            if (!File.Exists(caminhoArqDado)) File.Create(caminhoArqDado);

            StrmConf = File.Open(caminhoArqConf, FileMode.Open);
            StrmDado = File.Open(caminhoArqDado, FileMode.Open);


            Escritor = new StreamWriter(StrmConf);

            
            Escritor.WriteLine(Console.ReadLine());

            Escritor.Close();

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
