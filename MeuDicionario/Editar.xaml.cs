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

namespace MeuDicionario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Editar : ContentPage
    {
        SQLiteAsyncConnection _contexto;
        Dicionario termo = new Dicionario();

        public Editar()
        {
            InitializeComponent();
        }

        public Editar(Dicionario dic)
        {
            InitializeComponent();

            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
            termo = dic;
            lblTermo.Text = $" {termo.Palavra}?";
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string resposta = await DisplayActionSheet($"Apagar o termo {termo.Palavra}?", "Não", "Sim");

            if (resposta.Equals("Sim"))
            {
                await _contexto.DeleteAsync(termo);
            }

            await this.Navigation.PopModalAsync();
        }
    }
}