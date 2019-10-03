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
        List<Dicionario> dicionario = new List<Dicionario>();
        List<string> resultadoPesquisa = new List<string>();

        private SQLiteAsyncConnection _contexto;

        public MainPage()
        {
            InitializeComponent();

            listDicionario.ItemsSource = dicionario;

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
        }

        public void LimparCampos()
        {
            txtPalavra.Text =
            txtTraducao.Text = string.Empty;
        }

        public async void GravarNovaTraducao()
        {
            Dicionario novoItem = new Dicionario();
            novoItem.Palavra = txtPalavra.Text;
            novoItem.Traducao = txtTraducao.Text;
            novoItem.Idioma = EnumIdiomas.Alemao;

            await _contexto.InsertAsync(novoItem);
            await DisplayAlert("Dicionário", $"A palavra {palavra.ToUpper()} foi adicionada ao dicionário.", "OK");
            LimparCampos();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string palavra = txtPalavra.Text;
            string traducao = txtTraducao.Text;

            if (string.IsNullOrEmpty(palavra) || string.IsNullOrEmpty(traducao))
            {
                await DisplayAlert("Dicionário", "A palavra e a tradução são obrigatórias.", "Voltar");
                return;
            }

            GravarNovaTraducao();
        }

        protected override void OnAppearing()
        {
            /* cria a tabela */
            _contexto.CreateTableAsync<Dicionario>();

            /* lista os dados do dicionário */
            var dados = _contexto.Table<Dicionario>().ToListAsync();

            base.OnAppearing();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            listDicionario.ItemsSource = resultadoPesquisa;
        }

        private void TxtPesquisa_SearchButtonPressed(object sender, EventArgs e)
        {
            List<string> exibir = new List<string>();

            try
            {
                Dicionario resultado = dicionario.Where(x => x.Palavra.Contains(txtPesquisa.Text)).First();

                exibir.Add(resultado.Traducao);
            }
            catch (InvalidOperationException)
            {
                exibir.Add("Nenhuma tradução encontrada");
            }

            listDicionario.ItemsSource = exibir;
        }
    }
}
