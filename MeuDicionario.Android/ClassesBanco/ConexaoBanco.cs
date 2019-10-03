using MeuDicionario.Droid.ClassesBanco;
using MeuDicionario.Interfaces;
using SQLite;
using System;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(ConexaoBanco))]
namespace MeuDicionario.Droid.ClassesBanco
{
    public class ConexaoBanco : IConexao
    {
        public SQLiteAsyncConnection RetornaConexao()
        {
            var nome = "MeuDicionario.db3";
            var pasta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(pasta, nome);

            return new SQLiteAsyncConnection(path);
        }
    }
}