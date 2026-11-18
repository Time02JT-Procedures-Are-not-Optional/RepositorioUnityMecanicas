using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControleCadernoUI : MonoBehaviour
{
    [System.Serializable]
    public class PaginaCaderno
    {
        public string nomePagina;
        public Sprite spriteCaderno;     // Ex: Tarefas-Dia01, Tarefas-INVENT-Dia02_2
        public GameObject conteudoPainel; // Conteúdo UI específico da página (opcional)
    }

    [Header("Componentes de UI")]
    public GameObject painelCaderno;
    public GameObject menuLateral;   // Arraste o objeto do Menu Lateral aqui
    public Image cadernoImage;       // Componente Image do PainelCaderno que muda o desenho

    [Header("Botões das Setas (Opcional - oculta nos limites)")]
    public Button botaoSetaEsquerda;
    public Button botaoSetaDireita;

    [Header("Configuração de Páginas")]
    public PaginaCaderno[] paginas;

    private bool cadernoAberto = false;
    private bool menuLateralAberto = false;
    private int paginaAtual = 0;

    void Start()
    {
        FecharCaderno();

        // Garante que o menu lateral começa fechado
        if (menuLateral != null)
        {
            menuLateralAberto = false;
            menuLateral.SetActive(false);
        }

        GarantirCursorLivre();
    }

    void Update()
    {
        // A tecla TAB agora abre e fecha EXCLUSIVAMENTE o Menu Lateral
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AlternarMenuLateral();
        }

        // Mantém o cursor livre mesmo que outro script tente travá-lo
        GarantirCursorLivre();
    }

    // --- CONTROLE DO MENU LATERAL (TECLA TAB) ---

    public void AlternarMenuLateral()
    {
        if (menuLateral == null) return;

        menuLateralAberto = !menuLateralAberto;
        menuLateral.SetActive(menuLateralAberto);

        // Limpa a seleção de botões para o Tab não ficar preso na navegação da UI
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        GarantirCursorLivre();
    }

    // --- CONTROLE DO CADERNO (CLIQUE NA ABA AMARELA) ---

    public void AlternarCaderno()
    {
        if (painelCaderno == null) return;

        cadernoAberto = !cadernoAberto;
        painelCaderno.SetActive(cadernoAberto);

        if (cadernoAberto)
        {
            AbrirPagina(paginaAtual);
        }

        GarantirCursorLivre();
    }

    public void FecharCaderno()
    {
        cadernoAberto = false;

        if (painelCaderno != null)
            painelCaderno.SetActive(false);

        GarantirCursorLivre();
    }

    // --- CONTROLE DE PÁGINAS E SETAS ---

    public void AbrirPagina(int indicePagina)
    {
        if (paginas == null || indicePagina < 0 || indicePagina >= paginas.Length) return;

        paginaAtual = indicePagina;

        if (cadernoImage != null && paginas[indicePagina].spriteCaderno != null)
        {
            cadernoImage.sprite = paginas[indicePagina].spriteCaderno;
        }

        for (int i = 0; i < paginas.Length; i++)
        {
            if (paginas[i].conteudoPainel != null)
            {
                paginas[i].conteudoPainel.SetActive(i == indicePagina);
            }
        }

        AtualizarVisibilidadeSetas();
    }

    public void ProximaPagina()
    {
        if (paginaAtual < paginas.Length - 1)
        {
            AbrirPagina(paginaAtual + 1);
        }
    }

    public void PaginaAnterior()
    {
        if (paginaAtual > 0)
        {
            AbrirPagina(paginaAtual - 1);
        }
    }

    void AtualizarVisibilidadeSetas()
    {
        if (botaoSetaEsquerda != null)
            botaoSetaEsquerda.gameObject.SetActive(paginaAtual > 0);

        if (botaoSetaDireita != null)
            botaoSetaDireita.gameObject.SetActive(paginaAtual < paginas.Length - 1);
    }

    void GarantirCursorLivre()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}