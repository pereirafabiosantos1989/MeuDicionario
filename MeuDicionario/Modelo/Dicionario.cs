using MeuDicionario.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeuDicionario.Modelo
{
    public class Dicionario
    {
        public string Palavra { get; set; }
        public string Traducao { get; set; }
        public EnumIdiomas Idioma { get; set; }
    }
}
