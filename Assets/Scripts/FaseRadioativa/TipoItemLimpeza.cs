using UnityEngine;

public enum TipoItemLimpeza
{
    DetectorRadiacao,
    Neutralizante
}

public class ItemLimpezaTanque : MonoBehaviour
{
    public TipoItemLimpeza tipoItem;
    public TipoNeutralizante tipoNeutralizante; // Defina A, B ou C se for neutralizante

    [Header("Configuração de Interação")]
    public KeyCode teclaColeta = KeyCode.E;
    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(teclaColeta))
        {
            InventarioLimpeza inventario = FindAnyObjectByType<InventarioLimpeza>();
            if (inventario != null)
            {
                if (tipoItem == TipoItemLimpeza.DetectorRadiacao)
                {
                    inventario.ColetarDetector();
                    Destroy(gameObject); // Remove apenas o detector após coletar
                }
                else if (tipoItem == TipoItemLimpeza.Neutralizante)
                {
                    inventario.ColetarNeutralizante(tipoNeutralizante);
                    // O neutralizante não é destruído e permanece disponível no cenário
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) jogadorPerto = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) jogadorPerto = false;
    }
}