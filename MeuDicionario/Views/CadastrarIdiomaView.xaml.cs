using MeuDicionario.Modelo;
using MeuDicionario.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarIdiomaView : ContentPage
    {
        public CadastrarIdiomaView()
        {
            InitializeComponent();

            this.BindingContext = new CadastrarIdiomaViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            /* se o idioma foi cadastrado fecha a tela */
            MessagingCenter.Subscribe<Idioma>(this, "SucessoCadastrarNovoIdioma", 
            async (msg) =>
            {
                await DisplayAlert("Meu dicionário", $"O idioma {msg.Nome} foi cadastrado corretamente", "OK");
                await Navigation.PopAsync();
            });

            MessagingCenter.Subscribe<Exception>(this, "ErroCadastrarNovoIdioma",
            async (ex) =>
            {
                await DisplayAlert("Meu dicionário", ex.Message, "OK"); 
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Idioma>(this, "SucessoCadastrarNovoIdioma");
            MessagingCenter.Unsubscribe<Exception>(this, "ErroCadastrarNovoIdioma");
        }
    }
}