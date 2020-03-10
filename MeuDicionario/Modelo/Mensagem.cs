namespace MeuDicionario.Modelo
{
    public class Mensagem
    {
        public bool BotaoPressionado { get; set; }

        public Mensagem(bool botaoPresssionado)
        {
            BotaoPressionado = botaoPresssionado;
        }
    }
}
