using MeuDicionario.Modelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PesquisarTermoView : ContentPage
    {
        public PesquisarTermoView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Dicionario>(this, "TermoFoiSelecionado",
            (termo) =>
            {
                Navigation.PushAsync(new EditarTermoView(termo));
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Dicionario>(this, "TermoFoiSelecionado");
        }
    }
}