using System.Collections.Generic;
using System.Linq; // Essencial para ordenação (Sort)

public class Liga
{
    public string nomeDaLiga;
    public List<Time> timesDaLiga = new List<Time>();
    public List<Partida> calendario = new List<Partida>();

    // ==================================================================
    // NOVO: A nossa tabela de classificação!
    // ==================================================================
    public List<TimeNaLiga> tabelaDeClassificacao = new List<TimeNaLiga>();

    // Função para ordenar a tabela
    public void OrdenarTabela()
    {
        tabelaDeClassificacao = tabelaDeClassificacao.OrderByDescending(t => t.pontos)
                                                     .ThenByDescending(t => t.SaldoDeGols)
                                                     .ThenByDescending(t => t.golsPro)
                                                     .ToList();
    }
}