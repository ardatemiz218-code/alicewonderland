using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("01_Garden"); 
        // kendi oyun sahnenin adı neyse onu yaz
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
