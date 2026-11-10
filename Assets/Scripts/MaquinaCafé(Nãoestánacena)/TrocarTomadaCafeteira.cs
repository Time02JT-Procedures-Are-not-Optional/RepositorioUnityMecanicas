using UnityEngine;
using TMPro;

public class TrocarTomadaCafeteira : MonoBehaviour
{
    [Header("Configurações da Tomada")]
    public KeyCode teclaInteracao = KeyCode.E;
    public bool tomadaTrocada = false;

    [Header("Efeitos Visuais/Objetos")]
    public GameObject tomadaQuebradaObj;
    public GameObject tomadaNovaObj;

    [Header("UI")]
    public GameObject avisoInteracaoUI;
    public TextMeshProUGUI textoAvisoUI;

    private bool jogadorPerto = false;

    void Start()
    {
        if (avisoInteracaoUI != null) avisoInteracaoUI.SetActive(false);
        AtualizarVisualTomada();
    }

    void Update()
    {
        if (jogadorPerto && !tomadaTrocada && Input.GetKeyDown(teclaInteracao))
        {
            TrocarTomada();
        }
    }

    void TrocarTomada()
    {
        tomadaTrocada = true;
        AtualizarVisualTomada();

        if (avisoInteracaoUI != null) avisoInteracaoUI.SetActive(false);
        Debug.Log("<color=green>Tomada trocada com sucesso! Cafeteira pronta para uso.</color>");
    }

    void AtualizarVisualTomada()
    {
        if (tomadaQuebradaObj != null) tomadaQuebradaObj.SetActive(!tomadaTrocada);
        if (tomadaNovaObj != null) tomadaNovaObj.SetActive(tomadaTrocada);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !tomadaTrocada)
        {
            jogadorPerto = true;
            if (avisoInteracaoUI != null)
            {
                avisoInteracaoUI.SetActive(true);
                if (textoAvisoUI != null) textoAvisoUI.text = "Pressione [E] para trocar a tomada da cafeteira";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            if (avisoInteracaoUI != null) avisoInteracaoUI.SetActive(false);
        }
    }
}