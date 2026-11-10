using UnityEngine;

public class MaquinaCafe : MonoBehaviour
{
    [Header("Configuração")]
    public KeyCode teclaInteracao = KeyCode.E;
    private bool jogadorPerto = false;
    private PlayerMovement playerMovement;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(teclaInteracao))
        {
            if (playerMovement != null)
            {
                playerMovement.TomarCafe();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            playerMovement = other.GetComponent<PlayerMovement>();
            Debug.Log("Pressione [E] para tomar um café e recuperar a corrida!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            playerMovement = null;
        }
    }
}