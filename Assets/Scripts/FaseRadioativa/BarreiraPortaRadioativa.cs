using System.Collections;
using UnityEngine;
using TMPro;

public class BarreiraPortaRadioativa : MonoBehaviour
{
    [Header("Referência ao NPC Tiny")]
    public NPCTinyPET scriptTiny;

    [Header("UI de Aviso da Barreira")]
    public GameObject painelAvisoBarreira;
    public TextMeshProUGUI textoAvisoBarreira;

    [Header("Configurações da Barreira")]
    public float forcaEmpurrao = 3.5f;
    public float tempoAviso = 2.5f;

    [TextArea(2, 3)]
    public string mensagemSemPET = "Tiny: Ei! Fale comigo primeiro para assinar a PET antes de entrar!";

    private bool exibindoAviso = false;

    private void Start()
    {
        if (painelAvisoBarreira != null) painelAvisoBarreira.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChecarEProcessarBarreira(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChecarEProcessarBarreira(other.gameObject);
        }
    }

    void ChecarEProcessarBarreira(GameObject jogador)
    {
        if (scriptTiny != null && scriptTiny.petAssinada)
        {
            // Se a PET já foi assinada, desativa a barreira imediatamente
            gameObject.SetActive(false);
            return;
        }

        // Se AINDA NÃO assinou a PET: empurra para trás e avisa
        if (scriptTiny != null && !scriptTiny.petAssinada)
        {
            EmpurrarJogadorParaTras(jogador);

            if (!exibindoAviso)
            {
                StartCoroutine(ExibirAviso());
            }
        }
    }

    void EmpurrarJogadorParaTras(GameObject jogador)
    {
        Vector3 direcaoRecuo = -jogador.transform.forward;
        direcaoRecuo.y = 0;

        CharacterController controller = jogador.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.Move(direcaoRecuo * forcaEmpurrao);
        }
        else
        {
            Rigidbody rb = jogador.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direcaoRecuo * forcaEmpurrao, ForceMode.Impulse);
            }
        }
    }

    IEnumerator ExibirAviso()
    {
        exibindoAviso = true;

        if (painelAvisoBarreira != null)
        {
            painelAvisoBarreira.SetActive(true);
            if (textoAvisoBarreira != null) textoAvisoBarreira.text = mensagemSemPET;
        }

        yield return new WaitForSeconds(tempoAviso);

        if (painelAvisoBarreira != null)
        {
            painelAvisoBarreira.SetActive(false);
        }

        exibindoAviso = false;
    }
}