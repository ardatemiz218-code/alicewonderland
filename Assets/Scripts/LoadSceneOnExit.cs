using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnExit : StateMachineBehaviour
{
    [Header("Scene")]
    public string nextSceneName = "06_RabbitHouse";

    // Crying state'inden ÇIKINCA burası çalışır
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
