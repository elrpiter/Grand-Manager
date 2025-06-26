using UnityEngine;
using System.Collections.Generic;
using TMPro; // IMPORTANTE: Precisamos disso para interagir com textos do TextMeshPro.

public class GerenciadorDoJogo : MonoBehaviour
{
    public Time timeA;
    public Time timeB;

    // ==================================================================
    // NOVO: Uma referência para o nosso objeto de Texto na UI.
    // ==================================================================
    public TextMeshProUGUI textoResultadoUI;


    void Start()
    {
        // --- CRIAÇÃO DOS TIMES (sem mudanças aqui) ---
        timeA = new Time();
        timeA.nomeDoClube = "Unidos da Vila";
        timeA.reputacao = 4;
        timeA.elenco.Add(new Jogador { nome = "Ronaldo", ataque = 95, defesa = 30, velocidade = 90, resistencia = 80 });
        timeA.elenco.Add(new Jogador { nome = "Zidane", ataque = 90, defesa = 75, velocidade = 85, resistencia = 90 });
        timeA.elenco.Add(new Jogador { nome = "Maldini", ataque = 60, defesa = 98, velocidade = 88, resistencia = 95 });

        timeB = new Time();
        timeB.nomeDoClube = "Dragões da Colina";
        timeB.reputacao = 3;
        timeB.elenco.Add(new Jogador { nome = "Romário", ataque = 98, defesa = 25, velocidade = 92, resistencia = 82 });
        timeB.elenco.Add(new Jogador { nome = "Iniesta", ataque = 85, defesa = 80, velocidade = 86, resistencia = 88 });
        timeB.elenco.Add(new Jogador { nome = "Gamarra", ataque = 40, defesa = 97, velocidade = 80, resistencia = 94 });

        // Não vamos mais simular a partida automaticamente. O botão fará isso.
        Debug.Log("Times e jogadores criados. Pronto para simular.");
    }

    // ==================================================================
    // MUDANÇA: A função agora é 'public' para que o botão possa chamá-la.
    // E não precisa mais de parâmetros, pois usará os times da classe.
    // ==================================================================
    public void SimularPartida()
    {
        int forcaAtaqueCasa = 0;
        int forcaDefesaCasa = 0;
        foreach (Jogador j in timeA.elenco)
        {
            forcaAtaqueCasa += j.ataque;
            forcaDefesaCasa += j.defesa;
        }

        int forcaAtaqueFora = 0;
        int forcaDefesaFora = 0;
        foreach (Jogador j in timeB.elenco)
        {
            forcaAtaqueFora += j.ataque;
            forcaDefesaFora += j.defesa;
        }

        int golsCasa = Random.Range(0, (forcaAtaqueCasa / 50) - (forcaDefesaFora / 100) + 2);
        int golsFora = Random.Range(0, (forcaAtaqueFora / 50) - (forcaDefesaCasa / 100) + 2);

        if (golsCasa < 0) golsCasa = 0;
        if (golsFora < 0) golsFora = 0;
        
        // ==================================================================
        // NOVO: Em vez de Debug.Log, vamos atualizar nosso texto na tela!
        // ==================================================================
        string resultado = timeA.nomeDoClube + " " + golsCasa + " x " + golsFora + " " + timeB.nomeDoClube;
        textoResultadoUI.text = resultado; // Atualiza o texto do objeto de UI.
        Debug.Log("Partida simulada: " + resultado); // Manter o Debug.Log é bom para verificar.
    }
}