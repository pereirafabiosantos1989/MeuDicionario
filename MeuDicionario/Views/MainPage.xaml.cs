using MeuDicionario.Enum;
using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using MeuDicionario.ViewModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MeuDicionario
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        /* As palavras e sua tradução */
        List<Dicionario> dicionario;
        List<string> resultadoPesquisa = new List<string>();

        private SQLiteAsyncConnection _contexto;

        List<string> Idiomas;

        public MainPage()
        {
            InitializeComponent();

            Idiomas = new List<string>()
            {
                "Alemão",
                "Francês",
                "Grego",
                "Inglês"
            };

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();

            idiomaParaPesquisa.BindingContext = Idiomas;
            idiomaSelecionado.BindingContext = Idiomas;

            labelNenhumResultado.IsVisible = false;

            /* inicializa os pickers */
            idiomaSelecionado.SelectedIndex = 0;
            idiomaParaPesquisa.SelectedIndex = 0;
        }

        public void LimparCampos()
        {
            txtPalavra.Text =
            txtTraducao.Text = string.Empty;
        }

        public async void GravarNovaTraducao(string palavra)
        {
            Dicionario novoItem = new Dicionario();
            novoItem.Palavra = txtPalavra.Text;
            novoItem.Traducao = txtTraducao.Text;
            novoItem.Idioma = idiomaSelecionado.SelectedItem.ToString();

            await _contexto.InsertAsync(novoItem);
            await DisplayAlert("Dicionário", $"A palavra {palavra.ToUpper()} foi adicionada ao dicionário.", "OK");
            LimparCampos();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            /* cadastrar termo */
            string palavra = txtPalavra.Text;
            string traducao = txtTraducao.Text;
            string idioma = idiomaSelecionado.SelectedItem.ToString();

            if (string.IsNullOrEmpty(palavra) || string.IsNullOrEmpty(traducao))
            {
                await DisplayAlert("Dicionário", "A palavra e a tradução são obrigatórias.", "Voltar");
                return;
            }

            GravarNovaTraducao(palavra);
        }

        protected async override void OnAppearing()
        {
            /* cria as tabelas se ainda não existirem */
            await _contexto.CreateTableAsync<Dicionario>();
            await _contexto.CreateTableAsync<Licao>();

            /* lista os dados do dicionário */
            dicionario = await _contexto.Table<Dicionario>().ToListAsync();

            base.OnAppearing();

            labelNenhumResultado.IsVisible = false;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ListarItens();
        }

        /// <summary>
        /// Lista todos os itens
        /// </summary>
        public async void ListarItens()
        {
            dicionario = await _contexto.Table<Dicionario>()
                                        .OrderBy(x => x.Idioma)
                                        .ToListAsync();

            listDicionario.ItemsSource = dicionario;

            labelNenhumResultado.IsVisible = false;
        }

        public async void PesquisarTermo()
        {
            if (!checkFiltroPorIdioma.IsChecked)
            {
                /* Pesquisa por palavra ou tradução (sem filtro de idioma) */
                dicionario = await _contexto.Table<Dicionario>()
                                             .Where(x => x.Palavra.Contains(txtPesquisa.Text) || x.Traducao.Contains(txtPesquisa.Text))
                                             .OrderBy(x => x.Idioma)
                                             .ToListAsync();
            }
            else
            {
                string idiomaSelecionado = idiomaParaPesquisa.SelectedItem.ToString();

                /* Pesquisa pelo idioma */
                dicionario = await _contexto.Table<Dicionario>()
                                             .Where(x => (x.Palavra.Contains(txtPesquisa.Text) || x.Traducao.Contains(txtPesquisa.Text)) && x.Idioma.Equals(idiomaSelecionado))
                                             .OrderBy(x => x.Palavra)
                                             .ToListAsync();
            }

            listDicionario.ItemsSource = dicionario;

            if (dicionario.Count == 0)
            {
                labelNenhumResultado.IsVisible = true;
            }
            else
            {
                labelNenhumResultado.IsVisible = false;
            }
        }

        private void ListDicionario_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem) sender;
            
            Dicionario itemSelecionado = item.CommandParameter as Dicionario;

            bool apagarItem = await DisplayAlert("Apagar item", $"Deseja apagar o item '{itemSelecionado.Palavra}'?", "Sim", "Não");

            if (apagarItem)
            {
                await _contexto.DeleteAsync(itemSelecionado);
            }

            ListarItens();
        }

        private void ListDicionario_Refreshing(object sender, EventArgs e)
        {
            AtualizarListaExistente();
            listDicionario.IsRefreshing = false;
        }

        /// <summary>
        /// Atualiza a lista que já está preenchida
        /// </summary>
        public async void AtualizarListaExistente()
        {
            string idiomaSelecionado = idiomaParaPesquisa.SelectedItem.ToString();

            /* atualiza a lista com o idioma selecionado */
            if (checkFiltroPorIdioma.IsChecked)
            {
                dicionario = await _contexto.Table<Dicionario>()
                                             .Where(x => x.Idioma == idiomaSelecionado && (x.Palavra.Contains(txtPesquisa.Text) || x.Traducao.Contains(txtPesquisa.Text)))
                                             .OrderBy(x => x.Idioma)
                                             .ToListAsync();

                listDicionario.ItemsSource = dicionario;
            }
            else
            {
                dicionario = await _contexto.Table<Dicionario>()
                                             .Where(x => x.Palavra.Contains(txtPesquisa.Text) || x.Traducao.Contains(txtPesquisa.Text))
                                             .OrderBy(x => x.Idioma)
                                             .ToListAsync();

                listDicionario.ItemsSource = dicionario;
            }

            labelNenhumResultado.IsVisible = dicionario.Count == 0; 
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (checkFiltroPorIdioma.IsChecked)
            {
                idiomaParaPesquisa.IsVisible = true;
            }
            else
            {
                idiomaParaPesquisa.IsVisible = false;
            }
        }

        private void TxtPesquisa_TextChanged(object sender, TextChangedEventArgs e)
        {
            PesquisarTermo();
        }
    }
}
