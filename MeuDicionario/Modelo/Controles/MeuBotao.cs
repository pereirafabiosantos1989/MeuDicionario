using MeuDicionario.Enums;
using Xamarin.Forms;

namespace MeuDicionario.Modelo.Controles
{
    public class MeuBotao : Button
    {
        public static readonly BindableProperty IconeProperty = BindableProperty.Create(nameof(IconeProperty), typeof(Icones), typeof(MeuBotao), null);

        public Icones Icone
        {
            get { return (Icones) GetValue(IconeProperty); }
            set { SetValue(IconeProperty, value); }
        }
    }
}
