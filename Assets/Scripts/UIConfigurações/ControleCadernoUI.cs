using UnityEngine;

public class ControleCadernoUI : MonoBehaviour
{
    [Header("Componentes de UI")]
    public GameObject painelCaderno;

    private bool cadernoAberto = false;

    void Start()
    {
        if (painelCaderno != null)
        {
            painelCaderno.SetActive(false);
        }

        // Garante que o cursor do rato fica sempre visível e solto no jogo
        GarantirCursorLivre();
    }

    void Update()
    {
        // Atalho opcional pelo teclado (ex: 'J' ou 'TAB')
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Tab))
        {
            AlternarCaderno();
        }

        // Mantém o cursor livre mesmo que outro script tente travá-lo
        GarantirCursorLivre();
    }

    public void AlternarCaderno()
    {
        if (painelCaderno == null) return;

        cadernoAberto = !cadernoAberto;
        painelCaderno.SetActive(cadernoAberto);

        GarantirCursorLivre();
    }

    public void FecharCaderno()
    {
        cadernoAberto = false;
        if (painelCaderno != null)
            painelCaderno.SetActive(false);

        GarantirCursorLivre();
    }

    void GarantirCursorLivre()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}