using System.Collections.Generic;
using UnityEngine;

public class SingleDispenser : MonoBehaviour
{
    [Header("Configurações")]
    public GameObject substancePrefab; // Prefab 3D do frasco
    public List<SubstanceData> availableSubstances = new List<SubstanceData>(); // Lista com as 8 substâncias

    private List<SubstanceData> remainingSubstances;
    private SubstanceItem currentSpawnedItem;

    private void Awake()
    {
        // Cria uma cópia da lista para podermos remover conforme for tirando
        remainingSubstances = new List<SubstanceData>(availableSubstances);
    }

    public SubstanceItem SpawnSubstance()
    {
        // Se já tiver um frasco no dispenser ou se acabaram as substâncias
        if (currentSpawnedItem != null || remainingSubstances.Count == 0)
        {
            if (remainingSubstances.Count == 0)
            {
                Debug.Log("Todas as 8 substâncias já foram dispensadas!");
            }
            return null;
        }

        // Sorteia um índice aleatório
        int randomIndex = Random.Range(0, remainingSubstances.Count);
        SubstanceData selectedData = remainingSubstances[randomIndex];

        // Instancia o frasco no dispenser
        GameObject newObj = Instantiate(substancePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        currentSpawnedItem = newObj.GetComponent<SubstanceItem>();
        currentSpawnedItem.data = selectedData;

        // Remove da lista para não repetir
        remainingSubstances.RemoveAt(randomIndex);

        return currentSpawnedItem;
    }

    // Chamado quando o jogador pega o item do dispenser
    public void ClearCurrentItem()
    {
        currentSpawnedItem = null;
    }
}