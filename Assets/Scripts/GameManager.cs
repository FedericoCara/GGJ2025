using UnityEngine;
using UnityEngine.SceneManagement;

namespace BubbleNS
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null || _instance.gameObject == null)
                    _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }
        #endregion

        public float delay;

        public void RestartLevel()
        {
            Invoke(nameof(RestartLevelDelayed), delay);
        }

        private void RestartLevelDelayed()
        {
            SceneManager.LoadScene("GameOver");
        }

        public void ShowVictory()
        {
            SceneManager.LoadScene("Victory");
        }
    }
}