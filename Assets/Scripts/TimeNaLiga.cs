// Esta classe guarda as estatísticas de um time DENTRO de uma liga específica.
// Isso mantém nossa classe 'Time' principal mais limpa.
public class TimeNaLiga
{
    public Time timeRef; // Uma referência para o objeto Time original.

    public int pontos;
    public int vitorias;
    public int empates;
    public int derrotas;
    public int golsPro;
    public int golsContra;

    public int SaldoDeGols => golsPro - golsContra; // Uma propriedade que calcula o saldo automaticamente.

    public TimeNaLiga(Time time)
    {
        timeRef = time;
        // Inicializa todas as estatísticas em zero.
        pontos = 0;
        vitorias = 0;
        empates = 0;
        derrotas = 0;
        golsPro = 0;
        golsContra = 0;
    }
}