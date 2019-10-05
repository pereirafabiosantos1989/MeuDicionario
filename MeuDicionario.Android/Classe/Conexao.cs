using MeuDicionario.Interfaces;
using SQLite;
using System.IO;
using MeuDicionario.Droid.Classe;

[assembly: Xamarin.Forms.Dependency(typeof(Conexao))]
namespace MeuDicionario.Droid.Classe
{
    class Conexao : IConexao
    {
        public SQLiteAsyncConnection RetornaConexao()
        {
            var arquivo = "Dicionario.db3";
            var pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            var path = Path.Combine(pasta, arquivo);

            return new SQLiteAsyncConnection(path);
        }
    }
}