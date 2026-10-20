using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public string nomeCenaGameOver = "GameOver"; // Nome exato da sua cena de GameOver

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Carrega instantaneamente a tela de GameOver
            SceneManager.LoadScene(nomeCenaGameOver);
        }
    }
}