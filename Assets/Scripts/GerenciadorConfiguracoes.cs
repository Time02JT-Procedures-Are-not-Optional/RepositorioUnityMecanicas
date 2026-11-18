using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GerenciadorConfiguracoes : MonoBehaviour
{
    [Header("Painéis de Navegação")]
    public GameObject menuPrincipal;
    public GameObject painelConfiguracoes;

    [Header("Troca de Fundo do Caderno")]
    public Image cadernoFundoImage;
    public Sprite spriteCadernoMenu;
    public Sprite spriteCadernoConfig;

    [Header("Sliders da UI")]
    public Slider sliderMusica;
    public Slider sliderAmbiente;
    public Slider sliderFonte;

    [Header("Referências de Áudio (Opcional na tela de menu)")]
    public AudioSource fonteMusica;
    public AudioSource fonteAmbiente;

    [Header("Configuração do Tamanho da Fonte")]
    public float escalaMinimaFonte = 0.8f;
    public float escalaMaximaFonte = 1.4f;

    private TMP_Text[] todosTextosTMP;
    private float[] tamanhosOriginaisTMP;

    void Start()
    {
        todosTextosTMP = FindObjectsByType<TMP_Text>(FindObjectsInactive.Include);
        tamanhosOriginaisTMP = new float[todosTextosTMP.Length];
        for (int i = 0; i < todosTextosTMP.Length; i++)
        {
            tamanhosOriginaisTMP[i] = todosTextosTMP[i].fontSize;
        }

        if (sliderMusica != null) { sliderMusica.minValue = 0f; sliderMusica.maxValue = 1f; }
        if (sliderAmbiente != null) { sliderAmbiente.minValue = 0f; sliderAmbiente.maxValue = 1f; }
        if (sliderFonte != null) { sliderFonte.minValue = escalaMinimaFonte; sliderFonte.maxValue = escalaMaximaFonte; }

        float volMusica = PlayerPrefs.GetFloat("VolMusica", 1f);
        float volAmbiente = PlayerPrefs.GetFloat("VolAmbiente", 1f);
        float multFonte = PlayerPrefs.GetFloat("MultFonte", 1f);

        if (sliderMusica != null) sliderMusica.value = volMusica;
        if (sliderAmbiente != null) sliderAmbiente.value = volAmbiente;
        if (sliderFonte != null) sliderFonte.value = multFonte;

        AlterarVolumeMusica(volMusica);
        AlterarVolumeAmbiente(volAmbiente);
        AlterarTamanhoFonte(multFonte);

        if (sliderMusica != null) sliderMusica.onValueChanged.AddListener(AlterarVolumeMusica);
        if (sliderAmbiente != null) sliderAmbiente.onValueChanged.AddListener(AlterarVolumeAmbiente);
        if (sliderFonte != null) sliderFonte.onValueChanged.AddListener(AlterarTamanhoFonte);
    }
    void Update()
    {
        // Se o painel de configurações estiver aberto e o jogador apertar ESC, volta ao menu
        if (painelConfiguracoes != null && painelConfiguracoes.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            FecharConfiguracoes();
        }
    }
    public void AbrirConfiguracoes()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(false);
        if (painelConfiguracoes != null) painelConfiguracoes.SetActive(true);

        if (cadernoFundoImage != null && spriteCadernoConfig != null)
        {
            cadernoFundoImage.sprite = spriteCadernoConfig;
        }
    }

    public void FecharConfiguracoes()
    {
        PlayerPrefs.Save();
        if (painelConfiguracoes != null) painelConfiguracoes.SetActive(false);
        if (menuPrincipal != null) menuPrincipal.SetActive(true);

        if (cadernoFundoImage != null && spriteCadernoMenu != null)
        {
            cadernoFundoImage.sprite = spriteCadernoMenu;
        }
    }

    public void AlterarVolumeMusica(float valor)
    {
        PlayerPrefs.SetFloat("VolMusica", valor);
        if (fonteMusica != null) fonteMusica.volume = valor;
    }

    public void AlterarVolumeAmbiente(float valor)
    {
        PlayerPrefs.SetFloat("VolAmbiente", valor);
        if (fonteAmbiente != null) fonteAmbiente.volume = valor;
    }

    public void AlterarTamanhoFonte(float multiplicador)
    {
        PlayerPrefs.SetFloat("MultFonte", multiplicador);
        if (todosTextosTMP == null) return;

        for (int i = 0; i < todosTextosTMP.Length; i++)
        {
            if (todosTextosTMP[i] != null)
            {
                todosTextosTMP[i].fontSize = tamanhosOriginaisTMP[i] * multiplicador;
            }
        }
    }
}