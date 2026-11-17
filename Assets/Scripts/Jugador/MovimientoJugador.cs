using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocity;
    [SerializeField] private InputActionReference accionMover;

    [Header("Salto")]
    [SerializeField] private InputActionReference accionSaltar;
    [SerializeField] private LayerMask capaSuelo;
    //fuerza aplicada al saltar
    public float fuerzaImpulso = 5f;


    private Rigidbody2D fisica;
    private SpriteRenderer renderizador;
    private Animator animador;
    private Vector2 entradaX;
    private Boolean saltoPendiente;
    private Boolean estaTocandoSuelo;
    private float logitudRayCast = 1.6f;
    private float umbralCarrera = 0.1f;

    [SerializeField] private Transform spawnPoint;

    [Header("Puntuación")]
    [SerializeField] private int puntuacion;

    [Header("Vida")]
    [SerializeField] private int vidas = 3;
    public int Vidas => vidas; //Propiedad de solo lectura para acceder a las vidas del jugador desde otros scripts
    bool vulnerable = true; // Variable para controlar la invulnerabilidad del jugador

    [Header("Color")]
    private Color colorOriginal; // Color del sprite cuando el jugador pierde una vida

    [SerializeField, Min(0.01f)] private float segundosInvulnerable = 1f; // Tiempo que el jugador es invulnerable después de perder una vida

    [Header("Tiempo")]
    private float tiempoInicio;
    private float tiempoReal;
    [SerializeField] private int tiempoLimiteNivel = 100; // Tiempo límite en segundos
    [SerializeField] private float tiempoEmpleado; // Tiempo transcurrido desde el inicio del nivel

    [Header("Canvas")]
    [SerializeField] private Canvas canvasInterfaz; // Referencia al script ControlHUD para actualizar el HUD
    private ControlHUD hud; // Referencia al script ControlHUD para actualizar el HUD

    //Para acceder al mapa de acciones del inputAction.

    //Se ejecuta cuando el script este en la escena, se usa cuando tiene que acceder otro gameObject distinto al que tiene el script
    //ejemplo los sonidos
    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
        renderizador = GetComponent<SpriteRenderer>();
        animador = GetComponent<Animator>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Se inicia si el script esta activo
    void Start()
    {
        if (renderizador != null)
        {
            colorOriginal = renderizador.color; // Guardar el color original del sprite al inicio
        }

        tiempoInicio = Time.time; // Guardar el tiempo de inicio del nivel

        InicializaHUD();
    }

    // Update is called once per frame
    void Update()
    {
        entradaX = accionMover.action.ReadValue<Vector2>();

        comprobarFinJuegoTiempo();

        SaltoJugador();

        animarJugador();

        // Actualizar el HUD con la cantidad de power-ups restantes
        hud.SetPowerUp(GameObject.FindGameObjectsWithTag("PowerUp").Length);

    }

    private void InicializaHUD()
    {
        if (canvasInterfaz != null)
        {
            hud = canvasInterfaz.GetComponent<ControlHUD>();
        }
        else
        {
            Debug.LogError("Asigna el objeto Canvas con el controlHUD al atributo Canvas de Jugador");
        }

        if (hud != null)
        {
            hud.SetVidas(vidas);
            hud.SetPowerUp(GameObject.FindGameObjectsWithTag("PowerUp").Length);
            hud.SetTiempo(tiempoLimiteNivel);
        }
        else
        {
            Debug.LogError("Asigna el objeto Canvas con el controlHUD al atributo Canvas de Jugador");
        }
    }

    private void comprobarFinJuegoTiempo()
    {

        tiempoReal = Time.time - tiempoInicio; // Calcular el tiempo transcurrido desde el inicio del nivel
        tiempoEmpleado = Mathf.FloorToInt(tiempoReal); // Convertir el tiempo transcurrido a un valor entero
        int segundosRestantes = tiempoLimiteNivel - (int)tiempoReal; // Calcular los segundos restantes
        hud.SetTiempo(segundosRestantes); // Actualizar el HUD con el tiempo restante
        if (tiempoEmpleado >= tiempoLimiteNivel)
        {
            FinJuego();
        }
    }

    private void SaltoJugador()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, logitudRayCast, capaSuelo);

        estaTocandoSuelo = hit.collider != null;

        Color colorRayo = estaTocandoSuelo ? Color.green : Color.red;
        Debug.DrawRay(transform.position, Vector2.down * logitudRayCast, colorRayo);

        if (accionSaltar.action.WasPressedThisFrame() && estaTocandoSuelo)
        {
            saltoPendiente = true;
        }
    }

    private void animarJugador()
    {
        if (animador == null)
        {
            return;
        }

        if (!estaTocandoSuelo)
        {
            animador.Play("JugadorSaltando");
        }
        else if (Mathf.Abs(fisica.linearVelocity.x) > umbralCarrera)
        {
            animador.Play("JugadorCorriendo");
        }
        else
        {
            animador.Play("JugadorParado");
        }
    }

    private void OnEnable()
    {
        if ((accionMover == null) || accionMover.action == null)
        {
            Debug.LogError("Asigna Mapa de acción Jugador/Mover en Acción Mover");
            enabled = false;
            return;
        }
        accionMover.action.Enable();

        if ((accionSaltar == null) || accionSaltar.action == null)
        {
            Debug.LogError("Asigna Mapa de acción Jugador/Saltar en Acción Saltar");
            enabled = false;
            return;
        }
        accionSaltar.action.Enable();
    }

    private void OnDisable()
    {
        if ((accionMover != null) && (accionMover.action != null))
        {
            accionMover.action.Disable();
        }

        if ((accionSaltar != null) && (accionSaltar.action != null))
        {
            accionSaltar.action.Disable();
        }

    }

    //Todo lo que sea fisica se pone en el fixed update.
    void FixedUpdate()
    {
        MoverJugador();

        OrientarSprite();


    }

    private void MoverJugador()
    {
        fisica.linearVelocity = new Vector2(entradaX.x * velocity, fisica.linearVelocity.y);

        if (saltoPendiente && estaTocandoSuelo)
        {
            fisica.AddForce(Vector2.up * fuerzaImpulso, ForceMode2D.Impulse);

        }
        saltoPendiente = false;
    }

    private void OrientarSprite()
    {
        if (renderizador == null)
        {
            return;
        }

        //gira si va hacia la izquierda
        if (fisica.linearVelocity.x > 0)
        {
            renderizador.flipX = false;
        }
        else if (fisica.linearVelocity.x < 0)
        {
            renderizador.flipX = true;
        }
    }

    public void ReaparecerJugador()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Asigna un punto de spawn para el jugador", this);
            enabled = false;
            return;
        }
        // Reinicia la posición del jugador al reaparecer
        fisica.position = (Vector2)spawnPoint.position;
        // Reinicia la velocidad del jugador al reaparecer
        fisica.linearVelocity = Vector2.zero;
    }

    public void IncrementarPuntos(int cantidad)
    {
        // Lógica para incrementar los puntos del jugador
        if (cantidad <= 0)
        {
            puntuacion += 1;
        }
    }

    public void QuitarVida()
    {

        if (!vulnerable)
        {
            return; // Si el jugador es invulnerable, no se quita vida
        }

        vulnerable = false; // El jugador se vuelve invulnerable temporalmente

        vidas--;

        hud.SetVidas(vidas); // Actualiza el HUD con la nueva cantidad de vidas

        if (renderizador != null)
        {
            renderizador.color = Color.blue; // Cambia el color del sprite a azul para indicar que el jugador ha perdido una vida
        }

        if (vidas <= 0)
        {
            FinJuego();
            return;
        }

        Invoke(nameof(HacerVulnerable), segundosInvulnerable); // Llama a HacerVulnerable después de 1 segundo


    }

    private void HacerVulnerable()
    {
        vulnerable = true; // El jugador vuelve a ser vulnerable
        if (renderizador != null)
        {
            renderizador.color = colorOriginal; // Restaura el color original del sprite
        }
    }

    public void FinJuego()
    {
        Debug.Log("Juego finalizado");
        gameObject.SetActive(false);
    }
}
