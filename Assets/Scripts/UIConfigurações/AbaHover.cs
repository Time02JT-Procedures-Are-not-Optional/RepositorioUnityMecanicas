using UnityEngine;
using UnityEngine.EventSystems;

public class AbaHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Deslocamento")]
    [Tooltip("Distância em pixels que a aba vai avançar para a direita.")]
    public float distanciaAvanco = 30f;
    public float velocidadeAnimacao = 10f;

    private RectTransform rectTransform;
    private Vector2 posicaoOriginal;
    private Vector2 posicaoAlvo;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        posicaoOriginal = rectTransform.anchoredPosition;
        posicaoAlvo = posicaoOriginal;
    }

    void Update()
    {
        // Movimenta suavemente até a posição alvo
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, posicaoAlvo, Time.deltaTime * velocidadeAnimacao);
    }

    // Chamado quando o mouse ENTRA na aba
    public void OnPointerEnter(PointerEventData eventData)
    {
        posicaoAlvo = posicaoOriginal + new Vector2(distanciaAvanco, 0f);
    }

    // Chamado quando o mouse SAI da aba
    public void OnPointerExit(PointerEventData eventData)
    {
        posicaoAlvo = posicaoOriginal;
    }
}