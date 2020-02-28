
using MeuDicionario.Modelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : MasterDetailPage
    {
        public HomeView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Opcao>(this, "NavegarParaPagina",
            (opcao) =>
            {
                this.IsPresented = false;
                
                if (opcao.TituloOpcao.Equals("Cadastrar idioma"))
                {
                    this.Detail = new NavigationPage(new CadastrarIdiomaView());
                }
                else if (opcao.TituloOpcao.Equals("Cadastrar termo"))
                {
                    this.Detail = new NavigationPage(new CadastrarTermoView());
                }
                else if (opcao.TituloOpcao.Equals("Pesquisar termo"))
                {
                    this.Detail = new NavigationPage(new PesquisarTermoView());
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