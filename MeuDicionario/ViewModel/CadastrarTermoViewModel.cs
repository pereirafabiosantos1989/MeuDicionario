using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeuDicionario.ViewModel
{
	public class CadastrarTermoViewModel : BasePropertyChanged
    {
		public ICommand CadastrarTermoCommand { get; private set; }

		private string termo;
		private string traducao;
		private List<Idioma> _idiomas;

		private SQLiteAsyncConnection _contexto;

		public CadastrarTermoViewModel()
		{
			CadastrarTermoCommand = new Command(async () =>
			{
				await CadastrarTermo();
			},
			() =>
			{
				return
					Termo != null && 
					Traducao != null && 
					idiomaSelecionado != null &&
					Termo.Length > 0 &&
					Traducao.Length > 0;
			});

			_contexto = DependencyService.Get<IConexao>().RetornaConexao();
			_contexto.CreateTableAsync<Dicionario>();

			LerIdiomasCadastrados();
		}

		public string Termo
		{
			get { return termo; }
			set
			{
				termo = value;
				OnPropertyChanged();
				((Command)CadastrarTermoCommand).ChangeCanExecute();
			}
		}

		public string Traducao
		{
			get { return traducao; }
			set
			{
				traducao = value;
				OnPropertyChanged();
				((Command)CadastrarTermoCommand).ChangeCanExecute();
			}
		}

		public List<Idioma> Idiomas
		{
			get
			{
				return _idiomas;
			}
			set
			{
				_idiomas = value;
				OnPropertyChanged();
			}
		}

		private Idioma idiomaSelecionado;

		public Idioma IdiomaSelecionado
		{
			get { return idiomaSelecionado; }
			set
			{
				idiomaSelecionado = value;
				OnPropertyChanged();

				((Command)CadastrarTermoCommand).ChangeCanExecute();
			}
		}

		private async Task CadastrarTermo()
		{
			try
			{
				Dicionario item = new Dicionario()
				{
					Palavra = Termo,
					Traducao = Traducao,
					Idioma = idiomaSelecionado.Nome
				};

				await _contexto.InsertAsync(item);

				MessagingCenter.Send(this, "TermoCadastrado");
			}
			catch (Exception e1)
			{
				MessagingCenter.Send(new Exception(e1.Message), "ErroAoCadastrarTermo");
			}
		}

		public async void LerIdiomasCadastrados()
		{
			Idiomas = await _contexto.Table<Idioma>().ToListAsync();
		}
	}
}
