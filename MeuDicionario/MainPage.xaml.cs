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
    public partial class MainPage : CarouselPage
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
        }

        public void LimparCampos()
        {
            txtPalavra.Text =
            txtTraducao.Text = string.Empty;
            idiomaSelecionado.SelectedIndex = -1;
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
            /* cria a tabela */
            await _contexto.CreateTableAsync<Dicionario>();

            /* lista os dados do dicionário */
            dicionario = await _contexto.Table<Dicionario>().ToListAsync();

            base.OnAppearing();

            labelNenhumResultado.IsVisible = false;
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            dicionario = await _contexto.Table<Dicionario>().ToListAsync();
            listDicionario.ItemsSource = dicionario;

            labelNenhumResultado.IsVisible = false;
        }

        private async void TxtPesquisa_SearchButtonPressed(object sender, EventArgs e)
        {
            dicionario = await _contexto.Table<Dicionario>().Where(x => x.Palavra.Contains(txtPesquisa.Text)).ToListAsync();
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
            Editar editar = new Editar(e.SelectedItem as Dicionario);
            this.Navigation.PushModalAsync(editar);
        }
    }
}
