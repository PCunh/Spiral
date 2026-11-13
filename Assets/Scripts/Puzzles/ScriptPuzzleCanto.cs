
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPuzzleCanto : MonoBehaviour
{
    [Header("Sequencia de cinco letras")]
    [SerializeField] private Button[] slots = new Button[5];

    [Header("Cinco botoes das letras")]
    [SerializeField] private Button[] botoesLetras = new Button[5];

    [Header("Interface")]
    [SerializeField] private TMP_Text textoCanto;
    [SerializeField] private TMP_Text mensagem;

    [Header("Controles")]
    [SerializeField] private Button botaoOuvirCanto;
    [SerializeField] private Button botaoVerificar;
    [SerializeField] private Button botaoLimpar;
    [SerializeField] private Button botaoFechar;

    [Header("Velocidade da gravacao")]
    [SerializeField] private float tempoLetra = 0.5f;
    [SerializeField] private float intervalo = 0.15f;

    [Header("Cena de retorno")]
    [SerializeField] private string cenaRetorno = "Game";
    [SerializeField]
    private string pontoRetorno =
        "PontoEntrada_Canto";

    private readonly string[] letras =
    {
        "C", 
        "A", 
        "N", 
        "T", 
        "O"  
    };

    private readonly int[] solucao = { 3, 0, 4, 1, 2 };

    private int[] ordem = { -1, -1, -1, -1, -1 };

    private int slotSelecionado = -1;
    private bool concluido;
    private bool reproduzindo;

    private void Start()
    {
        concluido = PlayerPrefs.GetInt(
            "PuzzleCantoConcluido", 0
        ) == 1;

        for (int i = 0; i < slots.Length; i++)
        {
            ordem[i] = PlayerPrefs.GetInt(
                "CantoSlot_" + i, -1
            );

            int indice = i;

            slots[i].onClick.AddListener(
                () => SelecionarSlot(indice)
            );
        }

        for (int i = 0; i < botoesLetras.Length; i++)
        {
            int indice = i;

            botoesLetras[i].onClick.AddListener(
                () => SelecionarLetra(indice)
            );
        }

        botaoOuvirCanto.onClick.AddListener(OuvirCanto);
        botaoVerificar.onClick.AddListener(Verificar);
        botaoLimpar.onClick.AddListener(Limpar);
        botaoFechar.onClick.AddListener(Fechar);

        textoCanto.text = "";

        mensagem.text = concluido
            ? "Memoria recuperada."
            : "Ouça o canto e reproduza a sequencia.";

        AtualizarTela();
    }

    private void OuvirCanto()
    {
        if (reproduzindo)
            return;

        StartCoroutine(ReproduzirSequencia());
    }

    private IEnumerator ReproduzirSequencia()
    {
        reproduzindo = true;
        AtualizarTela();

        mensagem.text = "Memorize as letras...";

        textoCanto.text = "";
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < solucao.Length; i++)
        {
            int id = solucao[i];

            textoCanto.text = letras[id];

            yield return new WaitForSeconds(tempoLetra);

            textoCanto.text = "";

            yield return new WaitForSeconds(intervalo);
        }

        reproduzindo = false;

        mensagem.text = concluido
            ? "Memoria recuperada."
            : "Agora reproduza a sequencia.";

        AtualizarTela();
    }

    private void SelecionarSlot(int indice)
    {
        if (concluido || reproduzindo)
            return;

        slotSelecionado = indice;

        mensagem.text = "Escolha uma letra.";

        AtualizarTela();
    }

    private void SelecionarLetra(int id)
    {
        if (concluido || reproduzindo)
            return;

        if (slotSelecionado == -1)
        {
            mensagem.text =
                "Selecione primeiro uma posicao.";
            return;
        }

        for (int i = 0; i < ordem.Length; i++)
        {
            if (ordem[i] == id)
                ordem[i] = -1;
        }

        ordem[slotSelecionado] = id;
        slotSelecionado = -1;

        SalvarOrdem();
        AtualizarTela();

        mensagem.text = "Letra posicionada.";
    }

    private void AtualizarTela()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            int id = ordem[i];

            TMP_Text texto =
                slots[i].GetComponentInChildren<TMP_Text>();

            texto.text = id >= 0 && id < letras.Length
                ? (i + 1) + ". " + letras[id]
                : (i + 1) + ". ?";

            bool selecionado = slotSelecionado == i;

            slots[i].image.color = selecionado
                ? new Color(0.75f, 0.55f, 0.35f)
                : new Color(0.85f, 0.80f, 0.70f);

            slots[i].interactable = !concluido && !reproduzindo;
        }

        for (int i = 0; i < botoesLetras.Length; i++)
        {
            TMP_Text texto =
                botoesLetras[i].GetComponentInChildren<TMP_Text>();

            texto.text = letras[i];

            botoesLetras[i].interactable =
                !concluido && !reproduzindo;
        }

        botaoOuvirCanto.interactable = !reproduzindo;
        botaoVerificar.interactable = !concluido && !reproduzindo;
        botaoLimpar.interactable = !concluido && !reproduzindo;
    }

    private void SalvarOrdem()
    {
        for (int i = 0; i < ordem.Length; i++)
        {
            PlayerPrefs.SetInt(
                "CantoSlot_" + i, ordem[i]
            );
        }

        PlayerPrefs.Save();
    }

    private void Limpar()
    {
        if (concluido || reproduzindo)
            return;

        for (int i = 0; i < ordem.Length; i++)
            ordem[i] = -1;

        slotSelecionado = -1;

        SalvarOrdem();
        AtualizarTela();

        mensagem.text = "Sequencia limpa.";
    }

    private void Verificar()
    {
        if (concluido || reproduzindo)
            return;

        for (int i = 0; i < solucao.Length; i++)
        {
            if (ordem[i] != solucao[i])
            {
                mensagem.text = "Sequencia incorreta.";
                return;
            }
        }

        concluido = true;

        PlayerPrefs.SetInt("PuzzleCantoConcluido", 1);
        PlayerPrefs.Save();

        mensagem.text =
            "Memoria recuperada! O canto foi reconstruido.";

        AtualizarTela();
    }

    private void Fechar()
    {
        PlayerPrefs.SetString(
            "NomePontoEntrada", pontoRetorno
        );

        PlayerPrefs.SetInt("VeioDeOutraCena", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(cenaRetorno);
    }
}
