
using MeuDicionario.Modelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeuDicionario.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : Shell
    {
        public HomeView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //MessagingCenter.Subscribe<Opcao>(this, "NavegarParaPagina",
            //(opcao) =>
            //{
            //    if (opcao.TituloOpcao.Equals("Cadastrar idioma"))
            //    {
            //        Shell.Current.Navigation.PushAsync(new CadastrarIdiomaView());
            //    }
            //    else if (opcao.TituloOpcao.Equals("Cadastrar termo"))
            //    {
            //        Shell.Current.Navigation.PushAsync(new CadastrarTermoView());
            //    }
            //    else if (opcao.TituloOpcao.Equals("Pesquisar termo"))
            //    {
            //        Shell.Current.Navigation.PushAsync(new PesquisarTermoView());
            //    }
            //});
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<Opcao>(this, "NavegarParaPagina");
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
        }
    }
}