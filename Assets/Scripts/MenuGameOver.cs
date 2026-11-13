using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    [Header("Nomes Exatos das Cenas")]
    public string nomePrimeiraCena = "SampleScene";
    public string nomeCenaMenu = "MenuInicial";

    void Start()
    {
        // Garante que o tempo está normal e o rato está livre e visível
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RecomecarJogo()
    {
        SceneManager.LoadScene(nomePrimeiraCena);
    }

    public void VoltarAoMenu()
    {
        SceneManager.LoadScene(nomeCenaMenu);
    }
}