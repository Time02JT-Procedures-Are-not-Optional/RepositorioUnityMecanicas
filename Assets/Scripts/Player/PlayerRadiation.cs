using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerRadiation : MonoBehaviour
{
    [Header("UI da Barra Verde")]
    public Image barraVerdePreenchimento;
    public Text textoTimerDescontaminacao; // Opcional

    [Header("Efeito de Borda Escura")]
    public CanvasGroup vinhetaCanvasGroup;
    public float velocidadeTransicao = 3f;

    [Header("Configurações de Radiação")]
    public float radiacaoMaxima = 100f;
    public float radiacaoAtual;
    public float tempoTotalDescontaminacao = 30f; // 30 segundos para drenar 100% da barra na zona

    [Header("Cena de Morte")]
    public string nomeCenaGameOver = "GameOver";

    private float alphaAlvo = 0f;
    private bool estaMorto = false;

    void Start()
    {
        estaMorto = false;
        radiacaoAtual = radiacaoMaxima;
        AtualizarUI();
    }

    void Update()
    {
        if (estaMorto) return;

        // Transição suave do efeito de vinheta de contaminação
        if (vinhetaCanvasGroup != null)
        {
            vinhetaCanvasGroup.alpha = Mathf.MoveTowards(vinhetaCanvasGroup.alpha, alphaAlvo, Time.deltaTime * velocidadeTransicao);
        }
    }

    // Chamado continuadamente pela ZonaRadioativa.cs enquanto o jogador estiver dentro dela
    public void ReceberDanoRadioativo()
    {
        if (estaMorto) return;

        // Calcula a taxa para a barra durar exatamente 30 segundos dentro da área
        float taxaDrenagem = (radiacaoMaxima / tempoTotalDescontaminacao) * Time.deltaTime;
        radiacaoAtual -= taxaDrenagem;
        radiacaoAtual = Mathf.Clamp(radiacaoAtual, 0f, radiacaoMaxima);

        AtualizarUI();

        if (radiacaoAtual <= 0f)
        {
            Morrer();
        }
    }

    // Chamado pelo PocaAcido.cs ao pisar na poça (morte em ~3 segundos) ou usar neutralizante errado
    public void ReduzirRadiacao(float quantidade)
    {
        if (estaMorto) return;

        radiacaoAtual -= quantidade;
        radiacaoAtual = Mathf.Max(radiacaoAtual, 0f);

        // Atualiza a barra verde (fillAmount) e o efeito visual na tela
        AtualizarUI();

        if (radiacaoAtual <= 0f)
        {
            Morrer();
        }
    }

    // Chamado pelo ChuveiroDescontaminacao.cs ao apertar a tecla de interação
    public void RealizarDescontaminacao()
    {
        if (estaMorto) return;

        radiacaoAtual = radiacaoMaxima; // Recupera 100% da barra de radiação
        Debug.Log("<color=green>Descontaminação realizada com sucesso! Barra restaurada.</color>");
        AtualizarUI();
    }

    void AtualizarUI()
    {
        float percentual = radiacaoAtual / radiacaoMaxima;

        if (barraVerdePreenchimento != null)
        {
            barraVerdePreenchimento.fillAmount = percentual;
        }

        if (textoTimerDescontaminacao != null)
        {
            float tempoRestanteEstimado = percentual * tempoTotalDescontaminacao;
            textoTimerDescontaminacao.text = "Tempo de Oxigênio/Radiação: " + Mathf.CeilToInt(tempoRestanteEstimado) + "s";
        }

        // Ativa o efeito escuro na tela se estiver com menos de 50% de radiação
        if (percentual <= 0.5f)
        {
            alphaAlvo = Mathf.InverseLerp(0.5f, 0f, percentual);
        }
        else
        {
            alphaAlvo = 0f;
        }
    }

    void Morrer()
    {
        estaMorto = true;
        Debug.LogError("O jogador não se descontaminou a tempo e morreu por radiação!");
        SceneManager.LoadScene(nomeCenaGameOver);
    }
}