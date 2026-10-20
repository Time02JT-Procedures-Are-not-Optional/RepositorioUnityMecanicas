using UnityEngine;
using TMPro; // Importante para usar TextMeshPro

public class NPCDialogoApenas : MonoBehaviour
{
    [Header("UI de Diálogo")]
    public TextMeshProUGUI textoDialogoUI; // Arraste seu texto de UI do Canvas
    public GameObject painelDialogoUI;    // Arraste a caixa/painel pai do diálogo

    [Header("UI de Aviso de Interação")]
    public GameObject avisoInteracaoUI;   // Arraste o objeto com o texto "Pressione E para Interagir"

    [Header("Configurações do Diálogo")]
    [TextArea(2, 5)]
    public string[] falas; // Adicione quantas frases quiser pelo Inspector
    public KeyCode teclaInteracao = KeyCode.E;

    private int indiceFalaAtual = 0;
    private bool jogadorNaArea = false;
    private bool dialogoAtivo = false;

    private void Start()
    {
        // Garante que o aviso comece invisível ao iniciar o jogo
        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = true;

            // Mostra o aviso na tela apenas se o diálogo ainda não estiver aberto
            if (!dialogoAtivo && avisoInteracaoUI != null)
            {
                avisoInteracaoUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = false;

            // Esconde o aviso de interação quando o jogador se afasta
            if (avisoInteracaoUI != null)
            {
                avisoInteracaoUI.SetActive(false);
            }

            FecharDialogo();
        }
    }

    private void Update()
    {
        if (jogadorNaArea && Input.GetKeyDown(teclaInteracao))
        {
            if (!dialogoAtivo)
            {
                AbrirDialogo();
            }
            else
            {
                AvancarDialogo();
            }
        }
    }

    private void AbrirDialogo()
    {
        if (falas == null || falas.Length == 0) return;

        dialogoAtivo = true;
        indiceFalaAtual = 0;

        // Oculta a mensagem de "Pressione E" enquanto conversa
        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);

        if (painelDialogoUI != null)
            painelDialogoUI.SetActive(true);

        ExibirFalaAtual();
    }

    private void AvancarDialogo()
    {
        indiceFalaAtual++;

        if (falas != null && indiceFalaAtual < falas.Length)
        {
            ExibirFalaAtual();
        }
        else
        {
            FecharDialogo();
        }
    }

    private void ExibirFalaAtual()
    {
        if (textoDialogoUI != null && falas != null && indiceFalaAtual < falas.Length)
        {
            textoDialogoUI.text = falas[indiceFalaAtual];
        }
    }

    private void FecharDialogo()
    {
        dialogoAtivo = false;
        indiceFalaAtual = 0;

        if (painelDialogoUI != null)
            painelDialogoUI.SetActive(false);

        if (textoDialogoUI != null)
            textoDialogoUI.text = "";

        // Se o jogador continuar na área de alcance após fechar a conversa, reapresenta o aviso
        if (jogadorNaArea && avisoInteracaoUI != null)
        {
            avisoInteracaoUI.SetActive(true);
        }
    }
}