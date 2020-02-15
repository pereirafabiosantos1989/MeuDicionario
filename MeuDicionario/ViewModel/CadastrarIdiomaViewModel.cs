using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeuDicionario.ViewModel
{
    public class CadastrarIdiomaViewModel : BasePropertyChanged
    {
        public ICommand CadastrarIdiomaCommand { get; private set; }

        private SQLiteAsyncConnection _contexto;

        private Idioma _novoIdioma;
        private string _nomeIdioma;

        public string NomeIdioma
        {
            get { return _nomeIdioma; }
            set
            {
                _nomeIdioma = value;
                OnPropertyChanged();
            }
        }

        public Idioma NovoIdioma
        {
            get { return _novoIdioma; }

            set
            {
                _novoIdioma = value;
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
                NovoIdioma = new Idioma();
                NovoIdioma.Nome = NomeIdioma;

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
    }
}
