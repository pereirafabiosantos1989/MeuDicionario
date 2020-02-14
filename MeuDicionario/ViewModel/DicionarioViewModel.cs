using MeuDicionario.Modelo;
using System.Collections.Generic;

namespace MeuDicionario.ViewModel
{
    public class DicionarioViewModel : BasePropertyChange
    {
        public List<Dicionario> dicionario = new List<Dicionario>();

        public DicionarioViewModel()
        {
            
        }
    }
}
