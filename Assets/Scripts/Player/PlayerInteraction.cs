using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Importante para o suporte a textos da UI

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configurações de Interação")]
    public float interactionDistance = 10f;
    public Transform holdPoint;
    public LayerMask interactableLayer;

    [Header("Interface (UI)")]
    public TMP_Text substanceDescriptionText; // Arraste o objeto "New Text" para cá

    [Header("Configuração de Game Over")]
    public string gameOverSceneName = "GameOver";

    private SubstanceItem carriedItem;

    private void Start()
    {
        ClearUI();
    }

    private void Update()
    {
        Vector3 rayOrigin = GetRayOrigin();
        Vector3 rayDirection = transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * interactionDistance, Color.red);

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (carriedItem == null)
            {
                TryPickUp();
            }
            else
            {
                TryDropOrDeposit();
            }
        }
    }

    private Vector3 GetRayOrigin()
    {
        Vector3 basePos = holdPoint != null ? holdPoint.position : transform.position;
        return basePos + transform.forward * 0.5f;
    }

    private void TryPickUp()
    {
        Vector3 rayOrigin = GetRayOrigin();
        Vector3 rayDirection = transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance, interactableLayer))
        {
            SingleDispenser dispenser = hit.collider.GetComponent<SingleDispenser>();
            if (dispenser != null)
            {
                SubstanceItem item = dispenser.SpawnSubstance();
                if (item != null)
                {
                    carriedItem = item;
                    dispenser.ClearCurrentItem();
                    AttachItemToPlayer(carriedItem);

                    // Exibe a descrição da substância na tela
                    UpdateUI();
                }
            }
        }
    }

    private void TryDropOrDeposit()
    {
        Vector3 rayOrigin = GetRayOrigin();
        Vector3 rayDirection = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log($"[Raycast Hit] Atingiu: {hit.collider.gameObject.name}");

            // 1. TENTA APLICAR O RÓTULO
            LabelStation station = hit.collider.GetComponentInParent<LabelStation>();
            if (station == null) station = hit.collider.GetComponent<LabelStation>();

            if (station != null)
            {
                if (carriedItem == null)
                {
                    Debug.LogWarning("[Rotulador] Você precisa estar segurando um frasco para rotular!");
                    return;
                }

                if (carriedItem.data == null)
                {
                    Debug.LogError("[Rotulador] O frasco segurado não possui 'SubstanceData' atribuído!");
                    return;
                }

                bool aplicou = station.ApplyLabel(carriedItem);
                if (aplicou)
                {
                    Debug.Log($"[Sucesso] Frasco rotulado como: {carriedItem.assignedCategory}");
                    UpdateUI(); // Atualiza a interface com o novo rótulo
                }
                return;
            }

            // 2. TENTA DESCARTAR NO CONTAINER
            ColorBin bin = hit.collider.GetComponentInParent<ColorBin>();
            if (bin == null) bin = hit.collider.GetComponent<ColorBin>();

            if (bin != null && carriedItem != null && carriedItem.data != null)
            {
                if (carriedItem.assignedCategory == SubstanceCategory.None)
                {
                    Debug.LogWarning("[Container] Você precisa rotular o frasco antes de descartar!");
                    return;
                }

                if (carriedItem.assignedCategory == bin.binCategory)
                {
                    if (carriedItem.data.correctCategory == bin.binCategory)
                    {
                        Debug.Log($"[SUCESSO] Descarte correto no container {bin.binCategory}!");
                        Destroy(carriedItem.gameObject);
                        carriedItem = null;
                        ClearUI();
                    }
                    else
                    {
                        Debug.LogError($"[GAME OVER] Rótulo incorreto aplicado à substância!");
                        Destroy(carriedItem.gameObject);
                        carriedItem = null;
                        ClearUI();

                        SceneManager.LoadScene(gameOverSceneName);
                    }
                }
                else
                {
                    Debug.LogWarning($"[Recusado] O container {bin.binCategory} não aceita o rótulo {carriedItem.assignedCategory}.");
                }
            }
        }
    }


    // Restante do seu código (LabelStation, ColorBin, etc)...

    private void UpdateUI()
    {
        if (substanceDescriptionText == null) return;

        if (carriedItem != null && carriedItem.data != null)
        {
            string statusRotulo = carriedItem.assignedCategory == SubstanceCategory.None
                ? "<color=yellow>[Sem Rótulo]</color>"
                : $"<color=green>[Rótulo: {carriedItem.assignedCategory}]</color>";

            // Exibe o Nome, a Descrição/Propriedades da NR-26 e o Estado do Rótulo
            substanceDescriptionText.text = $"<b>{carriedItem.data.name}</b>\n{carriedItem.data.description}\n{statusRotulo}";
        }
    }

    private void ClearUI()
    {
        if (substanceDescriptionText != null)
        {
            substanceDescriptionText.text = ""; // Limpa a tela quando o jogador não estiver segurando nada
        }
    }

    private void AttachItemToPlayer(SubstanceItem item)
    {
        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = item.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }
   
}