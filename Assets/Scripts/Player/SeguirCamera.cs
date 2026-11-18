using UnityEngine;

public class SeguirCameraOvercooked : MonoBehaviour
{
    [Header("Alvo")]
    public Transform alvoPlayer; // Arraste o seu Player aqui

    [Header("Offset da Câmera (Distância e Altura)")]
    // Como a escala do Player é 50x50x50, usaremos valores proporcionais
    public Vector3 offset = new Vector3(0f, 300f, -300f);

    [Header("Suavização")]
    public float velocidadeSuavizacao = 5f;

    void LateUpdate()
    {
        if (alvoPlayer == null) return;

        // 1. Posição ideal da câmara em relação ao Player
        Vector3 posicaoDesejada = alvoPlayer.position + offset;

        // 2. Transição suave
        transform.position = Vector3.Lerp(transform.position, posicaoDesejada, velocidadeSuavizacao * Time.deltaTime);

        // 3. Força a câmara a olhar SEMPRE para o centro do Player
        transform.LookAt(alvoPlayer.position + new Vector3(0f, 25f, 0f));
    }
}