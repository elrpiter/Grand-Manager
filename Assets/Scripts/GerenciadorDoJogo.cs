using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Text;
using System.Linq;

public class GerenciadorDoJogo : MonoBehaviour
{
    public Liga nossaLiga;
    public TextMeshProUGUI textoResultadoUI;
    
    // ==================================================================
    // NOVO: Referência para o texto que mostrará a tabela.
    // ==================================================================
    public TextMeshProUGUI textoTabelaUI;

    private int rodadaAtual = 0;
    private int jogosPorRodada;

    void Start()
    {
        CriarLiga();
        GerarCalendario(nossaLiga);
        jogosPorRodada = nossaLiga.timesDaLiga.Count / 2;
        
        textoResultadoUI.text = "Clique em 'Simular Rodada' para começar a temporada!";
        AtualizarTabelaUI(); // Mostra a tabela zerada no início.
    }

    void CriarLiga()
    {
        nossaLiga = new Liga { nomeDaLiga = "Campeonato Carioca" };

        Time timeA = new Time { nomeDoClube = "Flamengo", reputacao = 4 };
        timeA.elenco.Add(new Jogador { nome = "Ronaldo", ataque = 95, defesa = 30, velocidade = 90, resistencia = 80 });

        Time timeB = new Time { nomeDoClube = "Fluminense", reputacao = 3 };
        timeB.elenco.Add(new Jogador { nome = "Romário", ataque = 98, defesa = 25, velocidade = 92, resistencia = 82 });

        Time timeC = new Time { nomeDoClube = "Botafogo", reputacao = 3 };
        timeC.elenco.Add(new Jogador { nome = "Pelé", ataque = 100, defesa = 50, velocidade = 95, resistencia = 95 });

        Time timeD = new Time { nomeDoClube = "Vasco da Gama", reputacao = 2 };
        timeD.elenco.Add(new Jogador { nome = "Ronaldinho", ataque = 97, defesa = 40, velocidade = 94, resistencia = 89 });

        nossaLiga.timesDaLiga.AddRange(new List<Time> { timeA, timeB, timeC, timeD });

        // NOVO: Cria a "ficha" de cada time para a tabela de classificação.
        foreach (Time t in nossaLiga.timesDaLiga)
        {
            nossaLiga.tabelaDeClassificacao.Add(new TimeNaLiga(t));
        }
    }
    
    void GerarCalendario(Liga liga)
    {
        List<Time> times = liga.timesDaLiga.ToList();
        if (times.Count % 2 != 0) { times.Add(null); }
        int numRodadas = times.Count - 1;
        int numJogosPorRodada = times.Count / 2;
        liga.calendario.Clear();
        List<Partida> jogosDoTurno = new List<Partida>();
        for (int r = 0; r < numRodadas; r++)
        {
            for (int i = 0; i < numJogosPorRodada; i++)
            {
                Time timeCasa = times[i];
                Time timeFora = times[times.Count - 1 - i];
                if (timeCasa != null && timeFora != null)
                {
                    if (r % 2 == 1) { jogosDoTurno.Add(new Partida(timeFora, timeCasa)); }
                    else { jogosDoTurno.Add(new Partida(timeCasa, timeFora)); }
                }
            }
            Time timeFixo = times[1];
            times.RemoveAt(1);
            times.Add(timeFixo);
        }
        liga.calendario.AddRange(jogosDoTurno);
        List<Partida> jogosDoReturno = new List<Partida>();
        foreach (Partida p in jogosDoTurno) { jogosDoReturno.Add(new Partida(p.timeFora, p.timeCasa)); }
        liga.calendario.AddRange(jogosDoReturno);
    }


    public void SimularProximaRodada()
    {
        if (rodadaAtual * jogosPorRodada >= nossaLiga.calendario.Count)
        {
            textoResultadoUI.text = "Fim da temporada!";
            return;
        }

        StringBuilder resultadosDaRodada = new StringBuilder();
        resultadosDaRodada.AppendLine("--- Resultados da Rodada " + (rodadaAtual + 1) + " ---");

        int primeiroJogoDaRodada = rodadaAtual * jogosPorRodada;
        int ultimoJogoDaRodada = primeiroJogoDaRodada + jogosPorRodada;

        for (int i = primeiroJogoDaRodada; i < ultimoJogoDaRodada; i++)
        {
            Partida partida = nossaLiga.calendario[i];
            SimularLogicaDaPartida(partida);
            AtualizarEstatisticas(partida);
            resultadosDaRodada.AppendLine(partida.timeCasa.nomeDoClube + " " + partida.golsCasa + " x " + partida.golsFora + " " + partida.timeFora.nomeDoClube);
        }

        nossaLiga.OrdenarTabela();
        AtualizarTabelaUI();
        
        textoResultadoUI.text = resultadosDaRodada.ToString();
        rodadaAtual++;
    }

