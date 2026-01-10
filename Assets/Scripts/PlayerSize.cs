using System.Collections;
using UnityEngine;

public class PlayerSize : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform visualRoot; // model child'ı (Peasant Girl@Slow Run) buraya ver; boşsa bu obje büyür

    [Header("Defaults")]
    public float defaultTweenTime = 0.35f;

    Vector3 baseScale;
    float baseHeight;
    Vector3 baseCenter;

    Coroutine routine;

    void Awake()
    {
        if (visualRoot == null) visualRoot = transform;

        baseScale = visualRoot.localScale;

        if (controller == null) controller = GetComponent<CharacterController>();
        if (controller == null) controller = GetComponentInChildren<CharacterController>(true);

        if (controller != null)
        {
            baseHeight = controller.height;
            baseCenter = controller.center;
        }
        else
        {
            Debug.LogWarning("[PlayerSize] CharacterController bulunamadı!");
        }
    }

    /// <summary>
    /// targetMultiplier: 1 = normal, >1 = büyü, <1 = küçül
    /// duration: 0 = kalıcı, >0 = süre bitince 1'e döner
    /// </summary>
    public void ApplySize(float targetMultiplier, float tweenTime = -1f, float duration = 0f)
    {
        if (controller == null)
        {
            Debug.LogWarning("[PlayerSize] ApplySize çağrıldı ama CharacterController yok!");
            return;
        }

        if (tweenTime <= 0f) tweenTime = defaultTweenTime;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(SizeRoutine(targetMultiplier, tweenTime, duration));
    }

    IEnumerator SizeRoutine(float targetMultiplier, float tweenTime, float duration)
    {
        // hedefler (base'e göre)
        Vector3 targetScale = baseScale * targetMultiplier;
        float targetHeight = baseHeight * targetMultiplier;
        Vector3 targetCenter = baseCenter * targetMultiplier;

        yield return TweenTo(targetScale, targetHeight, targetCenter, tweenTime);

        if (duration > 0f)
        {
            yield return new WaitForSeconds(duration);
            yield return TweenTo(baseScale, baseHeight, baseCenter, tweenTime);
        }

        routine = null;
    }

    IEnumerator TweenTo(Vector3 targetScale, float targetHeight, Vector3 targetCenter, float time)
    {
        Vector3 startScale = visualRoot.localScale;
        float startH = controller.height;
        Vector3 startC = controller.center;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, time);
            float s = Mathf.SmoothStep(0f, 1f, t);

            visualRoot.localScale = Vector3.Lerp(startScale, targetScale, s);
            controller.height = Mathf.Lerp(startH, targetHeight, s);
            controller.center = Vector3.Lerp(startC, targetCenter, s);

            yield return null;
        }
    }
}
