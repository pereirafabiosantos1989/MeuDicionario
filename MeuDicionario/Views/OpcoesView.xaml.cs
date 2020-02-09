using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpcoesView : ContentPage
    {
        public OpcoesView()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CadastrarTermoView());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PesquisarTermoView());
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CadastrarIdiomaView());
        }
    }
}