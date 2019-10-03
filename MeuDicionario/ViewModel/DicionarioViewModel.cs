using MeuDicionario.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeuDicionario.ViewModel
{
    public class DicionarioViewModel
    {
        List<Dicionario> dicionario;

        public DicionarioViewModel()
        {
            dicionario = new List<Dicionario>();
        }
    }
}
