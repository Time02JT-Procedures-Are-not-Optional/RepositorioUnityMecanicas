using UnityEngine;

public class BilhetesColetaveis : MonoBehaviour
{
    [Header("Configuração do Bilhete")]
    [Tooltip("0 a 3 = Dia 1 | 4 a 6 = Dia 2")]
    public int indiceDoBilhete = 0;

    [Header("Interação")]
    public bool coletarAoEncostar = false;
    public KeyCode teclaInteracao = KeyCode.E;

    private bool jogadorPerto = false;
    private ControleCadernoUI controleCaderno;

    void Start()
    {
        controleCaderno = FindAnyObjectByType<ControleCadernoUI>();
    }

    void Update()
    {
        if (!coletarAoEncostar && jogadorPerto && Input.GetKeyDown(teclaInteracao))
        {
            Coletar();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            if (coletarAoEncostar)
            {
                Coletar();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }

    void Coletar()
    {
        if (controleCaderno != null)
        {
            controleCaderno.DesbloquearBilhete(indiceDoBilhete);
        }

        // Remove o bilhete 3D do mapa após coletar
        Destroy(gameObject);
    }
}