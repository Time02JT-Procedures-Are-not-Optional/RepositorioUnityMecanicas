using UnityEngine;

public class LabelStation : MonoBehaviour
{
    [Header("Tipo de Rótulo que esta bancada aplica")]
    public SubstanceCategory labelColorToApply;

    public bool ApplyLabel(SubstanceItem item)
    {
        if (item == null)
        {
            Debug.LogError("[LabelStation] O item enviado é NULO!");
            return false;
        }

        item.assignedCategory = labelColorToApply;
        Debug.Log($"[LabelStation] Rótulo {labelColorToApply} aplicado no item {item.gameObject.name}!");

        Renderer itemRenderer = item.GetComponent<Renderer>();
        if (itemRenderer != null)
        {
            if (labelColorToApply == SubstanceCategory.Red) itemRenderer.material.color = Color.red;
            else if (labelColorToApply == SubstanceCategory.Yellow) itemRenderer.material.color = Color.yellow;
            else if (labelColorToApply == SubstanceCategory.Green) itemRenderer.material.color = Color.green;
        }

        return true;
    }
}