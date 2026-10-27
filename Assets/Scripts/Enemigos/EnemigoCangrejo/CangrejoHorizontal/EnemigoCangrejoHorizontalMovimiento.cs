using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemigoCangrejoHorizontalMovimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public Vector2 posicionFinal;
    private Vector2 posicionInicial;
    private bool movimientoHaciaLosLados = true;
    [SerializeField] private float tolerancia = 0.01f;


    private Rigidbody2D fisica;

    [Header("Animacion")]
    private SpriteRenderer renderizador;
    private Animator animador;

    void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
        renderizador = GetComponent<SpriteRenderer>();
        animador = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionInicial = fisica.position;
    }

    // Update is called once per frame
    void Update()
    {
        // animarCangrejo();
    }

    // private void animarCangrejo()
    // {
    //     if (animador == null)
    //     {
    //         return;
    //     }

    //     if (!estaTocandoSuelo)
    //     {
    //         animador.Play("JugadorSaltando");
    //     }
    //     else if (Mathf.Abs(fisica.linearVelocity.x) > umbralCarrera)
    //     {
    //         animador.Play("JugadorCorriendo");
    //     } 
    //     else
    //     {
    //         animador.Play("JugadorParado");
    //     }
    // }

    void FixedUpdate()
    {
        //Vector2 posicionObjetivo;

        if (velocidad <= 0)
        {
            return;
        }

        Vector2 posicionObjetivo = movimientoHaciaLosLados ? posicionFinal : posicionInicial;
        
        if (Vector2.Distance(fisica.position, posicionObjetivo) <= tolerancia)
        {
            movimientoHaciaLosLados = !movimientoHaciaLosLados; //si se mueve hacia un lado cambia el movimiento hacia ese lado a falso para que vaya hacia el lado contrario
        }
        fisica.MovePosition(Vector2.MoveTowards(fisica.position, posicionObjetivo, velocidad * Time.fixedDeltaTime));

        // OrientarSprite();
    }

    // private void OrientarSprite()
    // {
    //     if (renderizador == null)
    //     {
    //         return;
    //     }

    //     //gira si va hacia la izquierda
    //     if (fisica.linearVelocity.x > 0)
    //     {
    //         renderizador.flipX = false;
    //     }
    //     else if (fisica.linearVelocity.x < 0)
    //     {
    //         renderizador.flipX = true;
    //     }
    // }
}
