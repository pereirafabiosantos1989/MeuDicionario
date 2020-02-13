using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeuDicionario.ViewModel
{
    public class CadastrarIdiomaViewModel : BasePropertyChange
    {
        private SQLiteAsyncConnection _contexto;

        private Idioma _novoIdioma;

        public Idioma NovoIdioma
        {
            get { return _novoIdioma; }

            set 
            { 
                _novoIdioma = value;
                OnPropertyChanged();
            }
        }

        public CadastrarIdiomaViewModel()
        {
            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
            _contexto.CreateTableAsync<Idioma>();

            CadastrarIdiomaCommand = new Command(CadastrarIdioma);
        }

        public async void CadastrarIdioma()
        {
            try
            {
                if (NovoIdioma != null && !string.IsNullOrEmpty(NovoIdioma.Nome))
                {
                    await _contexto.InsertAsync(NovoIdioma);
                    MessagingCenter.Send(NovoIdioma, "SucessoCadastrarNovoIdioma");
                }
                else
                {
                    throw new Exception("Informe o nome do Idioma.");
                }
            }
            catch (Exception e1)
            {
                MessagingCenter.Send(new Exception(e1.Message), "ErroCadastrarNovoIdioma");
            }
        }

        public ICommand CadastrarIdiomaCommand { get; set; }
    }
}
