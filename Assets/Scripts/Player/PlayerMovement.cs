using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Componentes")]
    public CharacterController controller;
    public Animator animator;

    [Header("Movimentação Top-Down")]
    public float velocidade = 150f;
    public float velocidadeSprint = 300f;
    public float velocidadeRotacao = 15f;

    [Header("Pulo e Gravidade")]
    public float forcaPulo = 40f;
    public float gravidade = -130f;
    public Transform checadorDeChao;
    public float distanciaChao = 0.4f;
    public LayerMask mascaraChao;

    [Header("Configurações de Stamina")]
    public float staminaMaximaOriginal = 100f;
    public float staminaMaximaAtual = 100f;
    public float staminaAtual;
    public float taxaDrenagemStamina = 25f;
    public float taxaRegeneracaoStamina = 15f;
    public float tempoParaRegenerar = 1.5f;

    [Header("Estado de Exaustão (Tela Vermelha)")]
    public bool telaVermelhaAtiva = false; // Bloqueia a corrida enquanto estiver ativa

    [Header("UI de Estamina")]
    public Image imagemEstaminaRadial;
    public CanvasGroup painelExaustao;

    private Vector3 velocidadeVertical;
    private bool estaNoChao;
    private bool estaCorrendo;
    private Coroutine corrotinaRegeneracao;

    void Start()
    {
        staminaMaximaAtual = staminaMaximaOriginal;
        staminaAtual = staminaMaximaAtual;

        if (painelExaustao != null)
        {
            painelExaustao.alpha = 0f;
        }

        AtualizarUI();
    }

    void Update()
    {
        // 1. CHECAGEM DO CHÃO
        estaNoChao = Physics.CheckSphere(checadorDeChao.position, distanciaChao, mascaraChao);

        if (estaNoChao && velocidadeVertical.y < 0)
        {
            velocidadeVertical.y = -2f;
        }

        // 2. LÓGICA DE STAMINA E SPRINT
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direcao = new Vector3(horizontal, 0f, vertical).normalized;
        bool querCorrer = Input.GetKey(KeyCode.LeftShift);
        bool estaSeMovimentando = direcao.magnitude >= 0.1f;

        // Só corre se tiver estamina > 0 E a tela vermelha NÃO estiver ativa
        if (querCorrer && estaSeMovimentando && staminaAtual > 0 && !telaVermelhaAtiva)
        {
            estaCorrendo = true;
        }
        else
        {
            estaCorrendo = false;
        }

        GerenciarStamina();

        // 3. MOVIMENTAÇÃO
        if (estaSeMovimentando)
        {
            Quaternion rotacaoDesejada = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoDesejada, velocidadeRotacao * Time.deltaTime);

            float velocidadeAtual = estaCorrendo ? velocidadeSprint : velocidade;
            controller.Move(direcao * velocidadeAtual * Time.deltaTime);
        }

        // 4. PULO E GRAVIDADE
        if (Input.GetButtonDown("Jump") && estaNoChao)
        {
            velocidadeVertical.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
        }

        velocidadeVertical.y += gravidade * Time.deltaTime;
        controller.Move(velocidadeVertical * Time.deltaTime);

        // 5. ANIMATOR
        if (animator != null)
        {
            float parametroVelocidade = estaSeMovimentando ? (estaCorrendo ? 2f : 1f) : 0f;
            animator.SetFloat("Speed", parametroVelocidade);
            animator.SetBool("estaNoChao", estaNoChao);
        }
    }

    // ---------------------------------------------------------
    // MÉTODOS DE STAMINA E CAFÉ
    // ---------------------------------------------------------
    void GerenciarStamina()
    {
        if (estaCorrendo)
        {
            if (corrotinaRegeneracao != null)
            {
                StopCoroutine(corrotinaRegeneracao);
                corrotinaRegeneracao = null;
            }

            staminaAtual -= taxaDrenagemStamina * Time.deltaTime;
            staminaAtual = Mathf.Clamp(staminaAtual, 0, staminaMaximaAtual);
            AtualizarUI();

            // CHEGOU A ZERO: Liga tela vermelha e reduz o teto máximo em 25%
            if (staminaAtual <= 0)
            {
                estaCorrendo = false;
                telaVermelhaAtiva = true;

                staminaMaximaAtual -= staminaMaximaOriginal * 0.25f;
                staminaMaximaAtual = Mathf.Max(staminaMaximaAtual, 0f);

                Debug.LogWarning($"Estamina ZEROU! Tela vermelha ligada. Novo teto máximo: {staminaMaximaAtual}");
            }
        }
        else if (staminaAtual < staminaMaximaAtual && corrotinaRegeneracao == null)
        {
            // Começa a regenerar estamina se estiver abaixo do teto atual
            corrotinaRegeneracao = StartCoroutine(RegenerarStamina());
        }

        AtualizarEfeitoBordas();
    }

    IEnumerator RegenerarStamina()
    {
        yield return new WaitForSeconds(tempoParaRegenerar);

        while (staminaAtual < staminaMaximaAtual && !estaCorrendo)
        {
            staminaAtual += taxaRegeneracaoStamina * Time.deltaTime;
            staminaAtual = Mathf.Clamp(staminaAtual, 0, staminaMaximaAtual);
            AtualizarUI();
            yield return null;
        }

        // QUANDO Atinge O MÁXIMO PERMITIDO (ex: 75%): apaga a tela vermelha e libera a corrida
        if (staminaAtual >= staminaMaximaAtual && staminaMaximaAtual > 0)
        {
            telaVermelhaAtiva = false;
            Debug.Log("Estamina regenerou até ao limite atual. Tela vermelha apagada e corrida liberada!");
        }

        corrotinaRegeneracao = null;
    }

    // Método chamado ao interagir com a Máquina de Café
    public void TomarCafe()
    {
        float valorRecuperado = staminaMaximaOriginal * 0.25f;

        // Devolve os 25% perdidos ao teto máximo
        staminaMaximaAtual = Mathf.Min(staminaMaximaAtual + valorRecuperado, staminaMaximaOriginal);

        // Adiciona estamina atual
        staminaAtual = Mathf.Min(staminaAtual + valorRecuperado, staminaMaximaAtual);

        // Remove a tela vermelha imediatamente
        telaVermelhaAtiva = false;

        AtualizarUI();
        Debug.Log($"Tomou café! Estamina Atual: {staminaAtual} | Máxima Atual: {staminaMaximaAtual}");
    }

    void AtualizarEfeitoBordas()
    {
        if (painelExaustao != null)
        {
            float alphaAlvo = telaVermelhaAtiva ? 0.6f : 0f;
            painelExaustao.alpha = Mathf.MoveTowards(painelExaustao.alpha, alphaAlvo, Time.deltaTime * 4f);
        }
    }

    void AtualizarUI()
    {
        if (imagemEstaminaRadial != null)
        {
            imagemEstaminaRadial.fillAmount = staminaAtual / staminaMaximaOriginal;
        }
    }
}