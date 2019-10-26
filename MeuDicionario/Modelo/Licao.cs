using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MeuDicionario.Modelo
{
    public class Licao
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
    }
}
