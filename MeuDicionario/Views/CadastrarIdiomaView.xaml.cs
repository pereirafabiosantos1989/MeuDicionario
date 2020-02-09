using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarIdiomaView : ContentPage
    {
        private SQLiteAsyncConnection _contexto;

        public CadastrarIdiomaView()
        {
            InitializeComponent();

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
            _contexto.CreateTableAsync<Idioma>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (txtIdioma.Text != null && txtIdioma.Text.Length > 0)
            {
                Idioma novoIdioma = new Idioma()
                {
                    Nome = txtIdioma.Text
                };

                await _contexto.InsertAsync(novoIdioma);
                await DisplayAlert("Cadastro de idioma", $"O idioma '{novoIdioma.Nome}' foi cadastrado.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Cadastro de idioma", "É necessário informar o nome do idioma", "OK");
                return;
            }
        }
    }
}