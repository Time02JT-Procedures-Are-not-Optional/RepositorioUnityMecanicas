using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [Header("UI da Barra Azul")]
    public Image barraAzulPreenchimento;

    [Header("Efeito de Borda Escura")]
    public CanvasGroup vinhetaCanvasGroup; // Arraste o EfeitoExaustao aqui
    public float velocidadeTransicao = 3f;

    [Header("Configurações")]
    public float totalTarefas = 5f;
    private float tarefasRestantes;

    private float alphaAlvo = 0f;

    void Start()
    {
        tarefasRestantes = totalTarefas;
        AtualizarUI();
    }

    void Update()
    {
        // Transição suave da transparência da vinheta nas bordas
        if (vinhetaCanvasGroup != null)
        {
            vinhetaCanvasGroup.alpha = Mathf.MoveTowards(vinhetaCanvasGroup.alpha, alphaAlvo, Time.deltaTime * velocidadeTransicao);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ConcluirTarefa();
        }
    }

    public void ConcluirTarefa()
    {
        if (tarefasRestantes > 0)
        {
            tarefasRestantes--;
            AtualizarUI();
            Debug.Log($"Tarefa concluída! Restam {tarefasRestantes} tarefas.");
        }
    }

    void AtualizarUI()
    {
        float percentual = tarefasRestantes / totalTarefas;

        // Atualiza a barra de preenchimento
        if (barraAzulPreenchimento != null)
        {
            barraAzulPreenchimento.fillAmount = percentual;
        }

        // Se a barra chegar a 50% ou menos (metade ou menos)
        if (percentual <= 0.5f)
        {
            // Calcula o quão escuro fica (quanto menor o percentual, mais escuro)
            // Em 50% o alpha fica leve, em 0% fica no máximo (1.0)
            alphaAlvo = Mathf.InverseLerp(0.5f, 0f, percentual);
        }
        else
        {
            alphaAlvo = 0f; // Totalmente transparente acima de 50%
        }
    }
}