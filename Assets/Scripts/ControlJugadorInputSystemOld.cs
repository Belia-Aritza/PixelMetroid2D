using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlJugadorInputSystemOld : MonoBehaviour
{
    public float velocidad;

    private Rigidbody2D fisica; 
    private float entradaX;

    private
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Se unicia una sola vez antes de update()
    void Start()
    {
        fisica = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    // Se ejecuta una vez por fotograma
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        entradaX = Input.GetAxis("Horizontal");

        fisica.linearVelocity = new Vector2(entradaX * velocidad, fisica.linearVelocityX);
    }

    //MOVIMIENTO ANTIGUO
    // void Update()
    // {
    //  Vector2 posicionJugador = transform.position;

    //  posicionJugador.x = posicionJugador.x + 0.1f;

    //  transform.position = posicionJugador;
    // }


}
