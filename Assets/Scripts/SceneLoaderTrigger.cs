using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderTrigger : MonoBehaviour
{
    [Tooltip("Boş bırakılırsa Build Settings'te bir sonraki sahneye geçer.")]
    public string targetSceneName = "";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
            return;
        }

        // otomatik: bir sonraki scene
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            nextIndex = 0; // istersen oyunu başa sar

        SceneManager.LoadScene(nextIndex);
    }
}
