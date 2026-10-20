using UnityEngine;

public enum SubstanceCategory
{
    None,     // Para itens recém-pegos que ainda não foram rotulados
    Red,      // Inflamável
    Yellow,   // Corrosivo
    Green     // Segurança
}
[CreateAssetMenu(fileName = "NewSubstance", menuName = "NR26/Substance")]
public class SubstanceData : ScriptableObject
{
    [TextArea(3, 5)]
    public string description;
    public SubstanceCategory correctCategory;
}