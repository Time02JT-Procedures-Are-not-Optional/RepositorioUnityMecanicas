using UnityEngine;

public class SubstanceItem : MonoBehaviour
{
    public SubstanceData data;

    [HideInInspector]
    public SubstanceCategory assignedCategory = SubstanceCategory.None;
}