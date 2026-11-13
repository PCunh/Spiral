using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class botao : MonoBehaviour
{
    [SerializeField] private Button botaoFechar;
    [SerializeField] private Button botaoRotina;
    [SerializeField] private string cenaRetorno = "Game";
    [SerializeField] private string botaoCena = "PuzzleRotina1";
    [SerializeField] private string pontoRetorno = "PontoEntrada_Rotina";

    void Start()
    {
        botaoFechar.onClick.AddListener(Fechar);
        botaoRotina.onClick.AddListener(Rotina);

    }

    private void Rotina()
    {
        SceneManager.LoadScene(botaoCena);
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
