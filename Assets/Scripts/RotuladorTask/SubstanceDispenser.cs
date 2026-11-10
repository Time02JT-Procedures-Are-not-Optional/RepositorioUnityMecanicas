using UnityEngine;

public class SubstanceDispenser : MonoBehaviour
{
    public GameObject substancePrefab;
    public SubstanceData substanceDataToSpawn;

    public SubstanceItem SpawnSubstance()
    {
        GameObject newObj = Instantiate(substancePrefab);
        SubstanceItem item = newObj.GetComponent<SubstanceItem>();
        item.data = substanceDataToSpawn;
        return item;
    }
}
