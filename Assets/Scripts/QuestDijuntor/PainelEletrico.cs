using UnityEngine;
using UnityEngine.SceneManagement;

public class PainelEletrico : MonoBehaviour
{
    public bool energiaDesligada = false;
    public bool taskConcluida = false;

    [Header("Game Over & Minigame")]
    public string nomeCenaGameOver = "GameOver";
    public CupimMinigame scriptMinigame; // Arraste o objeto do minigame aqui no Inspector

    private bool jogadorNaArea = false;
    private PlayerInventory inventarioDoJogador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = true;
            inventarioDoJogador = other.GetComponent<PlayerInventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = false;
            inventarioDoJogador = null;
        }
    }

    private void Update()
    {
        if (jogadorNaArea && Input.GetKeyDown(KeyCode.E))
        {
            InteragirComPainel();
        }
    }

    private void InteragirComPainel()
    {
        if (taskConcluida) return;

        if (inventarioDoJogador == null)
        {
            Debug.LogError("O Painel não encontrou o script PlayerInventory no Jogador!");
            return;
        }

        // Teste de Estado
        Debug.Log($"Status do Jogador -> EPI: {inventarioDoJogador.temEPIEletrico} | Veneno: {inventarioDoJogador.temVeneno} | Energia Desligada: {energiaDesligada}");

        // Etapa 1: Choque sem EPI
        if (!energiaDesligada && !inventarioDoJogador.temEPIEletrico)
        {
            Debug.LogError("CHOQUE! Faltou a EPI Elétrica.");
            if (!string.IsNullOrEmpty(nomeCenaGameOver))
                SceneManager.LoadScene(nomeCenaGameOver);
            return;
        }

        // Etapa 2: Desligar Energia
        if (!energiaDesligada && inventarioDoJogador.temEPIEletrico)
        {
            energiaDesligada = true;
            Debug.Log("Energia desligada! Agora você precisa do veneno.");
            return;
        }

        // Etapa 3: Abrir Minigame
        if (energiaDesligada)
        {
            if (inventarioDoJogador.temVeneno)
            {
                if (scriptMinigame != null)
                {
                    Debug.Log("Abrindo Minigame...");
                    scriptMinigame.IniciarMinigame(this);
                }
                else
                {
                    Debug.LogError("ERRO: O campo 'Script Minigame' está VAZIO no Inspector do Painel Elétrico!");
                }
            }
            else
            {
                Debug.LogWarning("Energia já está desligada, mas o jogador NÃO tem o veneno no inventário.");
            }
        }
    }

    public void FinalizarTask()
    {
        taskConcluida = true;
        Debug.Log("Task dos cupins concluída com sucesso no minigame!");
    }
}