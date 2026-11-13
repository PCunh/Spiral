using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimacaoElias : MonoBehaviour
{
    [SerializeField] private Sprite[] frente = new Sprite[4];
    [SerializeField] private Sprite[] costas = new Sprite[4];
    [SerializeField] private Sprite[] lado = new Sprite[4];

    [SerializeField, Min(1f)] private float quadrosPorSegundo = 7f;

    private PlayerController jogador;
    private SpriteRenderer spriteRenderer;
    private float tempo;
    private int quadro;
    private Vector2 direcaoAnterior = Vector2.zero;

    private void Awake()
    {
        jogador = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector2 direcao = jogador.UltimaDirecao;
        Sprite[] animacao;

        if (Mathf.Abs(direcao.x) > 0.5f)
        {
            animacao = lado;
            spriteRenderer.flipX = direcao.x < 0f;
        }
        else if (direcao.y > 0.5f)
        {
            animacao = costas;
            spriteRenderer.flipX = false;
        }
        else
        {
            animacao = frente;
            spriteRenderer.flipX = false;
        }

        if (direcao != direcaoAnterior)
        {
            quadro = 0;
            tempo = 0f;
            direcaoAnterior = direcao;
        }

        if (!jogador.EstaMovendo)
        {
            quadro = 0;
            tempo = 0f;
        }
        else
        {
            tempo += Time.deltaTime;
            float duracao = 1f / quadrosPorSegundo;
            while (tempo >= duracao)
            {
                tempo -= duracao;
                quadro = (quadro + 1) % animacao.Length;
            }
        }

        if (animacao.Length > 0 && animacao[quadro] != null)
            spriteRenderer.sprite = animacao[quadro];
    }
}
