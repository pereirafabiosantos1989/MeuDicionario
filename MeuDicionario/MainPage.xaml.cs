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

        public MainPage()
        {
            InitializeComponent();

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();

            labelNenhumResultado.IsVisible = false;

            /* inicializa os pickers */
            idiomaSelecionado.SelectedIndex = 0;
            idiomaParaPesquisa.SelectedIndex = 0;
        }

        public void LimparCampos()
        {
            txtPalavra.Text =
            txtTraducao.Text = string.Empty;
            idiomaSelecionado.SelectedIndex = 0;
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

        public async void ListarItens()
        {
            dicionario = await _contexto.Table<Dicionario>()
                                        .OrderBy(x => x.Idioma)
                                        .ToListAsync();

            listDicionario.ItemsSource = dicionario;

            labelNenhumResultado.IsVisible = false;
        }

        private async void TxtPesquisa_SearchButtonPressed(object sender, EventArgs e)
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

                if (idiomaParaPesquisa.SelectedIndex == 0)
                {
                    await DisplayAlert("Dicionário", "É necessário informar um idioma para o filtro de resultado.", "Cancelar");
                    return;
                }
                else
                {

                    /* Pesquisa pelo idioma */

                    dicionario = await _contexto.Table<Dicionario>()
                                                 .Where(x => (x.Palavra.Contains(txtPesquisa.Text) || x.Traducao.Contains(txtPesquisa.Text)) && x.Idioma.Equals(idiomaSelecionado))
                                                 .OrderBy(x => x.Palavra)
                                                 .ToListAsync();
                }
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
            ListarItens();
            listDicionario.IsRefreshing = false;
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
    }
}
