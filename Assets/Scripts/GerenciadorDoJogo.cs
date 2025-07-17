using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Text;
using System.Linq; // Essencial para usar funções como .ToList()

public class GerenciadorDoJogo : MonoBehaviour
{
    public Liga nossaLiga;
    public TextMeshProUGUI textoResultadoUI;

    private int rodadaAtual = 0;
    private int jogosPorRodada;

    void Start()
    {
        CriarLiga();
        GerarCalendario(nossaLiga); // Agora esta função é muito mais inteligente.

        jogosPorRodada = nossaLiga.timesDaLiga.Count / 2;

        Debug.Log("Liga '" + nossaLiga.nomeDaLiga + "' pronta. Calendário de " + (nossaLiga.calendario.Count / jogosPorRodada) + " rodadas gerado.");
        textoResultadoUI.text = "Clique em 'Simular Rodada' para começar a temporada!";
    }

    void CriarLiga()
    {
        nossaLiga = new Liga { nomeDaLiga = "Campeonato das Lendas" };

        Time timeA = new Time { nomeDoClube = "Unidos da Vila", reputacao = 4 };
        timeA.elenco.Add(new Jogador { nome = "Ronaldo", ataque = 95, defesa = 30, velocidade = 90, resistencia = 80 });

        Time timeB = new Time { nomeDoClube = "Dragões da Colina", reputacao = 3 };
        timeB.elenco.Add(new Jogador { nome = "Romário", ataque = 98, defesa = 25, velocidade = 92, resistencia = 82 });

        Time timeC = new Time { nomeDoClube = "Gigantes da Baixada", reputacao = 3 };
        timeC.elenco.Add(new Jogador { nome = "Pelé", ataque = 100, defesa = 50, velocidade = 95, resistencia = 95 });

        Time timeD = new Time { nomeDoClube = "Arsenal dos Pampas", reputacao = 2 };
        timeD.elenco.Add(new Jogador { nome = "Ronaldinho", ataque = 97, defesa = 40, velocidade = 94, resistencia = 89 });

        nossaLiga.timesDaLiga.AddRange(new List<Time> { timeA, timeB, timeC, timeD });
    }

    // ==================================================================
    // NOVA VERSÃO: Algoritmo Round-Robin para gerar um calendário justo.
    // ==================================================================
    void GerarCalendario(Liga liga)
    {
        List<Time> times = liga.timesDaLiga.ToList(); // Cria uma cópia da lista para podermos manipular.
        
        // Se tivermos um número ímpar de times, adicionamos um "time fantasma" para folgas.
        // Não é nosso caso agora, mas é bom para o futuro.
        if (times.Count % 2 != 0)
        {
            times.Add(null); // 'null' representa a folga.
        }

        int numRodadas = times.Count - 1;
        int numJogosPorRodada = times.Count / 2;

        // Limpa o calendário antigo antes de gerar um novo.
        liga.calendario.Clear();
        List<Partida> jogosDoTurno = new List<Partida>();

        for (int r = 0; r < numRodadas; r++)
        {
            for (int i = 0; i < numJogosPorRodada; i++)
            {
                Time timeCasa = times[i];
                Time timeFora = times[times.Count - 1 - i];

                // Se não for um jogo de folga, adiciona ao calendário.
                if (timeCasa != null && timeFora != null)
                {
                    // Para garantir jogos em casa e fora variados, invertemos o mando a cada rodada.
                    if (r % 2 == 1)
                    {
                        jogosDoTurno.Add(new Partida(timeFora, timeCasa));
                    }
                    else
                    {
                        jogosDoTurno.Add(new Partida(timeCasa, timeFora));
                    }
                }
            }

            // Lógica de rotação: Fixa o primeiro time e gira o resto.
            Time timeFixo = times[1];
            times.RemoveAt(1);
            times.Add(timeFixo);
        }

        // Adiciona os jogos do primeiro turno ao calendário principal.
        liga.calendario.AddRange(jogosDoTurno);

        // Agora, cria o segundo turno (returno) invertendo o mando de campo.
        List<Partida> jogosDoReturno = new List<Partida>();
        foreach (Partida p in jogosDoTurno)
        {
            jogosDoReturno.Add(new Partida(p.timeFora, p.timeCasa));
        }
        liga.calendario.AddRange(jogosDoReturno);
    }

    // A função de simulação não precisa mudar, pois ela apenas lê o calendário.
    public void SimularProximaRodada()
    {
        jogosPorRodada = nossaLiga.timesDaLiga.Count / 2;
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
            resultadosDaRodada.AppendLine(partida.timeCasa.nomeDoClube + " " + partida.golsCasa + " x " + partida.golsFora + " " + partida.timeFora.nomeDoClube);
        }
        textoResultadoUI.text = resultadosDaRodada.ToString();
        rodadaAtual++;
    }
}