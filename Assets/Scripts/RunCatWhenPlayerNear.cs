using UnityEngine;

public class RunCatWhenPlayerNear : MonoBehaviour
{
    public string playerTag = "Player";
    public CatRunToHole cat;

    bool used = false;

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (!other.CompareTag(playerTag)) return;

        used = true;
        if (cat != null) cat.StartRun();
    }
}
