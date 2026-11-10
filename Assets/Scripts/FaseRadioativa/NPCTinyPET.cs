using TMPro;
using UnityEngine;

public class NPCTinyPET : MonoBehaviour
{
    [Header("UI de Diálogo")]
    public TextMeshProUGUI textoDialogoUI;
    public GameObject painelDialogoUI;

    [Header("UI de Aviso de Interação")]
    public GameObject avisoInteracaoUI; // "Pressione [E] para falar"

    [Header("Objeto da Barreira a Desativar")]
    public GameObject barreiraInvisivelPorta; // Arraste o Cube da barreira aqui!

    [Header("Falas da Tiny")]
    [TextArea(2, 4)]
    public string[] falasPrimeiraConversa = {
        "Tiny: Ei! O setor radioativo à frente está com níveis críticos.",
        "Tiny: Você precisa de uma PET (Permissão de Entrada de Trabalho) assinada para passar por aquela porta!"
    };

    [TextArea(2, 4)]
    public string[] falasSegundaConversa = {
        "Tiny: Certo, vejo que você está pronto. Vou assinar a sua PET agora.",
        "Tiny: PET Assinada! A porta do setor radioativo está liberada. Cuidado lá dentro!"
    };

    [Header("Estado do Progresso")]
    public bool petAssinada = false;
    public int conversasConcluidas = 0;
    public KeyCode teclaInteracao = KeyCode.E;

    private int indiceFalaAtual = 0;
    private bool jogadorNaArea = false;
    private bool dialogoAtivo = false;

    private void Start()
    {
        if (avisoInteracaoUI != null) avisoInteracaoUI.SetActive(false);
        if (painelDialogoUI != null) painelDialogoUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = true;

            if (!dialogoAtivo && conversasConcluidas < 2 && avisoInteracaoUI != null)
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
            if (avisoInteracaoUI != null) avisoInteracaoUI.SetActive(false);
            FecharDialogo();
        }
    }

    private void Update()
    {
        if (jogadorNaArea && conversasConcluidas < 2 && Input.GetKeyDown(teclaInteracao))
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
        dialogoAtivo = true;
        indiceFalaAtual = 0;

        if (avisoInteracaoUI != null) avisoInteracaoUI.SetActive(false);
        if (painelDialogoUI != null) painelDialogoUI.SetActive(true);

        ExibirFalaAtual();
    }

    private void AvancarDialogo()
    {
        string[] falasAtuais = (conversasConcluidas == 0) ? falasPrimeiraConversa : falasSegundaConversa;

        indiceFalaAtual++;

        if (indiceFalaAtual < falasAtuais.Length)
        {
            ExibirFalaAtual();
        }
        else
        {
            conversasConcluidas++;

            // Na 2ª conversa concluída: Assina a PET e esconde a barreira imediatamente!
            if (conversasConcluidas >= 2)
            {
                petAssinada = true;

                if (barreiraInvisivelPorta != null)
                {
                    barreiraInvisivelPorta.SetActive(false); // Sumir com a barreira na hora!
                }

                Debug.Log("<color=green>PET Assinada pela Tiny!</color> Barreira desativada.");
            }

            FecharDialogo();
        }
    }

    private void ExibirFalaAtual()
    {
        string[] falasAtuais = (conversasConcluidas == 0) ? falasPrimeiraConversa : falasSegundaConversa;

        if (textoDialogoUI != null && indiceFalaAtual < falasAtuais.Length)
        {
            textoDialogoUI.text = falasAtuais[indiceFalaAtual];
        }
    }

    private void FecharDialogo()
    {
        dialogoAtivo = false;
        indiceFalaAtual = 0;

        if (painelDialogoUI != null) painelDialogoUI.SetActive(false);
        if (textoDialogoUI != null) textoDialogoUI.text = "";

        if (jogadorNaArea && conversasConcluidas < 2 && avisoInteracaoUI != null)
        {
            avisoInteracaoUI.SetActive(true);
        }
    }
}