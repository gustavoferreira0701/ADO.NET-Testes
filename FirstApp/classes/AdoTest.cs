using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp.classes
{
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
}
