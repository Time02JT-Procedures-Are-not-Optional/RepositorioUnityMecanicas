using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum TipoItem { Comum, Veneno, EPI_Eletrico, EPI_Eletrico2 }
    public TipoItem tipo = TipoItem.Comum;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventario = other.GetComponent<PlayerInventory>();
            if (inventario != null)
            {
                if (tipo == TipoItem.Comum)
                {
                    inventario.AdicionarItem();
                }
                else
                {
                    inventario.AdicionarItemEspecial(tipo.ToString());
                }

                Destroy(gameObject);
            }
        }
    }
}