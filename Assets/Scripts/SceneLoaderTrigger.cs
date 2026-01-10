using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Boş bırakılırsa Build Settings'te bir sonraki sahneye geçer.")]
    public string targetSceneName = "";

    [Header("Rabbit References (Opsiyonel)")]
    public RabbitGuideController rabbitController;
    public RabbitArrowIndicator arrowIndicator;

    [Header("Options")]
    public bool hideRabbitWhenRabbitEnters = true;
    public bool loadSceneWhenPlayerEnters = true;

    private bool sceneLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        // 🐇 Tavşan girdi mi? -> sadece kaybolsun
        if (hideRabbitWhenRabbitEnters && rabbitController != null)
        {
            var rc = other.GetComponentInParent<RabbitGuideController>();
            if (rc == rabbitController)
            {
                HandleRabbitEnter();
                return; // ⛔ sahne geçişi YOK
            }
        }

        // 🧍 Player girdi mi? -> sahne geçsin
        if (loadSceneWhenPlayerEnters && other.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    private void HandleRabbitEnter()
    {
        if (rabbitController != null)
            rabbitController.HideRabbit();

        if (arrowIndicator != null)
            arrowIndicator.HideArrow();
    }

    private void LoadScene()
    {
        if (sceneLoaded) return;
        sceneLoaded = true;

        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
            return;
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            nextIndex = 0;

        SceneManager.LoadScene(nextIndex);
    }
}
