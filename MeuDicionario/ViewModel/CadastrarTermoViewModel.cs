using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
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
				try
				{
					if (Termo == null || Traducao == null ||
						Termo.Equals(string.Empty) || Traducao.Equals(string.Empty) ||
						idiomaSelecionado == null)
					{
						MessagingCenter.Send(this, "PreencherCampos");
						return;
					}

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
			}
		}

		public string Traducao
		{
			get { return traducao; }
			set 
			{ 
				traducao = value;
				OnPropertyChanged();
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
			}
		}

		public async void LerIdiomasCadastrados()
		{
			Idiomas = await _contexto.Table<Idioma>().ToListAsync();
		}
	}
}
