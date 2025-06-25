using System.Collections.Generic; // ESSENCIAL para podermos usar Listas!

// Esta é a "planta baixa" de um Time.
public class Time
{
    public string nomeDoClube;
    public int reputacao; // Ex: de 1 a 5 estrelas

    // A parte mais importante: um time TEM UMA LISTA de jogadores.
    // É assim que ligamos as duas classes que criámos.
    public List<Jogador> elenco = new List<Jogador>();
}