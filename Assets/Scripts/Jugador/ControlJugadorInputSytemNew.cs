using UnityEngine;
using UnityEngine.InputSystem;

public class ControlJugadorInputSytemNew : MonoBehaviour
{
    public InputAction moveAction;
    public float velocity;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();

        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 30;//Se actualiza 30fps
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();

        Vector2 position = (Vector2)transform.position + (move * velocity * (Time.deltaTime));

        //Habiamos modificado el vector2 position y actualizamos la posicion del jugador al nuevo position del vector2 position
        transform.position = position;
    }

    


}
