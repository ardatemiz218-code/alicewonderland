using UnityEngine;

public class SwimZone : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        Animator anim = other.GetComponentInChildren<Animator>();
        if (anim != null) anim.SetBool("IsSwimming", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        Animator anim = other.GetComponentInChildren<Animator>();
        if (anim != null) anim.SetBool("IsSwimming", false);
    }
}
