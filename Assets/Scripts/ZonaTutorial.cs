using UnityEngine;
using TMPro;

public class ZonaTutorial : MonoBehaviour
{
    [Header("Configurações da UI")]
    public GameObject painelTutorial;
    public TextMeshProUGUI textoUI;

    [Header("Falas do Tutorial (Adicione quantas quiser)")]
    [TextArea(3, 5)]
    public string[] falas;

    [Header("Controles")]
    public KeyCode teclaAvancar = KeyCode.E;
    public bool ativarApenasUmaVez = true;

    private int indiceFalaAtual = 0;
    private bool tutorialAtivo = false;
    private bool jaCompletou = false;
    private int frameInicio = -1;

    private PlayerMovement movimentoPlayer;
    private Animator animadorPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (jaCompletou && ativarApenasUmaVez) return;
        if (tutorialAtivo) return;

        if (other.CompareTag("Player"))
        {
            if (falas == null || falas.Length == 0) return;

            // Busca o script de movimentação e o Animator do Player automaticamente
            movimentoPlayer = other.GetComponent<PlayerMovement>();
            animadorPlayer = other.GetComponentInChildren<Animator>();

            ParalisarPlayer(true);

            tutorialAtivo = true;
            indiceFalaAtual = 0;
            frameInicio = Time.frameCount;

            if (painelTutorial != null) painelTutorial.SetActive(true);
            if (textoUI != null) textoUI.text = falas[0];
        }
    }

    private void Update()
    {
        if (!tutorialAtivo) return;

        // Evita pular a primeira fala no mesmo frame em que pisou na zona
        if (Time.frameCount <= frameInicio) return;

        // Avança a fala ao apertar E, Espaço, Enter ou clicar com o mouse
        if (Input.GetKeyDown(teclaAvancar) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetMouseButtonDown(0))
        {
            AvancarFala();
        }
    }

    private void AvancarFala()
    {
        indiceFalaAtual++;

        if (indiceFalaAtual < falas.Length)
        {
            if (textoUI != null) textoUI.text = falas[indiceFalaAtual];
        }
        else
        {
            FinalizarTutorial();
        }
    }

    private void FinalizarTutorial()
    {
        tutorialAtivo = false;
        jaCompletou = true;

        if (painelTutorial != null) painelTutorial.SetActive(false);

        ParalisarPlayer(false);

        if (ativarApenasUmaVez)
        {
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
    }

    private void ParalisarPlayer(bool paralisar)
    {
        if (movimentoPlayer != null)
        {
            movimentoPlayer.enabled = !paralisar;
        }

        if (animadorPlayer != null)
        {
            animadorPlayer.speed = paralisar ? 0f : 1f;
        }
    }
}