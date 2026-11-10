using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PainelGasesRadioativos : MonoBehaviour
{
    [Header("1. Oxigênio (Skill Check Estilo Manômetro)")]
    public float nivelOxigenio = 79.8f;
    public TextMeshProUGUI textoOxigenioUI;
    public TextMeshProUGUI textoRodadasQTEUI;
    public KeyCode teclaQTE = KeyCode.Space;
    public bool oxigenioNormalizado = false;

    [Header("Rodadas")]
    public int totalRodadasNecessarias = 3;
    private int rodadasConcluidas = 0;

    [Header("Configurações do Manômetro (Arco Superior: -90° a +90°)")]
    public RectTransform ponteiroQTE;          // Agulha do manômetro
    public RectTransform zonaSucessoQTE;       // Fatia verde de acerto
    public float velocidadePonteiro = 180f;
    public float anguloMinimoManometro = -90f;
    public float anguloMaximoManometro = 90f;

    private float anguloAtualPonteiro = -90f;
    private bool movendoParaDireita = true;

    private float anguloInicioZona;
    private float anguloFimZona;

    [Header("2. Gases Inflamáveis")]
    public Button botaoLiberarGases;
    public bool gasesLiberados = false;

    [Header("3. Contaminantes Tóxicos (Purga)")]
    public Slider sliderPurgaTorneira;
    public TextMeshProUGUI textoStatusPurgaUI;
    public bool purgaConcluida = false;

    [Header("Configurações Gerais de UI e Cena")]
    public GameObject painelCanvasUI;
    public string nomeCenaExplosao = "GameOver";
    private bool painelAberto = false;

    void Start()
    {
        if (painelCanvasUI != null)
            painelCanvasUI.SetActive(false);

        if (botaoLiberarGases != null)
            botaoLiberarGases.onClick.AddListener(LiberarGasesInflamaveis);

        ResetarManometro();
        AtualizarUI();
    }

    void Update()
    {
        if (!painelAberto) return;

        // 1. Movimento contínuo da agulha
        if (!oxigenioNormalizado)
        {
            MoverAgulhaManometro();

            if (Input.GetKeyDown(teclaQTE))
            {
                ValidarSkillCheck();
            }
        }

        // 3. Slider da Purga
        if (!purgaConcluida && sliderPurgaTorneira != null)
        {
            if (sliderPurgaTorneira.value >= 0.98f)
            {
                purgaConcluida = true;
                Debug.Log("<color=green>Purga Química concluída!</color>");
                AtualizarUI();
            }
        }
    }

    void MoverAgulhaManometro()
    {
        if (ponteiroQTE == null) return;

        // Oscilação entre o limite mínimo (-90°) e máximo (+90°)
        if (movendoParaDireita)
        {
            anguloAtualPonteiro += velocidadePonteiro * Time.deltaTime;
            if (anguloAtualPonteiro >= anguloMaximoManometro)
            {
                anguloAtualPonteiro = anguloMaximoManometro;
                movendoParaDireita = false;
            }
        }
        else
        {
            anguloAtualPonteiro -= velocidadePonteiro * Time.deltaTime;
            if (anguloAtualPonteiro <= anguloMinimoManometro)
            {
                anguloAtualPonteiro = anguloMinimoManometro;
                movendoParaDireita = true;
            }
        }

        // Aplica a rotação visual
        ponteiroQTE.localRotation = Quaternion.Euler(0, 0, -anguloAtualPonteiro);
    }

    void ResetarManometro()
    {
        anguloAtualPonteiro = anguloMinimoManometro;
        movendoParaDireita = true;

        if (ponteiroQTE != null)
            ponteiroQTE.localRotation = Quaternion.Euler(0, 0, -anguloMinimoManometro);

        SortearZonaSucesso();
    }

    void SortearZonaSucesso()
    {
        // Tamanho da zona de acerto (largura em graus)
        float tamanhoZona = Random.Range(25f, 35f);

        // Sorteia dentro da faixa do manômetro (-75° até +75° menos o tamanho da zona)
        anguloInicioZona = Random.Range(-75f, 75f - tamanhoZona);
        anguloFimZona = anguloInicioZona + tamanhoZona;

        if (zonaSucessoQTE != null)
        {
            zonaSucessoQTE.localRotation = Quaternion.Euler(0, 0, -anguloInicioZona);

            Image imgZona = zonaSucessoQTE.GetComponent<Image>();
            if (imgZona != null && imgZona.type == Image.Type.Filled)
            {
                imgZona.fillAmount = tamanhoZona / 360f;
            }
        }
    }

    void ValidarSkillCheck()
    {
        // Verifica se a agulha está dentro dos limites da zona
        if (anguloAtualPonteiro >= anguloInicioZona && anguloAtualPonteiro <= anguloFimZona)
        {
            rodadasConcluidas++;
            nivelOxigenio -= 10f;
            nivelOxigenio = Mathf.Max(nivelOxigenio, 0f);
            Debug.Log($"<color=green>Skill Check Perfeito! ({rodadasConcluidas}/{totalRodadasNecessarias})</color>");

            if (rodadasConcluidas >= totalRodadasNecessarias)
            {
                oxigenioNormalizado = true;
            }
            else
            {
                SortearZonaSucesso();
            }
        }
        else
        {
            nivelOxigenio += 10f;
            Debug.LogWarning($"<color=red>Skill Check Falhou!</color> Oxigênio: {nivelOxigenio:F1}%");

            if (nivelOxigenio >= 100f)
            {
                ExplodirSetor();
                return;
            }

            SortearZonaSucesso();
        }

        AtualizarUI();
    }

    public void LiberarGasesInflamaveis()
    {
        gasesLiberados = true;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (textoOxigenioUI != null)
        {
            textoOxigenioUI.text = "Oxigênio: " + nivelOxigenio.ToString("F1") + "%";
        }

        if (textoRodadasQTEUI != null)
        {
            textoRodadasQTEUI.text = oxigenioNormalizado ? "Status: NORMALIZADO" : $"Rodadas: {rodadasConcluidas}/{totalRodadasNecessarias}";
        }

        if (textoStatusPurgaUI != null)
        {
            textoStatusPurgaUI.text = purgaConcluida ? "Purga: CONCLUÍDA" : "Purga: ABRIR VÁLVULA";
        }
    }

    void ExplodirSetor()
    {
        Debug.LogError("OXIGÊNIO ATINGIU 100%! EXPLOSÃO NO SETOR!");
        SceneManager.LoadScene(nomeCenaExplosao);
    }

    public void AbrirPainel()
    {
        painelAberto = true;
        if (painelCanvasUI != null) painelCanvasUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        rodadasConcluidas = 0;
        oxigenioNormalizado = false;

        ResetarManometro();
        AtualizarUI();
    }

    public void FecharPainel()
    {
        painelAberto = false;
        if (painelCanvasUI != null) painelCanvasUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}