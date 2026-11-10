using UnityEngine;

public class ChuveiroDescontaminacao : MonoBehaviour
{
    public KeyCode teclaInteracao = KeyCode.E;
    private bool jogadorPerto = false;
    private PlayerRadiation playerRadiation;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(teclaInteracao))
        {
            if (playerRadiation != null)
            {
                playerRadiation.RealizarDescontaminacao();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            playerRadiation = other.GetComponent<PlayerRadiation>();
            Debug.Log("Pressione [E] no chuveiro para se descontaminar.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            playerRadiation = null;
        }
    }
}