using UnityEngine;

public class ItemLiberaCaminho : MonoBehaviour
{
    [Header("O que será liberado")]
    public GameObject paredeInvisivel;      // Arraste a parede que bloqueia o jogador aqui
    public GameObject avisoInteracaoUI;     // Opcional: texto "Aperte E para pegar"

    [Header("Configuração")]
    public KeyCode teclaInteracao = KeyCode.E;

    private bool jogadorPerto = false;

    void Start()
    {
        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(teclaInteracao))
        {
            PegarItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            if (avisoInteracaoUI != null)
                avisoInteracaoUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            if (avisoInteracaoUI != null)
                avisoInteracaoUI.SetActive(false);
        }
    }

    private void PegarItem()
    {
        // Esconde o aviso da tela
        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);

        // Desativa a parede invisível para liberar a passagem
        if (paredeInvisivel != null)
            paredeInvisivel.SetActive(false);

        // Remove o objeto coletado da cena
        Destroy(gameObject);
    }
}