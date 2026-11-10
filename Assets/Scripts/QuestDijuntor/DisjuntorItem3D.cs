using UnityEngine;

public class DisjuntorItem3D : MonoBehaviour
{
    private bool jogadorNaArea = false;
    private PlayerWireHolder playerHolder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = true;
            playerHolder = other.GetComponent<PlayerWireHolder>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = false;
            playerHolder = null;
        }
    }

    private void Update()
    {
        if (jogadorNaArea && Input.GetKeyDown(KeyCode.E))
        {
            ColetarDisjuntor();
        }
    }

    private void ColetarDisjuntor()
    {
        if (playerHolder != null)
        {
            // Ativa a flag no player indicando que ele tem o disjuntor novo
            playerHolder.temDisjuntorNovo = true;
            Debug.Log("Disjuntor Novo coletado!");
            gameObject.SetActive(false); // Esconde o item do chão
        }
    }
}