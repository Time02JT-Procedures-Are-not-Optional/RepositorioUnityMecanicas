using UnityEngine;

public class PlayerWire : MonoBehaviour
{
    [Header("Configurações do Fio")]
    public string wireID = "Fio1"; // Identificador único deste fio
    public LineRenderer lineRenderer;
    public Collider wireCollider;

    [Header("Estado")]
    public bool isConnected = false;
    public Transform currentAnchor;
    public WireSlot connectedSlot;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (wireCollider == null) wireCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (currentAnchor != null)
        {
            transform.position = currentAnchor.position;
            AtualizarLinha();
        }
    }

    public void PickUp(Transform hand)
    {
        currentAnchor = hand;
        if (wireCollider != null) wireCollider.enabled = false;
        if (lineRenderer != null) lineRenderer.enabled = true;
    }

    public void Drop()
    {
        currentAnchor = null;
        transform.position = originalPosition;
        if (wireCollider != null) wireCollider.enabled = true;
        if (lineRenderer != null) lineRenderer.enabled = false;
    }

    public bool TryConnectToSlot(WireSlot slot)
    {
        if (slot == null || slot.isOccupied) return false;

        currentAnchor = slot.transform;
        transform.position = slot.transform.position;
        isConnected = true;
        connectedSlot = slot;
        slot.isOccupied = true;

        if (wireCollider != null) wireCollider.enabled = true;
        AtualizarLinha();
        return true;
    }

    private void AtualizarLinha()
    {
        if (lineRenderer != null && lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, originalPosition);
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}