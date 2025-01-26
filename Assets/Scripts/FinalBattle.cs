using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    public GameObject finalBoss; // Prefabricado que se va a instanciar
    public Transform spawnLocation;  // Ubicación donde se instanciará el prefab

    public AudioSource audioboss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(finalBoss, spawnLocation.position, Quaternion.identity);
            audioboss.Play();
            
        }

            gameObject.SetActive(false);
    }
}
