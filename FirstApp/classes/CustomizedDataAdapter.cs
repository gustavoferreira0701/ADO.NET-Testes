using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApp.classes
{
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
