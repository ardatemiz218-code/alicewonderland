using UnityEngine;

public class RabbitArrowIndicator : MonoBehaviour
{
    public Transform rabbit;
    public Transform player; // ok player’ın child’ı ise şart değil ama iyi

    public float yLock = 0f; // ok yatayda dönsün diye

    private void LateUpdate()
    {
        if (rabbit == null) return;

        Vector3 from = transform.position;
        Vector3 to = rabbit.position;

        // sadece XZ düzleminde baksın
        to.y = from.y + yLock;

        Vector3 dir = (to - from);
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    public void HideArrow()
    {
        gameObject.SetActive(false);
    }
}
