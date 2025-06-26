// Define a estrutura de uma única partida de futebol.
public class Partida
{
    public Time timeCasa;
    public Time timeFora;
    
    public int golsCasa;
    public int golsFora;

    // Uma "flag" para sabermos se esta partida já foi simulada ou não.
    // Isso será muito útil no futuro.
    public bool partidaJogada;

    // Um construtor para facilitar a criação de uma nova partida.
    public Partida(Time casa, Time fora)
    {
        timeCasa = casa;
        timeFora = fora;
        partidaJogada = false; // Uma nova partida nunca foi jogada.
    }
}