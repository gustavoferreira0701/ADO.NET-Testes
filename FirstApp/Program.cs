using System;
using System.Collections.Generic;
using FirstApp.classes;

namespace FirstApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var adoTest = new AdoTest(@"Data Source=(localdb)\v11.0;Integrated Security=True;Database=GymApp");

            adoTest.ExibeResultadoDeConsultaAoBanco(new List<Serie>(), "SELECT * FROM Serie");

            adoTest.ExecutaEscritaNoBanco();

            adoTest.RemoverRegistrosDaBase();

            Console.ReadKey();
        }
    }
    
}
