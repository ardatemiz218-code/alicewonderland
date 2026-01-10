using System.Collections;
using UnityEngine;

public class PotionSizeTrigger : MonoBehaviour
{
    [Header("Player")]
    public string playerTag = "Player";

    [Header("Drink Animation")]
    public string drinkTriggerName = "Drink";
    public float drinkAnimSeconds = 2.0f;

    [Header("Size Effect")]
    [Tooltip("1 = normal, >1 büyütür, <1 küçültür")]
    public float sizeMultiplier = 1.6f;
    [Tooltip("0 = kalıcı. >0 olursa süre bitince normale döner")]
    public float effectDuration = 0f;
    public float tweenTime = 0.35f;

    [Header("Disable Movement")]
    public MonoBehaviour movementScript; // Player üzerindeki ThirdPersonMovement'i buraya sürükle

    [Header("After Drink")]
    public bool hidePotion = true;

    bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (!other.CompareTag(playerTag)) return;
        used = true;

        Transform playerRoot = other.transform;
        var anim = playerRoot.GetComponentInChildren<Animator>();
        var size = playerRoot.GetComponent<PlayerSize>();

        if (size == null)
        {
            Debug.LogWarning("Player'da PlayerSize yok! Player objesine ekle.");
            return;
        }

        // Coroutine'i Player üzerinde çalıştır
        var runner = playerRoot.GetComponent<PotionRunner>();
        if (runner == null) runner = playerRoot.gameObject.AddComponent<PotionRunner>();

        runner.Run(anim, size, movementScript, drinkTriggerName, drinkAnimSeconds, sizeMultiplier, tweenTime, effectDuration);

        if (hidePotion) gameObject.SetActive(false);
    }
}

public class PotionRunner : MonoBehaviour
{
    Coroutine routine;

    public void Run(Animator anim, PlayerSize size, MonoBehaviour move, string trigger, float animSec,
                    float mult, float tween, float dur)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Co(anim, size, move, trigger, animSec, mult, tween, dur));
    }

    IEnumerator Co(Animator anim, PlayerSize size, MonoBehaviour move, string trigger, float animSec,
                   float mult, float tween, float dur)
    {
        if (move != null) move.enabled = false;

        if (anim != null && anim.runtimeAnimatorController != null)
        {
            anim.ResetTrigger(trigger);
            anim.SetTrigger(trigger);
        }

        yield return new WaitForSeconds(animSec);

        size.ApplySize(mult, tween, dur);

        // tween bitince hareket geri gelsin
        yield return new WaitForSeconds(tween + 0.05f);

        if (move != null) move.enabled = true;

        routine = null;
    }
}
