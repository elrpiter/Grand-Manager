using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script vai gerenciar a criação e o estado do nosso jogo.
// Como ele está anexado a um objeto na cena, ele USA o MonoBehaviour.
public class GerenciadorDoJogo : MonoBehaviour
{
    // Vamos declarar duas variáveis para guardar nossos times.
    // A palavra 'public' faz com que elas apareçam no Inspector da Unity, o que é ótimo para debug.
    public Time timeA;
    public Time timeB;

    // A função Start() é chamada pela Unity UMA VEZ, no primeiro frame em que o script está ativo.
    // É o lugar perfeito para configurar nosso estado inicial.
    void Start()
    {
        // --- CRIAÇÃO DO TIME A ---
        timeA = new Time(); // 'new Time()' cria um objeto Time em branco, usando nossa "planta baixa".
        timeA.nomeDoClube = "Unidos da Vila";
        timeA.reputacao = 4;

        // Agora, vamos criar os jogadores e adicioná-los ao elenco do timeA.
        timeA.elenco.Add(new Jogador { nome = "Ronaldo", ataque = 95, defesa = 30, velocidade = 90, resistencia = 80 });
        timeA.elenco.Add(new Jogador { nome = "Zidane", ataque = 90, defesa = 75, velocidade = 85, resistencia = 90 });
        timeA.elenco.Add(new Jogador { nome = "Maldini", ataque = 60, defesa = 98, velocidade = 88, resistencia = 95 });


        // --- CRIAÇÃO DO TIME B ---
        timeB = new Time(); // Criando um segundo objeto Time.
        timeB.nomeDoClube = "Dragões da Colina";
        timeB.reputacao = 3;

        // Criando e adicionando jogadores ao elenco do timeB.
        timeB.elenco.Add(new Jogador { nome = "Romário", ataque = 98, defesa = 25, velocidade = 92, resistencia = 82 });
        timeB.elenco.Add(new Jogador { nome = "Iniesta", ataque = 85, defesa = 80, velocidade = 86, resistencia = 88 });
        timeB.elenco.Add(new Jogador { nome = "Gamarra", ataque = 40, defesa = 97, velocidade = 80, resistencia = 94 });


        // --- VERIFICAÇÃO NO CONSOLE ---
        // O Debug.Log imprime mensagens no Console da Unity. É a nossa principal ferramenta de verificação.
        Debug.Log("Times e jogadores criados com sucesso!");
        Debug.Log(timeA.nomeDoClube + " tem " + timeA.elenco.Count + " jogadores.");
        Debug.Log("Craque do time: " + timeA.elenco[0].nome + " com ataque " + timeA.elenco[0].ataque);
        Debug.Log("--------------------");
        Debug.Log(timeB.nomeDoClube + " tem " + timeB.elenco.Count + " jogadores.");
        Debug.Log("Craque do time: " + timeB.elenco[0].nome + " com ataque " + timeB.elenco[0].ataque);
    }
}