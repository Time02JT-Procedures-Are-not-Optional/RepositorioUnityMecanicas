using System.Collections;
using UnityEditor;
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
    public float staminaMaxima = 100f;
    public float staminaAtual;
    public float taxaDrenagemStamina = 25f;
    public float taxaRegeneracaoStamina = 15f;
    public float tempoParaRegenerar = 1.5f;

    [Header("UI")]
    public Slider barraStamina;
    public CanvasGroup painelExaustao;

    private Vector3 velocidadeVertical;
    private bool estaNoChao;
    private bool estaCorrendo;
    private bool jaDisparouExaustao = false;
    private Coroutine corrotinaRegeneracao;
    private Coroutine corrotinaEfeitoExaustao;

    void Start()
    {
        staminaAtual = staminaMaxima;

        if (barraStamina != null)
        {
            barraStamina.maxValue = staminaMaxima;
            barraStamina.value = staminaAtual;
        }

        if (painelExaustao != null)
        {
            painelExaustao.alpha = 0f;
        }
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

        if (querCorrer && estaSeMovimentando && staminaAtual > 0)
        {
            estaCorrendo = true;
        }
        else
        {
            estaCorrendo = false;
        }

        // Executa o gerenciamento de stamina a cada frame
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
    // MÉTODOS DE STAMINA E EFEITOS (FORA DO UPDATE)
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
            staminaAtual = Mathf.Clamp(staminaAtual, 0, staminaMaxima);
            AtualizarUI();

            // Se zerou a stamina, marca que entrou em estado de exaustão
            if (staminaAtual <= 0)
            {
                estaCorrendo = false;
                jaDisparouExaustao = true;
            }
        }
        else if (staminaAtual < staminaMaxima && corrotinaRegeneracao == null)
        {
            corrotinaRegeneracao = StartCoroutine(RegenerarStamina());
        }

        // Se a stamina recuperou e passou de 50%, remove o estado de exaustão
        if (staminaAtual >= 50f)
        {
            jaDisparouExaustao = false;
        }

        // Atualiza a visibilidade das bordas em tempo real
        AtualizarEfeitoBordas();
    }

    void AtualizarEfeitoBordas()
    {
        if (painelExaustao != null)
        {
            // Se está em exaustão, suaviza o Alpha até 0.4f (ou 1f para mais forte). Se recuperou >= 50, suaviza até 0f.
            float alphaAlvo = jaDisparouExaustao ? 0.4f : 0f;
            painelExaustao.alpha = Mathf.MoveTowards(painelExaustao.alpha, alphaAlvo, Time.deltaTime * 3f);
        }
    }

    void GatilharEfeitoExaustao()
    {
        if (painelExaustao != null)
        {
            if (corrotinaEfeitoExaustao != null) StopCoroutine(corrotinaEfeitoExaustao);
            corrotinaEfeitoExaustao = StartCoroutine(PulsarTelaExaustao());
        }
    }

    IEnumerator PulsarTelaExaustao()
    {
        float tempo = 0f;
        while (tempo < 0.2f)
        {
            tempo += Time.deltaTime;
            painelExaustao.alpha = Mathf.Lerp(0f, 0.4f, tempo / 0.2f);
            yield return null;
        }

        tempo = 0f;
        while (tempo < 0.5f)
        {
            tempo += Time.deltaTime;
            painelExaustao.alpha = Mathf.Lerp(0.4f, 0f, tempo / 0.5f);
            yield return null;
        }

        painelExaustao.alpha = 0f;
    }

    IEnumerator RegenerarStamina()
    {
        yield return new WaitForSeconds(tempoParaRegenerar);

        while (staminaAtual < staminaMaxima && !estaCorrendo)
        {
            staminaAtual += taxaRegeneracaoStamina * Time.deltaTime;
            staminaAtual = Mathf.Clamp(staminaAtual, 0, staminaMaxima);
            AtualizarUI();
            yield return null;
        }

        corrotinaRegeneracao = null;
    }

    void AtualizarUI()
    {
        if (barraStamina != null)
        {
            barraStamina.value = staminaAtual;
        }
    }
}