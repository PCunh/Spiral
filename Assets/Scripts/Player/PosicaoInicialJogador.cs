
using UnityEngine;

public class PosicaoInicialJogador : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("VeioDeOutraCena", 0) == 1)
        {
            string nomePonto = PlayerPrefs.GetString("NomePontoEntrada");

            GameObject ponto = GameObject.Find(nomePonto);

            if (ponto != null)
            {
                transform.position = ponto.transform.position;
                Debug.Log("Jogador posicionado em: " + nomePonto);
            }
            else
            {
                Debug.LogError("Ponto de entrada não encontrado: " + nomePonto);
            }

            PlayerPrefs.SetInt("VeioDeOutraCena", 0);
            PlayerPrefs.DeleteKey("NomePontoEntrada");
        }
    }
}
