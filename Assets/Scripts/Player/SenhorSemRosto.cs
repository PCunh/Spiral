
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class SenhorSemRosto : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Transform spawnEsquerda;
    [SerializeField] private Transform spawnDireita;

    [Header("Jogador")]
    [SerializeField] private Transform jogador;

    [Header("Perseguicao")]
    [SerializeField] private float velocidadeBase = 2f;
    [SerializeField] private float raioDeteccao = 6f;
    [SerializeField] private float raioPerda = 9f;
    [SerializeField] private float distanciaParada = 0.4f;

    [Header("Insanidade")]
    [SerializeField, Range(0f, 100f)] private float insanidade = 0f;
    [SerializeField] private float velocidadeExtraMaxima = 2f;

    [Header("Animacao")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Estados do Animator")]
    [SerializeField] private string andandoLado = "AndandoLado";
    [SerializeField] private string andandoFrente = "AndandoFrente";
    [SerializeField] private string andandoCostas = "AndandoCostas";
    [SerializeField] private string correndoLado = "CorrendoLado";
    [SerializeField] private string correndoFrente = "CorrendoFrente";
    [SerializeField] private string correndoCostas = "CorrendoCostas";

    [Header("Corrida")]
    [SerializeField] private float velocidadeParaCorrer = 3f;

    private Rigidbody2D rb;
    private bool perseguindo;
    private bool capturado;
    private string animacaoAtual = "";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (jogador == null)
        {
            GameObject encontrado = GameObject.FindGameObjectWithTag("Player");

            if (encontrado != null)
                jogador = encontrado.transform;
        }

        if (jogador == null || spawnEsquerda == null || spawnDireita == null)
            return;

        float distanciaEsquerda = Vector2.Distance(
            jogador.position,
            spawnEsquerda.position
        );

        float distanciaDireita = Vector2.Distance(
            jogador.position,
            spawnDireita.position
        );

        Transform spawnEscolhido = distanciaEsquerda > distanciaDireita
            ? spawnEsquerda
            : spawnDireita;

        rb.position = spawnEscolhido.position;
        rb.linearVelocity = Vector2.zero;
        perseguindo = false;
    }

    private void FixedUpdate()
    {
        if (jogador == null || capturado)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 posicaoJogador = jogador.position;
        Vector2 posicaoMonstro = rb.position;

        float distancia = Vector2.Distance(posicaoMonstro, posicaoJogador);

        if (!perseguindo && distancia <= raioDeteccao)
            perseguindo = true;

        if (perseguindo && distancia >= raioPerda)
            perseguindo = false;

        if (!perseguindo || distancia <= distanciaParada)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float fatorInsanidade = insanidade / 100f;

        float velocidadeAtual = velocidadeBase +
            velocidadeExtraMaxima * fatorInsanidade;

        Vector2 direcao = (posicaoJogador - posicaoMonstro).normalized;

        rb.linearVelocity = direcao * velocidadeAtual;

        AtualizarAnimacao(direcao, velocidadeAtual);
    }

    private void AtualizarAnimacao(Vector2 direcao, float velocidade)
    {
        if (animator == null || spriteRenderer == null)
            return;

        if (direcao.sqrMagnitude < 0.001f)
            return;

        bool correndo = velocidade >= velocidadeParaCorrer;
        string novaAnimacao;

        if (Mathf.Abs(direcao.x) >= Mathf.Abs(direcao.y))
        {
            novaAnimacao = correndo ? correndoLado : andandoLado;
            spriteRenderer.flipX = direcao.x < 0f;
        }
        else if (direcao.y > 0f)
        {
            novaAnimacao = correndo ? correndoCostas : andandoCostas;
            spriteRenderer.flipX = false;
        }
        else
        {
            novaAnimacao = correndo ? correndoFrente : andandoFrente;
            spriteRenderer.flipX = false;
        }

        if (novaAnimacao != animacaoAtual)
        {
            animator.Play(novaAnimacao);
            animacaoAtual = novaAnimacao;
        }
    }

    public void DefinirInsanidade(float valor)
    {
        insanidade = Mathf.Clamp(valor, 0f, 100f);
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (capturado || !colisao.gameObject.CompareTag("Player"))
            return;

        capturado = true;
        rb.linearVelocity = Vector2.zero;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeteccao);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raioPerda);
    }
}
