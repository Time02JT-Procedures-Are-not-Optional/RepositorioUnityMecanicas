using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    [System.Serializable]
    public class OpcaoMenu
    {
        public RectTransform elementoUI; // O botão/caixinha branca da opção
    }

    [Header("Opções do Menu")]
    public OpcaoMenu[] opcoes;

    [Header("Marcador (Check Verde)")]
    public Image marcadorImage; // Arraste o componente Image do Marcador aqui
    private RectTransform marcadorRect;

    [Tooltip("Ajuste fino caso o sprite precise de centralização extra dentro da caixa")]
    public Vector2 offsetMarcador = Vector2.zero;

    [Header("Configurações do Efeito de Revelação")]
    public float duracaoRevelacao = 0.2f; // Tempo em segundos para aparecer da esquerda pra direita

    [Header("Configuração de Cena")]
    public string nomeCenaJogo = "SampleScene";

    private int indiceSelecionado = 0;
    private Coroutine corrotinaRevelacao;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (marcadorImage != null)
        {
            marcadorRect = marcadorImage.rectTransform;

            // Garante que a imagem está configurada para preenchimento horizontal
            marcadorImage.type = Image.Type.Filled;
            marcadorImage.fillMethod = Image.FillMethod.Horizontal;
            marcadorImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }

        // Posiciona no primeiro item e revela imediatamente
        AtualizarSelecao(instantaneo: true);
    }

    void Update()
    {
        // 1. Navegação com W/S ou Setas Cima/Baixo
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MudarSelecao(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MudarSelecao(1);
        }

        // 2. Confirmação com Enter ou Espaço
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            ExecutarOpcao();
        }
    }

    void MudarSelecao(int direcao)
    {
        indiceSelecionado += direcao;

        if (indiceSelecionado < 0)
            indiceSelecionado = opcoes.Length - 1;
        else if (indiceSelecionado >= opcoes.Length)
            indiceSelecionado = 0;

        AtualizarSelecao(instantaneo: false);
    }

    void AtualizarSelecao(bool instantaneo)
    {
        if (marcadorImage == null || opcoes == null || opcoes.Length == 0) return;

        RectTransform alvoTransform = opcoes[indiceSelecionado].elementoUI;
        if (alvoTransform == null) return;

        // Reposiciona o marcador instantaneamente na caixinha alvo
        marcadorRect.position = alvoTransform.position + (Vector3)offsetMarcador;

        // Interrompe a animação anterior se ainda estiver rodando
        if (corrotinaRevelacao != null)
        {
            StopCoroutine(corrotinaRevelacao);
        }

        if (instantaneo)
        {
            marcadorImage.fillAmount = 1f;
        }
        else
        {
            // Inicia o efeito de desenhar da esquerda para a direita
            corrotinaRevelacao = StartCoroutine(AnimarRevelacaoMarcador());
        }
    }

    IEnumerator AnimarRevelacaoMarcador()
    {
        marcadorImage.fillAmount = 0f; // Some completamente
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracaoRevelacao)
        {
            tempoDecorrido += Time.deltaTime;
            marcadorImage.fillAmount = Mathf.Clamp01(tempoDecorrido / duracaoRevelacao);
            yield return null;
        }

        marcadorImage.fillAmount = 1f; // Garante que preencheu 100%
        corrotinaRevelacao = null;
    }

    public void ExecutarOpcao()
    {
        switch (indiceSelecionado)
        {
            case 0: // Iniciar
                IniciarJogo();
                break;
            case 1: // NRs
                Debug.Log("NRs selecionado!");
                break;
            case 2: // Configurações
                Debug.Log("Configurações selecionado!");
                break;
            case 3: // Créditos
                Debug.Log("Créditos selecionado!");
                break;
            case 4: // Sair
                SairDoJogo();
                break;
        }
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}