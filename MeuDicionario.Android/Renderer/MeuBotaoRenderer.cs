using Android.Content;
using MeuDicionario.Droid.Renderer;
using MeuDicionario.Modelo.Controles;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MeuBotao), typeof(MeuBotaoRenderer))]
namespace MeuDicionario.Droid.Renderer
{
    public class MeuBotaoRenderer : ButtonRenderer
    {
        Context c;

        public MeuBotaoRenderer(Context context) : base(context)
        {
            c = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }
}
