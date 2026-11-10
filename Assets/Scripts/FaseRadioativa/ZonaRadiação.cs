using UnityEngine;

public class ZonaRadioativa : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRadiation rad = other.GetComponent<PlayerRadiation>();
            if (rad != null)
            {
                rad.ReceberDanoRadioativo();
            }
        }
    }
}