using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAfterAnimatorState : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    public string cryingStateName = "Crying"; // Animator’daki state adı
    public int cryingLayerIndex = 0;

    [Header("Scene")]
    public string nextSceneName;

    bool started;
    bool finished;

    void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (animator == null || finished) return;

        var st = animator.GetCurrentAnimatorStateInfo(cryingLayerIndex);

        // Cry state'ine girdi mi?
        if (!started && st.IsName(cryingStateName))
            started = true;

        // Cry state'indeyken normalizedTime >= 1 oldu mu? (anim bitti)
        if (started && st.IsName(cryingStateName) && st.normalizedTime >= 1f)
        {
            finished = true;
            if (!string.IsNullOrEmpty(nextSceneName))
                SceneManager.LoadScene(nextSceneName);
        }
    }
}
