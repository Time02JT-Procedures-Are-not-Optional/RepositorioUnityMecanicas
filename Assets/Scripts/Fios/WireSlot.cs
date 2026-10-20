using UnityEngine;


public class WireSlot : MonoBehaviour
{
    [Header("Validação da Entrada")]
    [Tooltip("Coloque o mesmo ID do Fio que deve encaixar aqui (ex: Fio1)")]
    public string requiredWireID = "Fio1";

    public bool isOccupied = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = isOccupied ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 2f);
    }
}