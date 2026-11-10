using UnityEngine;
using TMPro;

public class QuestNPC : MonoBehaviour
{
    [Header("UI e Referências")]
    public TextMeshProUGUI textoDialogoUI;
    public GameObject painelDialogoUI;
    public GameObject zonaDaMorte;

    [Header("UI de Aviso de Interação")]
    public GameObject avisoInteracaoUI; // Arraste o objeto com o texto "Pressione E para Interagir"

    [Header("Diálogos - Antes da Quest Completa")]
    [TextArea(2, 4)]
    public string[] falasAntesQuest;

    [Header("Diálogos - Depois da Quest Completa")]
    [TextArea(2, 4)]
    public string[] falasDepoisQuest;

    private bool jogadorPerto = false;
    private bool dialogoAtivo = false;
    private int indiceFalaAtual = 0;
    private bool questConcluida = false;
    private PlayerInventory inventarioJogador;

    private void Start()
    {
        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            inventarioJogador = other.GetComponent<PlayerInventory>();

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
            jogadorPerto = false;

            if (avisoInteracaoUI != null)
            {
                avisoInteracaoUI.SetActive(false);
            }

            FecharDialogo();
        }
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogoAtivo)
            {
                IniciarDialogo();
            }
            else
            {
                AvancarDialogo();
            }
        }
    }

    void IniciarDialogo()
    {
        if (inventarioJogador == null) return;

        dialogoAtivo = true;
        indiceFalaAtual = 0;

        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);

        if (painelDialogoUI != null)
            painelDialogoUI.SetActive(true);

        int faltam = inventarioJogador.itensNecessarios - inventarioJogador.itensColetados;

        if (faltam <= 0)
        {
            questConcluida = true;

            if (zonaDaMorte != null && zonaDaMorte.activeSelf)
            {
                zonaDaMorte.SetActive(false);
            }
        }

        ExibirFalaAtual();
    }

    void AvancarDialogo()
    {
        indiceFalaAtual++;

        string[] listaAtual = questConcluida ? falasDepoisQuest : falasAntesQuest;

        if (listaAtual == null || indiceFalaAtual >= listaAtual.Length)
        {
            FecharDialogo();
        }
        else
        {
            ExibirFalaAtual();
        }
    }

    void ExibirFalaAtual()
    {
        string[] listaAtual = questConcluida ? falasDepoisQuest : falasAntesQuest;

        if (listaAtual != null && listaAtual.Length > 0 && indiceFalaAtual < listaAtual.Length)
        {
            string textoFormatado = listaAtual[indiceFalaAtual];

            if (inventarioJogador != null)
            {
                int faltam = Mathf.Max(0, inventarioJogador.itensNecessarios - inventarioJogador.itensColetados);
                textoFormatado = textoFormatado.Replace("{faltam}", faltam.ToString())
                                               .Replace("{totais}", inventarioJogador.itensNecessarios.ToString());
            }

            if (textoDialogoUI != null)
                textoDialogoUI.text = textoFormatado;
        }
        else
        {
            if (textoDialogoUI != null)
            {
                int faltam = inventarioJogador.itensNecessarios - inventarioJogador.itensColetados;
                textoDialogoUI.text = questConcluida
                    ? "Excelente! Você trouxe todos os itens. O caminho mortal agora está liberado!"
                    : $"Olá! Preciso que encontre {inventarioJogador.itensNecessarios} itens. Ainda faltam {faltam}!";
            }
        }
    }

    void FecharDialogo()
    {
        dialogoAtivo = false;
        indiceFalaAtual = 0;

        if (painelDialogoUI != null)
            painelDialogoUI.SetActive(false);

        if (textoDialogoUI != null)
            textoDialogoUI.text = "";

        if (jogadorPerto && avisoInteracaoUI != null)
        {
            avisoInteracaoUI.SetActive(true);
        }
    }
}