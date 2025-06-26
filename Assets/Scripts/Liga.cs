using System.Collections.Generic; // Essencial para usar Listas!

// Define a estrutura de uma Liga/Campeonato.
public class Liga
{
    public string nomeDaLiga;

    // A lista de todos os times que participam desta liga.
    public List<Time> timesDaLiga = new List<Time>();

    // O calendário completo da temporada, que será uma lista de Partidas.
    public List<Partida> calendario = new List<Partida>();
}