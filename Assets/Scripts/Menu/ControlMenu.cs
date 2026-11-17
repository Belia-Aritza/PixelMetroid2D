using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void VerCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        
    }
}
