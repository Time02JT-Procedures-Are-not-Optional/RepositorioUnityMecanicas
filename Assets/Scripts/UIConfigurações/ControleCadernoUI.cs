using UnityEngine;
using UnityEngine.UI;

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
    public Image cadernoImage;       // Componente Image do PainelCaderno que muda o desenho

    [Header("Botões das Setas (Opcional - oculta nos limites)")]
    public Button botaoSetaEsquerda;
    public Button botaoSetaDireita;

    [Header("Configuração de Páginas")]
    public PaginaCaderno[] paginas;

    private bool cadernoAberto = false;
    private int paginaAtual = 0;

    void Start()
    {
        FecharCaderno();
        GarantirCursorLivre();
    }

    void Update()
    {
        // Atalho pelo teclado ('J' ou 'TAB')
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

        // 1. Troca a imagem de fundo do caderno
        if (cadernoImage != null && paginas[indicePagina].spriteCaderno != null)
        {
            cadernoImage.sprite = paginas[indicePagina].spriteCaderno;
        }

        // 2. Ativa apenas o conteúdo da página atual (se houver objetos filhos)
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