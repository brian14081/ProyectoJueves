using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelTutorial;

    void Update()
    {
        // Si el jugador aprieta ESC y el panel de tutorial está abierto, lo cierra
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelTutorial != null && panelTutorial.activeSelf)
            {
                CerrarTutorial();
            }
        }
    }

    public void Jugar()
    {
        Time.timeScale = 1f; // Asegura que el tiempo corra
        SceneManager.LoadScene(1); // Carga la escena del juego
    }

    public void AbrirTutorial()
    {
        if (panelTutorial != null) panelTutorial.SetActive(true);
    }

    public void CerrarTutorial()
    {
        if (panelTutorial != null) panelTutorial.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit(); // Cierra el juego final (.exe)
    }
}