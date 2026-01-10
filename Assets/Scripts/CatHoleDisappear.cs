using UnityEngine;
using UnityEngine.AI;

public class CatHoleDisappear : MonoBehaviour
{
    [Tooltip("Kedi girince tamamen yok olsun mu? (SetActive false)")]
    public bool disableObject = true;

    private void OnTriggerEnter(Collider other)
    {
        // Kedi root'u child collider ile gelebilir → root'a çık
        Transform root = other.attachedRigidbody != null ? other.attachedRigidbody.transform : other.transform.root;

        // Sadece NavMeshAgent olan (yani AI hayvan) için çalıştır
        var agent = root.GetComponent<NavMeshAgent>();
        if (agent == null) agent = root.GetComponentInChildren<NavMeshAgent>(true);
        if (agent == null) return;

        if (disableObject)
            root.gameObject.SetActive(false);
        else
            Destroy(root.gameObject);
    }
}
