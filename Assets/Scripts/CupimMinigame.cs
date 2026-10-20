using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CupimMinigame : MonoBehaviour
{
    [Header("Configurações dos Cupins")]
    public GameObject painelUI;
    public List<GameObject> listaCupins;
    private int cupinsRestantes;

    [Header("Configurações do Pano e Sujeira")]
    public RectTransform panoUI;
    public List<GameObject> manchasSujeira;
    public float raioDeLimpeza = 60f; // Distância em pixels para apagar a mancha
    public bool panoAtivo = false;
    private int manchasAtivas = 0;

    [HideInInspector] public PainelEletrico painelEletricoRef;

    public void IniciarMinigame(PainelEletrico painel)
    {
        painelEletricoRef = painel;
        panoAtivo = false;
        manchasAtivas = 0;

        if (panoUI != null) panoUI.gameObject.SetActive(false);

        cupinsRestantes = listaCupins.Count;
        foreach (GameObject cupim in listaCupins)
        {
            if (cupim != null) cupim.SetActive(true);
        }

        foreach (GameObject mancha in manchasSujeira)
        {
            if (mancha != null) mancha.SetActive(false);
        }

        painelUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (panoAtivo && panoUI != null)
        {
            // 1. Move o Pano para a posição do mouse
            Vector2 posicaoMouse;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                painelUI.transform as RectTransform,
                Input.mousePosition,
                null,
                out posicaoMouse
            );
            panoUI.anchoredPosition = posicaoMouse;

            // 2. Checa distância entre o Pano e cada Mancha visível
            VerificarLimpezaManchas();
        }
    }

    public void ClicarNoCupim(GameObject cupimClicado)
    {
        int index = listaCupins.IndexOf(cupimClicado);

        cupimClicado.SetActive(false);
        cupinsRestantes--;

        if (index >= 0 && index < manchasSujeira.Count && manchasSujeira[index] != null)
        {
            manchasSujeira[index].SetActive(true);
            manchasAtivas++;
        }

        if (cupinsRestantes <= 0)
        {
            AtivarEtapaPano();
        }
    }

    private void AtivarEtapaPano()
    {
        panoAtivo = true;
        Cursor.visible = false;
        if (panoUI != null) panoUI.gameObject.SetActive(true);
    }

    private void VerificarLimpezaManchas()
    {
        for (int i = 0; i < manchasSujeira.Count; i++)
        {
            GameObject mancha = manchasSujeira[i];

            // Se a mancha existe e está ativa na tela
            if (mancha != null && mancha.activeSelf)
            {
                RectTransform rectMancha = mancha.GetComponent<RectTransform>();
                if (rectMancha != null)
                {
                    // Calcula a distância entre o centro do Pano e o centro da Mancha
                    float distancia = Vector2.Distance(panoUI.anchoredPosition, rectMancha.anchoredPosition);

                    // Se o pano chegou perto o suficiente da mancha, apaga ela
                    if (distancia <= raioDeLimpeza)
                    {
                        mancha.SetActive(false);
                        manchasAtivas--;

                        if (manchasAtivas <= 0)
                        {
                            ConcluirMinigame();
                        }
                    }
                }
            }
        }
    }

    private void ConcluirMinigame()
    {
        panoAtivo = false;
        painelUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (painelEletricoRef != null)
        {
            painelEletricoRef.FinalizarTask();
        }
    }
}