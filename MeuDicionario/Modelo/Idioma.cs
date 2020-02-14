using SQLite;

namespace MeuDicionario.Modelo
{
    public class Idioma
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Nome { get; set; }

    }
}
