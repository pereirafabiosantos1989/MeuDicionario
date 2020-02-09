using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarTermoView : ContentPage
    {
        private SQLiteAsyncConnection _contexto;

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

        public CadastrarTermoView()
        {
            InitializeComponent();

            this.BindingContext = this;

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
            _contexto.CreateTableAsync<Dicionario>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                string idiomaSelecionado = pickerIdioma.SelectedItem == null ? string.Empty : (string)pickerIdioma.SelectedItem;

                if (txtTermo.Text == null || txtTraducao.Text == null ||
                    txtTermo.Text.Equals(string.Empty) || txtTraducao.Text.Equals(string.Empty) || 
                    idiomaSelecionado.Equals(string.Empty))
                {
                    await DisplayAlert("Cadastro", "O termo a tradução e o idioma são obrigatórios", "OK");
                    return;
                }

                Dicionario item = new Dicionario()
                {
                    Palavra = txtTermo.Text,
                    Traducao = txtTraducao.Text,
                    Idioma = idiomaSelecionado
                };

                await _contexto.InsertAsync(item);

                await DisplayAlert("Cadastro", $"O termo {item.Palavra} foi cadastrado corretamente.", "OK");
                await Navigation.PopToRootAsync();
            }
            catch (Exception e1)
            {
                await DisplayAlert("Erro", e1.Message, "OK");
            }
        }
    }
}