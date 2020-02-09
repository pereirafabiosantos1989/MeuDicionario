
using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarTermoView : ContentPage
    {
        private SQLiteAsyncConnection _contexto;

        private Dicionario _termoAEditar;

        public Dicionario TermoAEditar 
        { 
            get
            {
                return _termoAEditar;
            }

            set
            {
                _termoAEditar = value;
                OnPropertyChanged();
            }
        }

        private string _tituloPagina;

        public string TituloPagina
        {
            get
            {
                return _tituloPagina;
            }

            set
            {
                _tituloPagina = value;
                OnPropertyChanged();
            }
        }

        public List<string> Idiomas
        {
            get
            {
                return new List<string>()
                {
                    "Alemão",
                    "Francês",
                    "Grego",
                    "Inglês"
                };
            }
        }

        private Dicionario _idiomaDoTermoAtual;

        public Dicionario IdiomaDoTermoAtual
        {
            get { return _idiomaDoTermoAtual; }
            set 
            { 
                _idiomaDoTermoAtual = value;
                OnPropertyChanged();
            }
        }

        public EditarTermoView(Dicionario termo)
        {
            InitializeComponent();

            this.BindingContext = this;

            TermoAEditar = termo;
            IdiomaDoTermoAtual = termo;
            TituloPagina = $"Termo: {TermoAEditar.Palavra}";

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
        }

        /// <summary>
        /// Apaga o termo selecionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            string resposta = await DisplayActionSheet($"Apagar o termo '{TermoAEditar.Palavra}'?", "Cancelar", "Apagar");

            if (resposta.Equals("Apagar"))
            {
                await _contexto.DeleteAsync(TermoAEditar);
                await Navigation.PopAsync();
            }
        }

        private async void Button_Clicked_1(object sender, System.EventArgs e)
        {
            string idiomaAtualizado = (string) pickerIdiomas.SelectedItem;

            TermoAEditar.Palavra = txtPalavra.Text;
            TermoAEditar.Traducao = txtTraducao.Text;
            TermoAEditar.Idioma = idiomaAtualizado;

            await _contexto.UpdateAsync(TermoAEditar);
            await Navigation.PopAsync();
        }
    }
}