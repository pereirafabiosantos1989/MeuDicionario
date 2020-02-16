using MeuDicionario.Modelo;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Opcao>(this, "NavegarParaPagina",
            (opcao) =>
            {
                if (opcao.TituloOpcao.Equals("Cadastrar idioma"))
                {
                    Navigation.PushAsync(new CadastrarIdiomaView());
                }
                else if (opcao.TituloOpcao.Equals("Cadastrar termo"))
                {
                    Navigation.PushAsync(new CadastrarTermoView());
                }
                else if (opcao.TituloOpcao.Equals("Pesquisar termo"))
                {
                    Navigation.PushAsync(new PesquisarTermoView());
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Opcao>(this, "NavegarParaPagina");
        }
    }
}