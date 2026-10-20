using UnityEngine;

public class PainelEletricoTrigger : MonoBehaviour
{
    [Header("UI & Controles")]
    public GameObject painelUI;
    public KeyCode teclaInteracao = KeyCode.E;

    [Header("Configuração de Câmera/Jogador")]
    public MonoBehaviour scriptMovimentoPlayer;

    private bool playerPerto = false;
    private bool painelAberto = false;

    void Start()
    {
        // Garante que o painel comece desativado e o tempo rodando normalmente
        if (painelUI != null)
        {
            painelUI.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Usa Input.GetKeyDown com unscaledTime/IgnorePause para responder mesmo com Time.timeScale = 0
        if (playerPerto && Input.GetKeyDown(teclaInteracao))
        {
            TogglePainel();
        }
        else if (painelAberto && Input.GetKeyDown(KeyCode.Escape))
        {
            FecharPainel();
        }
    }

    void TogglePainel()
    {
        painelAberto = !painelAberto;
        painelUI.SetActive(painelAberto);

        if (painelAberto)
        {
            AbrirPainel();
        }
        else
        {
            FecharPainel();
        }
    }

    public void AbrirPainel()
    {
        painelAberto = true;
        if (painelUI != null)
        {
            painelUI.SetActive(true);
        }

        // Pausa o tempo do jogo
        Time.timeScale = 0f;

        // Libera o cursor para interagir com a UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Desativa a movimentação do jogador
        if (scriptMovimentoPlayer != null)
        {
            scriptMovimentoPlayer.enabled = false;
        }
    }

    public void FecharPainel()
    {
        painelAberto = false;
        if (painelUI != null)
        {
            painelUI.SetActive(false);
        }

        // Despausa o tempo do jogo
        Time.timeScale = 1f;

        // Trava o cursor de volta para o modo de jogo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reativa a movimentação do jogador
        if (scriptMovimentoPlayer != null)
        {
            scriptMovimentoPlayer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
            if (painelAberto)
            {
                FecharPainel();
            }
        }
    }
}