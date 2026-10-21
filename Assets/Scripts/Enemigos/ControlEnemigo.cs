using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ControlEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    public Vector2 posicionFinal;
    private Vector2 posicionInicial;
    private bool movimientoHaciaArriva = true;
    [SerializeField] private float tolerancia = 0.01f;

    private Rigidbody2D fisica;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        posicionInicial = fisica.position;
    }

    void FixedUpdate()
    {
        //Vector2 posicionObjetivo;

        if (velocidad <= 0)
            return;

//        if (movimientoHaciaArriva)
//        {
//            posicionObjetivo = posicionFinal;
//        }
//        else
//        {
//            posicionObjetivo = posicionInicial;
//        }

        Vector2 posicionObjetivo = movimientoHaciaArriva ? posicionFinal : posicionInicial;

        if (Vector2.Distance(fisica.position, posicionObjetivo) <= tolerancia)
        {
            movimientoHaciaArriva = !movimientoHaciaArriva; //si se mueve hacia arriva cambia el movimiento hacia arriva a falso para que vaya hacia abajo y al revés
        }
        fisica.MovePosition(Vector2.MoveTowards(fisica.position, posicionObjetivo, velocidad * Time.fixedDeltaTime));
    }
}
