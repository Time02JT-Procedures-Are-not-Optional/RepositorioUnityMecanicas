using UnityEngine;

public class WireGameManager : MonoBehaviour
{
    public static WireGameManager Instance;

    [Header("Objetos do Jogo")]
    public PlayerWire[] allWires;

    [Header("Telas de Interface (UI)")]
    public GameObject gameOverUI;
    public GameObject winUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckGameCompletion()
    {
        foreach (PlayerWire wire in allWires)
        {
            if (!wire.isConnected)
            {
                return; // Se falta algum fio conectar, o jogo continua
            }
        }

        // Todos os fios foram conectados com sucesso
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        Debug.Log("Você venceu! Todos os fios foram conectados corretamente.");
    }

    public void TriggerGameOver(string reason)
    {
        Debug.Log("Game Over: " + reason);

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        Time.timeScale = 0f; // Congela o jogo ao perder
    }
}