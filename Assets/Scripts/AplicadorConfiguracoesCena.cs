using UnityEngine;
using TMPro;

public class AplicadorConfiguracoesCena : MonoBehaviour
{
    [Header("Fontes de Áudio da Fase")]
    public AudioSource[] musicasDaCena;
    public AudioSource[] sonsAmbienteEMecanicas;

    void Start()
    {
        float volMusica = PlayerPrefs.GetFloat("VolMusica", 1f);
        float volAmbiente = PlayerPrefs.GetFloat("VolAmbiente", 1f);
        float multFonte = PlayerPrefs.GetFloat("MultFonte", 1f);

        foreach (AudioSource audio in musicasDaCena)
        {
            if (audio != null) audio.volume = volMusica;
        }

        foreach (AudioSource audio in sonsAmbienteEMecanicas)
        {
            if (audio != null) audio.volume = volAmbiente;
        }

        TMP_Text[] textos = FindObjectsByType<TMP_Text>(FindObjectsInactive.Include);
        foreach (TMP_Text txt in textos)
        {
            txt.fontSize *= multFonte;
        }
    }
}