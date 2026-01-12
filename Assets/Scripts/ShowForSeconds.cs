using System.Collections;
using UnityEngine;

public class ShowForSeconds : MonoBehaviour
{
    public float showDuration = 2f;
    public GameObject target; // göster/gizleyeceğimiz obje (Image veya Canvas)

    void Awake()
    {
        if (target == null) target = gameObject;
    }

    IEnumerator Start()
    {
        target.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        target.SetActive(false); // istersen Destroy(target); da yapabilirsin
    }
}
