using MeuDicionario.Interfaces;
using MeuDicionario.Modelo;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MeuDicionario.ViewModel
{
    public class PesquisarTermoViewModel : BasePropertyChanged
    {
        private SQLiteAsyncConnection _contexto;

        private List<Dicionario> _termosEncontrados;
        private List<Idioma> _idiomas;

        private string _termoAPesquisar;

        private bool _habilitaFiltroIdioma;
        private bool _erroNaPesquisaPorIdioma;
        private bool _listaAtualizada;
        private bool _temResultado;

        private Dicionario termoSelecionado = null;

        public Dicionario TermoSelecionado
        {
            get { return termoSelecionado; }
            set 
            {
                OnPropertyChanged();
                MessagingCenter.Send(value, "TermoFoiSelecionado");
            }
        }

        public PesquisarTermoViewModel()
        {
            _contexto = DependencyService.Get<IConexao>().RetornaConexao();
            LerIdiomasCadastrados();

            HabilitaFiltroIdioma = false;

            /* pesquisar todos */
            Pesquisar(true, false, false);
        }

        public string TermoAPesquisar
        {
            get { return _termoAPesquisar; }
            set
            {
                _termoAPesquisar = value;
                OnPropertyChanged();

                if (HabilitaFiltroIdioma)
                {
                    /* pesquisa pelo termo e pelo idioma selecionado */
                    Pesquisar(false, false, true);
                }
                else
                {
                    /* pesquisa apenas pelo termo */
                    Pesquisar(false, true, false);
                }
            }
        }

        public bool ListaAtualizada
        {
            get { return _listaAtualizada; }
            set
            {
                _listaAtualizada = !value;
                OnPropertyChanged();
            }
        }

        public bool ErroNaPesquisaPorIdioma
        {
            get { return _erroNaPesquisaPorIdioma; }
            set
            {
                _erroNaPesquisaPorIdioma = value;
                OnPropertyChanged();
            }
        }

        public bool TemResultado
        {
            get { return _temResultado; }
            set
            {
                _temResultado = value;
                OnPropertyChanged();
            }
        }

        public List<Dicionario> TermosEncontrados
        {
            get { return _termosEncontrados; }
            set
            {
                _termosEncontrados = value;
                OnPropertyChanged();
            }
        }

        public bool HabilitaFiltroIdioma
        {
            get
            {
                return _habilitaFiltroIdioma;
            }

            set
            {
                _habilitaFiltroIdioma = value;
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

                if (value != null)
                    Pesquisar();
            }
        }

        /// <summary>
        /// Efetua a pesuisa do termo informado no campo de texto
        /// </summary>
        public async void Pesquisar(bool pesquisarTodos = false, bool pesquisarApenasPorTermo = false, bool pesquisarPorTermoEIdioma = false)
        {
            if (pesquisarTodos)
            {
                TermosEncontrados = await _contexto.Table<Dicionario>().ToListAsync();

                return;
            }

            if (pesquisarApenasPorTermo)
            {
                TermosEncontrados = await _contexto.Table<Dicionario>().
                                    Where(x => x.Palavra.Contains(TermoAPesquisar) || x.Traducao.Contains(TermoAPesquisar)).
                                    ToListAsync();

                return;
            }

            if (pesquisarPorTermoEIdioma)
            {
                TermosEncontrados = await _contexto.Table<Dicionario>().
                                    Where(x => x.Idioma.Equals(IdiomaSelecionado.Nome) &&
                                    (x.Palavra.Contains(TermoAPesquisar) || x.Traducao.Contains(TermoAPesquisar))).
                                    ToListAsync();

                return;
            }
        }

        public async void LerIdiomasCadastrados()
        {
            Idiomas = await _contexto.Table<Idioma>().ToListAsync();
        }
    }
}
