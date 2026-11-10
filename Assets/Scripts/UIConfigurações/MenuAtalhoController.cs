using UnityEngine;

public class MenuAtalhosController : MonoBehaviour
{
    [Header("Referência da UI")]
    [Tooltip("Arraste o objeto pai da base e das abas (AtalhosBase) aqui.")]
    public GameObject painelAtalhos;

    [Header("Configuração de Tecla")]
    public KeyCode teclaAtalho = KeyCode.Escape;

    void Start()
    {
        // Garante que o painel comece escondido ao iniciar o jogo
        if (painelAtalhos != null)
        {
            painelAtalhos.SetActive(false);
        }
    }

    void Update()
    {
        // Detecta quando a tecla Esc é pressionada
        if (Input.GetKeyDown(teclaAtalho))
        {
            AlternarPainel();
        }
    }

    public void AlternarPainel()
    {
        if (painelAtalhos != null)
        {
            // Inverte o estado atual: se estiver ativo desativa, se estiver desativado ativa
            bool estadoAtual = painelAtalhos.activeSelf;
            painelAtalhos.SetActive(!estadoAtual);
        }
    }
}
