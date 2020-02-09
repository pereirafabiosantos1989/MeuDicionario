using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarTermoView : ContentPage
    {
        private SQLiteAsyncConnection _contexto;

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

        public CadastrarTermoView()
        {
            InitializeComponent();

            this.BindingContext = this;

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
            _contexto.CreateTableAsync<Dicionario>();

            LerIdiomasCadastrados();
        }

        public async void LerIdiomasCadastrados()
        {
            Idiomas = await _contexto.Table<Idioma>().ToListAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Idioma idiomaSelecionado = pickerIdioma.SelectedItem == null ? null : (Idioma)pickerIdioma.SelectedItem;

                if (txtTermo.Text == null || txtTraducao.Text == null ||
                    txtTermo.Text.Equals(string.Empty) || txtTraducao.Text.Equals(string.Empty) || 
                    idiomaSelecionado == null)
                {
                    await DisplayAlert("Cadastro", "O termo, a tradução e o idioma são obrigatórios", "OK");
                    return;
                }

                Dicionario item = new Dicionario()
                {
                    Palavra = txtTermo.Text,
                    Traducao = txtTraducao.Text,
                    Idioma = idiomaSelecionado.Nome
                };

                await _contexto.InsertAsync(item);

                await DisplayAlert("Cadastro", $"O termo '{item.Palavra}' foi cadastrado corretamente.", "OK");
                await Navigation.PopToRootAsync();
            }
            catch (Exception e1)
            {
                await DisplayAlert("Erro", e1.Message, "OK");
            }
        }
    }
}