using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControleCadernoUI : MonoBehaviour
{
    [System.Serializable]
    public class PaginaCaderno
    {
        public string nomePagina;
        public Sprite spriteCaderno;      // Ex: Tarefas-Dia01, Tarefas-INVENT-Dia02_2
        public GameObject conteudoPainel; // Conteúdo UI específico da página (opcional)
    }

    [Header("Aba Verde - Bilhetes Encontrados")]
    public GameObject paginaBilhetes;          // Painel com o fundo bilhetesVAZIO-01
    public GameObject[] imagensBilhetesUI;     // Os 7 objetos Image posicionados sobre as silhuetas

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
    private bool bilhetesAberto = false;
    private bool menuLateralAberto = false;
    private int paginaAtual = 0;

    void Start()
    {
        FecharTudo();

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
        // A tecla TAB abre e fecha EXCLUSIVAMENTE o Menu Lateral
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AlternarMenuLateral();
        }

        GarantirCursorLivre();
    }

    // --- CONTROLE DA ABA VERDE (BILHETES) ---

    public void AbrirAbaVerde()
    {
        // Se os bilhetes já estiverem abertos, clicar na aba verde fecha tudo
        if (bilhetesAberto)
        {
            FecharTudo();
            return;
        }

        // Esconde o caderno de tarefas (Aba Amarela) e mostra apenas os bilhetes (Aba Verde)
        cadernoAberto = false;
        if (painelCaderno != null) painelCaderno.SetActive(false);

        bilhetesAberto = true;
        if (paginaBilhetes != null) paginaBilhetes.SetActive(true);

        GarantirCursorLivre();
    }

    // Método chamado pelo objeto 3D quando o jogador coleta um bilhete no mapa
    public void DesbloquearBilhete(int indiceBilhete)
    {
        if (imagensBilhetesUI != null && indiceBilhete >= 0 && indiceBilhete < imagensBilhetesUI.Length)
        {
            if (imagensBilhetesUI[indiceBilhete] != null)
            {
                imagensBilhetesUI[indiceBilhete].SetActive(true);
            }
        }
    }

    // --- CONTROLE DO MENU LATERAL (TECLA TAB) ---

    public void AlternarMenuLateral()
    {
        if (menuLateral == null) return;

        menuLateralAberto = !menuLateralAberto;
        menuLateral.SetActive(menuLateralAberto);

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

        // Se a aba verde (bilhetes) estava aberta, fecha ela e abre a aba amarela direto
        if (bilhetesAberto)
        {
            bilhetesAberto = false;
            if (paginaBilhetes != null) paginaBilhetes.SetActive(false);

            cadernoAberto = true;
            painelCaderno.SetActive(true);
            AbrirPagina(paginaAtual);
            GarantirCursorLivre();
            return;
        }

        // Comportamento normal de abrir/fechar a aba amarela
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
        FecharTudo();
    }

    public void FecharTudo()
    {
        cadernoAberto = false;
        bilhetesAberto = false;

        if (painelCaderno != null)
            painelCaderno.SetActive(false);

        if (paginaBilhetes != null)
            paginaBilhetes.SetActive(false);

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