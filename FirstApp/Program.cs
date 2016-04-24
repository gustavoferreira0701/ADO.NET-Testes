using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;
using System.ComponentModel;

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

    public class AdoTest
    {
        private string stringConnection = string.Empty;

        private CustomizedDataAdapter adapter;
        
        public AdoTest(string conexao)
        {
            this.stringConnection = conexao;
            this.adapter = new CustomizedDataAdapter();
        }

        public void ExibeResultadoDeConsultaAoBanco<T>(List<T> resultados, string query)
        {
            using (var conexao = new SqlConnection(stringConnection))
            {
                conexao.Open();

                var instancia = Activator.CreateInstance<T>();

                SqlCommand command = new SqlCommand(query, conexao);

                var reader = command.ExecuteReader();

                adapter.ProcessarRespostaBanco(ref reader, instancia);

                resultados.Add(instancia);
            }
        }

        public void ExecutaEscritaNoBanco()
        {
            using (var conexao = new SqlConnection(stringConnection))
            {
                conexao.Open();

                string sql = "insert into Serie (Nome, DataInicio, DataTermino, DataAtualizacao, Peso) values (@Nome, @DataInicio, @DataTermino, @DataAtualizacao, @Peso)";

                var parameters = new SqlParameter[5] {
                    new SqlParameter("@Nome","Serie B"),
                    new SqlParameter("@DataInicio",DateTime.Now),
                    new SqlParameter("@DataTermino",DateTime.Now),
                    new SqlParameter("@DataAtualizacao",DateTime.Now),
                    new SqlParameter("@Peso", 90.3)
                };

                SqlCommand command = new SqlCommand(sql, conexao);
                command.Parameters.AddRange(parameters);

                command.ExecuteNonQuery();

                Console.WriteLine("Linha adiconada");
            }
        }

        public void RemoverRegistrosDaBase()
        {
            using (var conexao = new SqlConnection(stringConnection))
            {
                conexao.Open();

                string sql = "DELETE FROM Serie WHERE Peso > 95.0";

                SqlCommand command = new SqlCommand(sql, conexao);

                command.ExecuteNonQuery();

                Console.WriteLine("Linha Removida");
            }
        }

        public void AtualizaRegistroNaBase()
        {
            using (var conexao = new SqlConnection(stringConnection))
            {
                var sql = "UPDATE SERIE SET Peso += 1 WHERE PESO > 80.4";

            }
        }

    }


    public class Serie
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public decimal Peso { get; set; }
    }




    public class CustomizedDataAdapter
    {
        private class ConfiguracaoColuna
        {
            public Type TipoDado { get; set; }
            public string NomeClasse { get; set; }
            public string NomeColuna { get; set; }
            public object ValorColuna { get; set; }
        }

        public void ProcessarRespostaBanco<T>(ref SqlDataReader reader, T objeto)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var count = reader.FieldCount;
                    var index = 0;

                    while (index < count)
                    {
                        PreencherObjeto(new ConfiguracaoColuna
                        {
                            NomeClasse = objeto.GetType().Name,
                            NomeColuna = reader.GetName(index),
                            TipoDado = reader.GetFieldType(index),
                            ValorColuna = reader.GetValue(index)
                        }, objeto);

                        index++;
                    }
                }
            }
            else
                Console.Write("Nenhuma linha encontrada");
        }

        private void PreencherObjeto<T>(ConfiguracaoColuna coluna, T objetoDestino)
        {
            var property = objetoDestino.GetType().GetProperty(coluna.NomeColuna);

            if (property != null)
                property.SetValue(objetoDestino, coluna.ValorColuna);
        }
    }



}
