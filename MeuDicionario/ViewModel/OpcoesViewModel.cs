using MeuDicionario.Modelo;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeuDicionario.ViewModel
{
    public class OpcoesViewModel : BasePropertyChanged
    {
        private List<Opcao> itensMenu;

        public List<Opcao> ItensMenu
        {
            get { return itensMenu; }
            set
            {
                itensMenu = value;
                OnPropertyChanged();
            }
        }

        private Opcao opcaoSelecionada;

        public Opcao OpcaoSelecionada
        {
            get { return opcaoSelecionada; }
            set 
            { 
                opcaoSelecionada = null;
                OnPropertyChanged();

                MessagingCenter.Send(value, "NavegarParaPagina");
            }
        }

        public OpcoesViewModel()
        {
            ItensMenu = new List<Opcao>()
            {
                new Opcao(){ TituloOpcao = "Cadastrar idioma", CorItem = System.Drawing.Color.Green },
                new Opcao(){ TituloOpcao = "Cadastrar termo", CorItem = System.Drawing.Color.Blue },
                new Opcao(){ TituloOpcao = "Pesquisar termo", CorItem = System.Drawing.Color.Purple }
            };
        }
    }
}
