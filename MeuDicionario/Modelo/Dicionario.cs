using MeuDicionario.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeuDicionario.Modelo
{
    public class Dicionario
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Palavra { get; set; }
        public string Traducao { get; set; }
        public string Idioma { get; set; }
    }
}
