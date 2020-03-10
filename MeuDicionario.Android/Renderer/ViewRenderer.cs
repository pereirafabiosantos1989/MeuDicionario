using Android.Content;
using Android.Support.Design.Widget;
using MeuDicionario.Droid.Renderer;
using MeuDicionario.Modelo.Controles;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MeuView), typeof(MeuViewRenderer))]
namespace MeuDicionario.Droid.Renderer
{
    public class MeuViewRenderer : PageRenderer
    {
        Context c;
        FloatingActionButton b;

        public MeuViewRenderer(Context context) : base(context)
        {
            c = context;

            b = new FloatingActionButton(c);
            b.SetImageResource(Resource.Drawable.pesquisarTermo);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            
            IList<Android.Views.View> views = new List<Android.Views.View>();
            views.Add(b);

            ContentPage pagina = (ContentPage) Element;

            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
            };

            stack.Children.Add(b);

            var abc = pagina.FindByName("stackBotoes");

            if (abc is StackLayout)
            {
                StackLayout layoutLista = (StackLayout)abc;

                layoutLista.Children.Add(stack);
            }            
        }
    }
}
