using UnityEngine;

public class BotaoFisico : MonoBehaviour
{
    public GameController gameController;
    public KeyCode teclaInteracao = KeyCode.E;
    public float distanciaInteracao = 100f;
    public Transform playerTransform;

    void Update()
    {
        if (Input.GetKeyDown(teclaInteracao))
        {
            if (playerTransform == null)
            {
                Debug.LogError("ERRO: O campo Player Transform está VAZIO no Inspector do Cubo!");
                return;
            }

            float distancia = Vector3.Distance(transform.position, playerTransform.position);
      

            if (distancia <= distanciaInteracao)
            {
                if (gameController != null)
                {
                    gameController.ResetarTempo();
                    Debug.Log("SUCESSO: Tempo resetado a tempo!");
                }
                else
                {
                    Debug.LogError("ERRO: O campo Game Controller está VAZIO no Inspector!");
                }
            }
            else
            {
                Debug.LogWarning("Você está muito longe do botão! Distância necessária: " + distanciaInteracao);
            }
        }
    }
}