using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSequence_AfterCryLoad : MonoBehaviour
{
    [Header("Vanish Condition")]
    public BirdController bird;          // Bird state Done olunca tetiklenecek
    public GameObject birdObject;
    public GameObject catObject;

    [Header("Alice Animation")]
    public Animator aliceAnimator;
    public string cryTriggerName = "Cry";

    [Tooltip("Animator'daki Crying state's adı (tam aynı yazılmalı).")]
    public string cryingStateName = "Crying";
    public int cryingLayerIndex = 0;

    [Header("Disable Movement While Crying")]
    public Behaviour[] movementScriptsToDisable; // ThirdPersonMovement vb.
    public CharacterController characterControllerToDisable; // varsa

    [Header("Scene Change")]
    public string nextSceneName = "06_RabbitHouse";

    [Header("Timing")]
    public float delayAfterVanish = 0.05f; // 0.05-0.2 iyi

    bool vanished;
    bool started;

    void Update()
    {
        // 1) Kuş Done oldu mu? -> vanish
        if (!vanished && bird != null && bird.state == BirdController.State.Done)
        {
            vanished = true;

            if (birdObject != null) birdObject.SetActive(false);
            if (catObject != null) catObject.SetActive(false);
        }

        // 2) Vanish olduysa cry + kilitle + anim bittiğinde scene değiş
        if (vanished && !started)
        {
            started = true;
            StartCoroutine(CryThenLoad());
        }
    }

    IEnumerator CryThenLoad()
    {
        // Hareketi kapat
        if (movementScriptsToDisable != null)
            foreach (var b in movementScriptsToDisable)
                if (b != null) b.enabled = false;

        if (characterControllerToDisable != null)
            characterControllerToDisable.enabled = false;

        yield return new WaitForSeconds(delayAfterVanish);

        // Cry tetikle
        if (aliceAnimator != null)
            aliceAnimator.SetTrigger(cryTriggerName);

        // Crying state'e gerçekten girmesini bekle (1-2 frame)
        yield return null;
        yield return null;

        // Crying animasyonu bitene kadar bekle
        while (aliceAnimator != null)
        {
            var st = aliceAnimator.GetCurrentAnimatorStateInfo(cryingLayerIndex);

            // Doğru state mi?
            if (st.IsName(cryingStateName))
            {
                // normalizedTime 1.0 => anim 1 kez tamamlandı
                if (st.normalizedTime >= 1f)
                    break;
            }

            yield return null;
        }

        // Scene load
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}

