using MeuDicionario.Modelo;
using MeuDicionario.Modelo.Controles;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    public partial class PesquisarTermoView : MeuView 
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

            MessagingCenter.Subscribe<Mensagem>(this, "BotaoListViewPressionado",
            async (valor) =>
            {
                await DisplayActionSheet("Nova ação", "Cancelar", null, "Dashboard", "Cadastro de idioma", "Cadastro de termo");
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Dicionario>(this, "TermoFoiSelecionado");
            MessagingCenter.Unsubscribe<Mensagem>(this, "BotaoListViewPressionado");
        }
    }
}