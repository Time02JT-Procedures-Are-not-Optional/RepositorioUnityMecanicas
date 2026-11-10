using UnityEngine;
using TMPro;

public class NPCAjudante : MonoBehaviour
{
    [Header("UI de Diálogo")]
    public TextMeshProUGUI textoDialogoUI;
    public GameObject painelDialogoUI;

    [Header("UI de Aviso de Interação")]
    public GameObject avisoInteracaoUI; // Arraste o objeto com o texto "Pressione E para Interagir"

    [Header("Referências do Jogo")]
    public GameController gameController;

    [Header("Diálogos - Antes de Ativar")]
    [TextArea(2, 4)]
    public string[] falasAntesAtivar;

    [Header("Diálogo - Momento da Ativação")]
    [TextArea(2, 4)]
    public string falaNaAtivacao = "Tudo bem, eu assumo o botão para você!";

    [Header("Diálogos - Após Ativar (Botão Já Assumido)")]
    [TextArea(2, 4)]
    public string[] falasAposAtivar;

    [Header("Mecânica de Automação")]
    public float intervaloAperto = 10f;
    public KeyCode teclaInteracao = KeyCode.E;

    private int indiceFalaAtual = 0;
    private int indiceFalaAposAtivar = 0;
    private bool npcAtivado = false;
    private bool jogadorNaArea = false;
    private bool dialogoAtivo = false;
    private float cronometroAperto;

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

        if (npcAtivado)
        {
            cronometroAperto += Time.deltaTime;

            if (cronometroAperto >= intervaloAperto)
            {
                cronometroAperto = 0f;
                ExecutarApertoDeBotao();
            }
        }
    }

    private void AbrirDialogo()
    {
        dialogoAtivo = true;

        if (avisoInteracaoUI != null)
            avisoInteracaoUI.SetActive(false);

        if (painelDialogoUI != null)
            painelDialogoUI.SetActive(true);

        ExibirFalaAtual();
    }

    private void AvancarDialogo()
    {
        if (!npcAtivado)
        {
            indiceFalaAtual++;

            if (falasAntesAtivar != null && indiceFalaAtual < falasAntesAtivar.Length)
            {
                ExibirFalaAtual();
            }
            else if (indiceFalaAtual == (falasAntesAtivar != null ? falasAntesAtivar.Length : 0))
            {
                ExibirFalaAtual();
                AtivarNPCAjudante();
            }
            else
            {
                FecharDialogo();
            }
        }
        else
        {
            indiceFalaAposAtivar++;

            if (falasAposAtivar != null && indiceFalaAposAtivar < falasAposAtivar.Length)
            {
                ExibirFalaAtual();
            }
            else
            {
                FecharDialogo();
            }
        }
    }

    private void ExibirFalaAtual()
    {
        if (textoDialogoUI == null) return;

        if (!npcAtivado)
        {
            if (falasAntesAtivar != null && falasAntesAtivar.Length > 0 && indiceFalaAtual < falasAntesAtivar.Length)
            {
                textoDialogoUI.text = falasAntesAtivar[indiceFalaAtual];
            }
            else if (indiceFalaAtual >= (falasAntesAtivar != null ? falasAntesAtivar.Length : 0))
            {
                textoDialogoUI.text = string.IsNullOrEmpty(falaNaAtivacao)
                    ? "Tudo bem, eu assumo o botão para você!"
                    : falaNaAtivacao;
            }
        }
        else
        {
            if (falasAposAtivar != null && falasAposAtivar.Length > 0 && indiceFalaAposAtivar < falasAposAtivar.Length)
            {
                textoDialogoUI.text = falasAposAtivar[indiceFalaAposAtivar];
            }
            else
            {
                textoDialogoUI.text = "Pode ir tranquilo, estou cuidando do botão!";
            }
        }
    }

    private void AtivarNPCAjudante()
    {
        npcAtivado = true;
        Debug.Log($"[{gameObject.name}] assumiu o controle do botão!");

        ExecutarApertoDeBotao();
        cronometroAperto = 0f;
    }

    private void FecharDialogo()
    {
        dialogoAtivo = false;

        if (npcAtivado)
        {
            indiceFalaAposAtivar = 0;
        }

        if (painelDialogoUI != null)
            painelDialogoUI.SetActive(false);

        if (textoDialogoUI != null)
            textoDialogoUI.text = "";

        if (jogadorNaArea && avisoInteracaoUI != null)
        {
            avisoInteracaoUI.SetActive(true);
        }
    }

    private void ExecutarApertoDeBotao()
    {
        if (gameController != null)
        {
            gameController.ResetarTempo();
            Debug.Log($"SUCESSO: [{gameObject.name}] apertou o botão e resetou o tempo!");
        }
        else
        {
            Debug.LogError($"ERRO: O campo Game Controller está VAZIO no Inspector de {gameObject.name}!");
        }
    }
}