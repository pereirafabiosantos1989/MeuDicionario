using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PesquisarTermoView : ContentPage
    {
        private bool _habilitaFiltroIdioma;
        private SQLiteAsyncConnection _contexto;

        private List<Dicionario> _termosEncontrados;
        private bool _erroNaPesquisaPorIdioma;

        private bool _listaAtualizada;

        public bool ListaAtualizada
        {
            get { return _listaAtualizada; }
            set 
            { 
                _listaAtualizada = !value;
                OnPropertyChanged();
            }
        }

        public bool ErroNaPesquisaPorIdioma
        {
            get { return _erroNaPesquisaPorIdioma; }
            set 
            { 
                _erroNaPesquisaPorIdioma = value;
                OnPropertyChanged();
            }
        }

        private bool _temResultado;

        public bool TemResultado
        {
            get { return _temResultado; }
            set 
            { 
                _temResultado = value;
                OnPropertyChanged();
            }
        }

        public List<Dicionario> TermosEncontrados
        {
            get { return _termosEncontrados; }
            set 
            { 
                _termosEncontrados = value;
                OnPropertyChanged();
            }
        }

        public bool HabilitaFiltroIdioma 
        {
            get
            {
                return _habilitaFiltroIdioma;
            }

            set
            {
                _habilitaFiltroIdioma = value;
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

        public PesquisarTermoView()
        {
            InitializeComponent();

            this.BindingContext = this;
            _contexto = DependencyService.Get<IConexao>().RetornaConexao();

            LerIdiomasCadastrados();
            PesquisarTermos();
        }

        public async void LerIdiomasCadastrados()
        {
            Idiomas = await _contexto.Table<Idioma>().ToListAsync();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (switchFiltroIdioma.IsToggled)
            {
                HabilitaFiltroIdioma = true;

                Idioma idiomaSelecionado = (Idioma) pickerIdioma.SelectedItem;
                PesquisarTermos(idiomaSelecionado.Nome);
            }
            else
            {
                HabilitaFiltroIdioma = false;
                PesquisarTermos();
            }
        }

        public async void PesquisarTermos(string idioma = null, bool pesquisarTodosOsTermos = false)
        {
            string termoDePesquisa = txtTermo.Text == null ? string.Empty : txtTermo.Text;

            if (pesquisarTodosOsTermos)
            {
                /* é para pesquisar todos os termos existentes independente do termo informado no campo de pesquisa */
                ErroNaPesquisaPorIdioma = false;

                TermosEncontrados = await _contexto.Table<Dicionario>().ToListAsync();
            }
            else if (idioma == null && HabilitaFiltroIdioma)
            {
                /* é para pesquisar por idioma mas nenhum idioma foi selecionado na lista */
                ErroNaPesquisaPorIdioma = true;

                TermosEncontrados = null;
            }
            else if (HabilitaFiltroIdioma)
            {
                /* efetuar a pesquisa de acordo com o idioma */
                ErroNaPesquisaPorIdioma = false;

                TermosEncontrados = await _contexto.Table<Dicionario>()
                                    .Where(x => (x.Palavra.Contains(termoDePesquisa) || x.Traducao.Contains(termoDePesquisa)) &&
                                    x.Idioma == idioma).ToListAsync();
            }
            else
            {
                /* efetuar a pesquisa apenas pelo termo */
                ErroNaPesquisaPorIdioma = false;

                TermosEncontrados = await _contexto.Table<Dicionario>()
                                    .Where(x => x.Palavra.Contains(termoDePesquisa) || x.Traducao.Contains(termoDePesquisa)).ToListAsync();
            }
        }

        private void txtTermo_TextChanged(object sender, TextChangedEventArgs e)
        {
            Idioma idiomaSelecionado = (Idioma) pickerIdioma.SelectedItem;
            PesquisarTermos(idiomaSelecionado.Nome);
        }

        private void pickerIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            Idioma idiomaSelecionado = (Idioma) pickerIdioma.SelectedItem;
            PesquisarTermos(idiomaSelecionado.Nome);
        }

        private void ListView_Refreshing(object sender, EventArgs e)
        {
            if (HabilitaFiltroIdioma)
            {
                Idioma idiomaSelecionado = (Idioma) pickerIdioma.SelectedItem;   
                PesquisarTermos(idiomaSelecionado.Nome);
            }
            else
            {
                PesquisarTermos();
            }

            ListaAtualizada = true;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Dicionario termoParaEditar = (Dicionario) e.SelectedItem;
            Navigation.PushAsync(new EditarTermoView(termoParaEditar));
        }
            
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            PesquisarTermos(null, true);
        }
    }
}