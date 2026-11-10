using UnityEngine;

public class CaixaDisjuntor3D : MonoBehaviour
{
    [Header("Referências 3D dos Disjuntores")]
    public GameObject disjuntorQueimado3D;
    public GameObject disjuntorNovo3D;
    public Transform alavancaChave3D;

    [Header("Estados do Painel")]
    public bool disjuntorQueimadoRemovido = false;
    public bool disjuntorNovoInstalado = false;
    public bool disjuntorLigado = false;

    private bool jogadorNaArea = false;
    private PlayerWireHolder playerHolder;

    private void Start()
    {
        if (disjuntorNovo3D != null) disjuntorNovo3D.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNaArea = true;
            playerHolder = other.GetComponentInParent<PlayerWireHolder>();
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
            InteragirComCaixa();
        }
    }

    // Função que checa se TODOS os WireSlots da cena foram preenchidos
    private bool ChecarSeTodosFiosEstaoConectados()
    {
        WireSlot[] slots = FindObjectsByType<WireSlot>();

        // Se não houver nenhum slot na cena, libera o acesso
        if (slots.Length == 0) return true;

        foreach (WireSlot slot in slots)
        {
            if (!slot.isOccupied)
            {
                return false; // Se encontrar ao menos 1 slot vazio, bloqueia
            }
        }

        return true; // Todos os slots estão ocupados!
    }

    private void InteragirComCaixa()
    {
        // VALIDAÇÃO DOS FIOS: Checa diretamente se as tomadas estão ocupadas
        if (!ChecarSeTodosFiosEstaoConectados())
        {
            Debug.LogWarning("BLOQUEADO: Conecte todos os fios 3D nas tomadas antes de mexer no disjuntor!");
            return;
        }

        // ETAPA 1: Remover Disjuntor Queimado
        if (!disjuntorQueimadoRemovido)
        {
            disjuntorQueimadoRemovido = true;
            if (disjuntorQueimado3D != null) disjuntorQueimado3D.SetActive(false);
            Debug.Log("ETAPA 1: Disjuntor queimado removido!");
            return;
        }

        // ETAPA 2: Colocar Disjuntor Novo
        if (disjuntorQueimadoRemovido && !disjuntorNovoInstalado)
        {
            bool temItem = (playerHolder != null) && playerHolder.temDisjuntorNovo;

            if (temItem)
            {
                disjuntorNovoInstalado = true;
                if (disjuntorNovo3D != null) disjuntorNovo3D.SetActive(true);
                Debug.Log("ETAPA 2: Disjuntor novo instalado no trilho!");
            }
            else
            {
                Debug.LogWarning("ATENÇÃO: Você precisa coletar o disjuntor novo no mapa primeiro!");
            }
            return;
        }

        // ETAPA 3: Ligar a Chave
        if (disjuntorNovoInstalado && !disjuntorLigado)
        {
            disjuntorLigado = true;
            if (alavancaChave3D != null)
            {
                alavancaChave3D.Rotate(45f, 0, 0);
            }
            Debug.Log("ETAPA 3: Chave ligada! Task concluída com sucesso.");
        }
    }
}