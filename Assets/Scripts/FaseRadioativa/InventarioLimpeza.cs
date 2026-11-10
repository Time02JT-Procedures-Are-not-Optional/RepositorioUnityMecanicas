using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InventarioLimpeza : MonoBehaviour
{
    public bool temDetector = false;
    public List<TipoNeutralizante> neutralizantesPossuidos = new List<TipoNeutralizante>();
    public TipoNeutralizante neutralizanteEquipado = TipoNeutralizante.Nenhum;

    [Header("UI do Inventário")]
    public TextMeshProUGUI textoDetectorUI;
    public TextMeshProUGUI textoNeutralizanteUI;

    void Start()
    {
        AtualizarUI();
    }

    void Update()
    {
        // Troca manual de itens pelas teclas 1, 2 e 3
        if (Input.GetKeyDown(KeyCode.Alpha1) && neutralizantesPossuidos.Contains(TipoNeutralizante.TipoA))
        {
            EquiparNeutralizante(TipoNeutralizante.TipoA);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && neutralizantesPossuidos.Contains(TipoNeutralizante.TipoB))
        {
            EquiparNeutralizante(TipoNeutralizante.TipoB);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && neutralizantesPossuidos.Contains(TipoNeutralizante.TipoC))
        {
            EquiparNeutralizante(TipoNeutralizante.TipoC);
        }
    }

    // --- FUNÇÕES QUE CORRIGEM O ERRO CS1061 ---

    // Chamado quando o jogador coleta o Detector no chão
    public void ColetarDetector()
    {
        temDetector = true;
        Debug.Log("<color=green>[INVENTÁRIO] Detector de Radiação Adquirido!</color>");
        AtualizarUI();
    }

    // Chamado quando o jogador coleta um Neutralizante no chão
    public void ColetarNeutralizante(TipoNeutralizante tipo)
    {
        if (!neutralizantesPossuidos.Contains(tipo))
        {
            neutralizantesPossuidos.Add(tipo);
        }

        // Equipado automaticamente ao coletar
        EquiparNeutralizante(tipo);
    }

    // ------------------------------------------

    public void EquiparNeutralizante(TipoNeutralizante tipo)
    {
        neutralizanteEquipado = tipo;
        Debug.Log($"<color=cyan>[INVENTÁRIO] Item Equipado Alterado Para: {neutralizanteEquipado}</color>");
        AtualizarUI();
    }

    public void ConsumirNeutralizanteAtual()
    {
        if (neutralizantesPossuidos.Contains(neutralizanteEquipado))
        {
            neutralizantesPossuidos.Remove(neutralizanteEquipado);
        }

        if (neutralizantesPossuidos.Count > 0)
        {
            neutralizanteEquipado = neutralizantesPossuidos[0];
        }
        else
        {
            neutralizanteEquipado = TipoNeutralizante.Nenhum;
        }

        AtualizarUI();
    }

    public void AtualizarUI()
    {
        if (textoDetectorUI != null)
            textoDetectorUI.text = temDetector ? "Detector: ADQUIRIDO" : "Detector: NÃO POSSUI";

        if (textoNeutralizanteUI != null)
            textoNeutralizanteUI.text = "Neutralizante Ativo: " + neutralizanteEquipado.ToString();
    }
}