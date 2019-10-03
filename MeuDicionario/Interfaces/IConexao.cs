using SQLite;

namespace MeuDicionario.Interfaces
{
    public interface IConexao
    {
        SQLiteAsyncConnection RetornaConexao();
    }
}
