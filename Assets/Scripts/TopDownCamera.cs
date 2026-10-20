using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Alvo da Câmera")]
    public Transform player; // O personagem que a câmera vai acompanhar

    [Header("Configurações de Movimento")]
    public float suavizacao = 5f; // Quão suave a câmera acompanha o movimento

    private Vector3 offset; // Guarda a distância original entre Câmera e Player

    void Start()
    {
        if (player != null)
        {
            // Calcula e salva a distância inicial entre a câmera e o player
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // Se não houver player associado, não faz nada para evitar erros no console
        if (player == null) return;

        // 1. Calcula a nova posição no eixo X mantendo Y e Z travados na posição da Câmera
        float posicaoX = player.position.x + offset.x;
        float posicaoY = transform.position.y; // Altura fixa
        float posicaoZ = player.position.z + offset.z; // Profundidade fixa

        Vector3 posicaoDesejada = new Vector3(posicaoX, posicaoY, posicaoZ);

        // 2. Desliza a câmera de forma suave entre a posição atual e a desejada
        transform.position = Vector3.Lerp(transform.position, posicaoDesejada, suavizacao * Time.deltaTime);
    }
}