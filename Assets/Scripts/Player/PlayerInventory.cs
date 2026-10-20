using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Configurações Gerais de Itens")]
    public int itensColetados = 0;
    public int itensNecessarios = 5;

    [Header("Itens da Task dos Cupins")]
    public bool temVeneno = false;
    public bool temEPIEletrico = false;

    // Método original mantido para itens genéricos/comuns
    public void AdicionarItem()
    {
        itensColetados++;
        Debug.Log("Item coletado! Total: " + itensColetados);
    }

    // Método para os itens especiais das tarefas
    public void AdicionarItemEspecial(string tipoItem)
    {
        // Incrementa a contagem geral de itens
        AdicionarItem();

        // Valida o tipo específico sem dar conflito com outros puzzles
        if (tipoItem == "Veneno")
        {
            temVeneno = true;
            Debug.Log("Veneno adicionado ao inventário!");
        }
        else if (tipoItem == "EPI_Eletrico2")
        {
            temEPIEletrico = true;
            Debug.Log("EPI Elétrica equipada com sucesso!");
        }
    }
}