using UnityEngine;
using TMPro;

public class ConsoleGasesRadioativos : MonoBehaviour
{
    [Header("Referência ao Painel da UI")]
    public PainelGasesRadioativos scriptPainelGases;

    [Header("Aviso de Interação na Tela")]
    public GameObject avisoInteracaoUI;
    public TextMeshProUGUI textoAvisoUI;

    [Header("Teclas")]
    public KeyCode teclaInteracao = KeyCode.E;
    private bool jogadorNaArea = false;

    private void Start()
    {
        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = true;

            if (avisoInteracaoUI != null)
            {
                avisoInteracaoUI.SetActive(true);
                if (textoAvisoUI != null)
                    textoAvisoUI.text = "Pressione [" + teclaInteracao.ToString() + "] para acessar o Painel de Gases";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = false;

            if (avisoInteracaoUI != null)
                avisoInteracaoUI.SetActive(false);

            if (scriptPainelGases != null)
                scriptPainelGases.FecharPainel();
        }
    }

    private void Update()
    {
        if (jogadorNaArea && Input.GetKeyDown(teclaInteracao))
        {
            if (scriptPainelGases != null)
            {
                scriptPainelGases.AbrirPainel();

                if (avisoInteracaoUI != null)
                    avisoInteracaoUI.SetActive(false);
            }
        }
    }
}
