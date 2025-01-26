using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoadSceneOnVideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Referencia al VideoPlayer
    public string sceneToLoad;       // El nombre de la escena a cargar

    void Start()
    {
        // Asegurarse de que el VideoPlayer esté asignado
        if (videoPlayer != null)
        {
            // Suscribirse al evento que se llama cuando el video termina
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogWarning("VideoPlayer no está asignado en el inspector.");
        }
    }

    // Método que se llama cuando el video termina
    private void OnVideoEnd(VideoPlayer vp)
    {
        // Cargar la escena cuando el video termine
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No se ha asignado el nombre de la escena.");
        }
    }

    // Si quieres asegurarte de desuscribirte cuando el objeto se destruya
    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
