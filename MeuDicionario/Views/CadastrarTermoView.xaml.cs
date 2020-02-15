using MeuDicionario.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarTermoView : ContentPage
    {
        public CadastrarTermoView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Exception>(this, "ErroAoCadastrarTermo",
            async (erro) =>
            {
                await DisplayAlert("Meu dicionário", erro.Message, "OK");
            });

            MessagingCenter.Subscribe<CadastrarTermoViewModel>(this, "TermoCadastrado",
            async (termo) =>
            {
                await DisplayAlert("Meu dicionário", $"O termo '{termo.Termo}' foi cadastrado corretamente.", "OK");
                await Navigation.PopToRootAsync();
            });

            MessagingCenter.Subscribe<CadastrarTermoViewModel>(this, "PreencherCampos",
            async (termo) =>
            {
                await DisplayAlert("Meu dicionário", "O termo, a tradução e o idioma são obrigatórios", "OK");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Exception>(this, "ErroAoCadastrarTermo");
            MessagingCenter.Unsubscribe<CadastrarTermoViewModel>(this, "TermoCadastrado");
            MessagingCenter.Unsubscribe<CadastrarTermoViewModel>(this, "PreencherCampos");
        }
    }
}