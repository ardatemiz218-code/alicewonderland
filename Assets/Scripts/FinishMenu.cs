using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    // Retry butonu burayı çağıracak
    public void RetryGame()
    {
        Time.timeScale = 1f; // Pause kaldıysa diye garanti
        SceneManager.LoadScene("01_Garden"); 
        // Oyunun başladığı sahne adı neyse onu yaz
    }

    // Quit butonu burayı çağıracak
    public void QuitGame()
    {
        Debug.Log("Game Quit"); // Editor'da görmek için
        Application.Quit();
    }
}
