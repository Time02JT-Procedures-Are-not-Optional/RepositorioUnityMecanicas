using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para trocar de cena
using TMPro; // Necessário para manipular textos TextMeshPro

public class GameController : MonoBehaviour
{
    [Header("Configurações do Tempo")]
    public float tempoMaximo = 1000f;
    private float tempoRestante;
    private bool jogoAtivo = true;

    [Header("Componentes de UI")]
    public TextMeshProUGUI textoContador;
    public string nomeCenaGameOver = "GameOver";

    void Start()
    {
        // Inicia o cronômetro com o tempo máximo definido
        tempoRestante = tempoMaximo;
    }

    void Update()
    {
        if (!jogoAtivo) return;

        // Diminui o tempo com base no tempo real passado entre quadros
        tempoRestante -= Time.deltaTime;

        // Atualiza o texto na tela (arredondado sem casas decimais)
        if (textoContador != null)
        {
            textoContador.text = "Tempo: " + Mathf.CeilToInt(tempoRestante).ToString() + "s";
        }

        // Se o tempo zerar, chama a função de Game Over
        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            jogoAtivo = false;
            AcionarGameOver();
        }
    }

    // Função que deve ser associada ao clique do botão
    public void ResetarTempo()
    {
        if (!jogoAtivo) return;

        // Reinicia a contagem para o tempo máximo
        tempoRestante = tempoMaximo;
    }

    void AcionarGameOver()
    {
        Debug.Log("O tempo acabou! Carregando tela de Game Over...");

        // Libera e mostra o cursor do mouse para interagir na tela de Game Over
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Carrega a cena de Game Over
        SceneManager.LoadScene(nomeCenaGameOver);
    }
}