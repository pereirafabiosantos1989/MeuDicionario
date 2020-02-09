
using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System.Collections.Generic;
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

        private Idioma _idiomaDoTermoAtual;

        public Idioma IdiomaDoTermoAtual
        {
            get { return _idiomaDoTermoAtual; }
            set 
            { 
                _idiomaDoTermoAtual = value;
                OnPropertyChanged();
            }
        }

        private List<Idioma> _idiomas;

        public List<Idioma> Idiomas
        {
            get
            {
                return _idiomas;
            }
            set
            {
                _idiomas = value;
                OnPropertyChanged();
            }
        }

        public EditarTermoView(Dicionario termo)
        {
            InitializeComponent();

            this.BindingContext = this;

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
            
            LerIdiomasCadastrados();

            TermoAEditar = termo;
            IdiomaDoTermoAtual = new Idioma() { Nome = termo.Idioma };
            TituloPagina = $"Termo: {TermoAEditar.Palavra}";
        }

        public async void LerIdiomasCadastrados()
        {
            Idiomas = await _contexto.Table<Idioma>().ToListAsync();
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
            Idioma idiomaAtualizado = (Idioma) pickerIdiomas.SelectedItem;

            TermoAEditar.Palavra = txtPalavra.Text;
            TermoAEditar.Traducao = txtTraducao.Text;
            TermoAEditar.Idioma = idiomaAtualizado.Nome;

            await _contexto.UpdateAsync(TermoAEditar);
            await Navigation.PopAsync();
        }
    }
}