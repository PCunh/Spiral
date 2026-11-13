
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TransicaoPorta : MonoBehaviour
{
    [Header("Configuração da transição")]
    public string nomeCenaDestino;

    [Header("Ponto de chegada na outra cena")]
    public string nomePontoEntrada;

    private bool jogadorPerto = false;

    private void Update()
    {
        if (jogadorPerto &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (string.IsNullOrEmpty(nomePontoEntrada))
            {
                Debug.LogError(
                    "Ponto de entrada vazio na porta: " + gameObject.name +
                    ". Valor recebido: [" + nomePontoEntrada + "]"
                );
                return;
            }
            else
            {
                Debug.Log("Porta: " + gameObject.name +
                        " | Cena destino: " + nomeCenaDestino +
                        " | Ponto de entrada: " + nomePontoEntrada);
            }

            PlayerPrefs.SetString("NomePontoEntrada", nomePontoEntrada);
            PlayerPrefs.SetInt("VeioDeOutraCena", 1);

            SceneManager.LoadScene(nomeCenaDestino);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            Debug.Log("Pressione E para entrar.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            Debug.Log("Você saiu da área da porta.");
        }
    }
}
