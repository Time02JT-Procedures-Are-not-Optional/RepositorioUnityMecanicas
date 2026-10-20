using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para trocar de cena

public class PlayerWireHolder : MonoBehaviour
{
    [Header("Configurações de Interação")]
    public float interactRadius = 25f;
    public Transform handPosition;

    [Header("Configuração de Game Over")]
    [Tooltip("Digite aqui o NOME EXATO da sua cena de GameOver")]
    public string gameOverSceneName = "GameOver";

    private PlayerWire currentWire = null;

    public int totalFiosNecessarios = 2;
    private int fiosConectadosAtualmente = 0;

    public bool todosFiosConectados = false;
    public bool temDisjuntorNovo = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentWire == null)
            {
                TryPickUpWire();
            }
            else
            {
                TryConnectWire();
            }
        }
    }

    private void TryPickUpWire()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Wire"))
            {
                PlayerWire wire = hit.GetComponent<PlayerWire>();
                if (wire != null && !wire.isConnected)
                {
                    currentWire = wire;
                    currentWire.PickUp(handPosition != null ? handPosition : transform);
                    Debug.Log($"Pegou o fio: {wire.wireID}");
                    break;
                }
            }
        }
    }

    private void TryConnectWire()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
        bool encostouEmTomada = false;

        foreach (Collider hit in hits)
        {
            WireSlot slot = hit.GetComponent<WireSlot>();
            if (slot != null && !slot.isOccupied)
            {
                encostouEmTomada = true;

                // Checa se o ID do Fio bate com o ID exigido pela Entrada
                if (currentWire.wireID == slot.requiredWireID)
                {
                    currentWire.TryConnectToSlot(slot);
                    Debug.Log($"Sucesso! {currentWire.wireID} conectado na {slot.name}.");
                    currentWire = null;
                }
                else
                {
                    Debug.LogError($"FIO ERRADO! {currentWire.wireID} foi colocado na {slot.name}. Mudando para a cena de Game Over...");

                    // Carrega a cena separada de GameOver
                    SceneManager.LoadScene(gameOverSceneName);
                }
                break;
            }
        }

        // Se apertar E longe de qualquer tomada, solta o fio de volta
        if (!encostouEmTomada && currentWire != null)
        {
            currentWire.Drop();
            currentWire = null;
        }
    }

    public void RegistrarFioConectado()
    {
        fiosConectadosAtualmente++;
        if (fiosConectadosAtualmente >= totalFiosNecessarios)
        {
            todosFiosConectados = true;
            Debug.Log("Todos os fios 3D foram conectados com sucesso!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}