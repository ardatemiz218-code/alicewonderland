using System;
using UnityEngine;

public class CatFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 3f;
    public float stopDistance = 1.2f;
    public float turnSpeed = 10f;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        // sadece yön için y'yi sıfırla (yerde kalsın)
        Vector3 flat = new Vector3(toTarget.x, 0f, toTarget.z);

        if (flat.magnitude > stopDistance)
        {
            if (animator != null)
            {
                animator.SetFloat("Vert", 1);
                animator.SetFloat("State", 1);
            }
            Vector3 dir = flat.normalized;
            transform.position += dir * followSpeed * Time.deltaTime;

            if (dir.sqrMagnitude > 0.0001f)
            {
                var rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
            }
        }
    }
}