    void SimularLogicaDaPartida(Partida partida)
    {
        int forcaAtaqueCasa = 0;
        foreach (Jogador j in partida.timeCasa.elenco) forcaAtaqueCasa += j.ataque;
        int forcaDefesaFora = 0;
        foreach (Jogador j in partida.timeFora.elenco) forcaDefesaFora += j.defesa;
        partida.golsCasa = Random.Range(0, (forcaAtaqueCasa / 50) - (forcaDefesaFora / 100) + 2);
        if (partida.golsCasa < 0) partida.golsCasa = 0;
            
        int forcaAtaqueFora = 0;
        foreach (Jogador j in partida.timeFora.elenco) forcaAtaqueFora += j.ataque;
        int forcaDefesaCasa = 0;
        foreach (Jogador j in partida.timeCasa.elenco) forcaDefesaCasa += j.defesa;
        partida.golsFora = Random.Range(0, (forcaAtaqueFora / 50) - (forcaDefesaCasa / 100) + 2);
        if (partida.golsFora < 0) partida.golsFora = 0;
            
        partida.partidaJogada = true;
    }

    void AtualizarEstatisticas(Partida partida)
    {
        TimeNaLiga dadosCasa = nossaLiga.tabelaDeClassificacao.Find(t => t.timeRef == partida.timeCasa);
        TimeNaLiga dadosFora = nossaLiga.tabelaDeClassificacao.Find(t => t.timeRef == partida.timeFora);

        dadosCasa.golsPro += partida.golsCasa;
        dadosCasa.golsContra += partida.golsFora;
        dadosFora.golsPro += partida.golsFora;
        dadosFora.golsContra += partida.golsCasa;

        if (partida.golsCasa > partida.golsFora)
        {
            dadosCasa.vitorias++;
            dadosCasa.pontos += 3;
            dadosFora.derrotas++;
        }
        else if (partida.golsFora > partida.golsCasa)
        {
            dadosFora.vitorias++;
            dadosFora.pontos += 3;
            dadosCasa.derrotas++;
        }
        else
        {
            dadosCasa.empates++;
            dadosCasa.pontos += 1;
            dadosFora.empates++;
        }
    }

    void AtualizarTabelaUI()
    {
        StringBuilder tabelaTexto = new StringBuilder();
        tabelaTexto.AppendLine("--- CLASSIFICAÇÃO ---");
        // Usamos <mspace=...px> para alinhar o texto como uma tabela. Ajuste o valor de 'px' se necessário.
        tabelaTexto.AppendLine("<b><mspace=35px>P</mspace> <mspace=25px>J</mspace> <mspace=25px>V</mspace> <mspace=25px>E</mspace> <mspace=25px>D</mspace> <mspace=35px>GP</mspace> <mspace=35px>GC</mspace> <mspace=35px>SG</mspace> <mspace=150px>CLUBE</mspace></b>");

        foreach (TimeNaLiga dados in nossaLiga.tabelaDeClassificacao)
        {
            int jogos = dados.vitorias + dados.empates + dados.derrotas;
            tabelaTexto.AppendLine(
                $"<mspace=35px>{dados.pontos}</mspace> <mspace=25px>{jogos}</mspace> <mspace=25px>{dados.vitorias}</mspace> <mspace=25px>{dados.empates}</mspace> <mspace=25px>{dados.derrotas}</mspace> <mspace=35px>{dados.golsPro}</mspace> <mspace=35px>{dados.golsContra}</mspace> <mspace=35px>{dados.SaldoDeGols}</mspace> {dados.timeRef.nomeDoClube}"
            );
        }

        textoTabelaUI.text = tabelaTexto.ToString();
    }
}