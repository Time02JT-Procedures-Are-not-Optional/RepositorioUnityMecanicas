using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PocaAcido : MonoBehaviour
{
    [Header("Tipo Exigido para Neutralizar")]
    public TipoNeutralizante neutralizanteCorreto; // Define no Inspector de cada poça (TipoA, TipoB, TipoC)

    [Header("Dano por Radiação")]
    public float danoPorSegundo = 33.4f;

    [Header("Configurações de UI e Teclas")]
    public TextMeshProUGUI textoLeituraDetectorUI;
    public string nomeCenaGameOver = "GameOver";
    public KeyCode teclaUsarNeutralizante = KeyCode.F; // Tecla F para evitar conflito com E (coletar)

    private bool jogadorSobreAPoca = false;
    private InventarioLimpeza inventarioJogador;
    private PlayerRadiation playerRadiationScript;

    void Start()
    {
        if (textoLeituraDetectorUI != null)
            textoLeituraDetectorUI.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorSobreAPoca = true;
            inventarioJogador = other.GetComponent<InventarioLimpeza>();
            playerRadiationScript = other.GetComponent<PlayerRadiation>();

            if (inventarioJogador != null && inventarioJogador.temDetector && textoLeituraDetectorUI != null)
            {
                textoLeituraDetectorUI.gameObject.SetActive(true);
                textoLeituraDetectorUI.text = $"[DETECTOR] Requer: Neutralizante {neutralizanteCorreto}";
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && playerRadiationScript != null)
        {
            playerRadiationScript.ReduzirRadiacao(danoPorSegundo * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorSobreAPoca = false;
            if (textoLeituraDetectorUI != null)
                textoLeituraDetectorUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (jogadorSobreAPoca && Input.GetKeyDown(teclaUsarNeutralizante))
        {
            TentarNeutralizar();
        }
    }

    void TentarNeutralizar()
    {
        if (inventarioJogador == null)
            inventarioJogador = FindAnyObjectByType<InventarioLimpeza>();

        if (inventarioJogador == null || inventarioJogador.neutralizanteEquipado == TipoNeutralizante.Nenhum)
        {
            Debug.LogWarning("[POÇA] Nenhum neutralizante equipado!");
            return;
        }

        Debug.Log($"[TENTATIVA] Poça exige: {neutralizanteCorreto} | Jogador usou: {inventarioJogador.neutralizanteEquipado}");

        // VERIFICAÇÃO ESTRITA: Só neutraliza se o equipado for EXATAMENTE o correto
        if (inventarioJogador.neutralizanteEquipado == neutralizanteCorreto)
        {
            Debug.Log("<color=green>[SUCESSO] Poça neutralizada com o item correto!</color>");

            inventarioJogador.ConsumirNeutralizanteAtual();

            if (textoLeituraDetectorUI != null)
                textoLeituraDetectorUI.gameObject.SetActive(false);

            Destroy(gameObject);
        }
        else
        {
            Debug.LogError($"<color=red>[EXPLOSÃO] Usou {inventarioJogador.neutralizanteEquipado} em poça {neutralizanteCorreto}!</color>");

            if (playerRadiationScript != null)
            {
                playerRadiationScript.ReduzirRadiacao(100f); // Zera a barra de radiação na hora
            }

            SceneManager.LoadScene(nomeCenaGameOver);
        }
    }
}