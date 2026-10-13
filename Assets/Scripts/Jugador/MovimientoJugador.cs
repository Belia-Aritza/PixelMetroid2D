using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class MovimientoJugador : MonoBehaviour
{
    public float velocity;

    private Rigidbody2D fisica;

    private Vector2 entradaX;


    //Para acceder al mapa de acciones del inputAction.
    [SerializeField]private InputActionReference accionMover;

    //Se ejecuta cuando el script este en la escena, se usa cuando tiene que acceder otro gameObject distinto al que tiene el script
    //ejemplo los sonidos
    private void Awake()
    {
        fisica = GetComponent<Rigidbody2D>();
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Se inicia si el script esta activo
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        entradaX = accionMover.action.ReadValue<Vector2>();
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
    }

    private void OnDisable()
    {
        if ((accionMover != null) && (accionMover.action != null))
        {
            accionMover.action.Disable();
        }

        
    }

    void FixedUpdate()
    {
        fisica.linearVelocity = new Vector2(entradaX.x * velocity, fisica.linearVelocity.y);
    }

}
