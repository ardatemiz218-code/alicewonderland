using UnityEngine;

public class BirdController : MonoBehaviour
{
    public enum State { Circle, Flee, Done }
    public State state = State.Circle;

    [Header("Circle Movement")]
    public Vector3 circleCenterOffset = Vector3.zero;
    public float circleRadius = 2.5f;
    public float circleAngularSpeed = 180f; // degrees/sec
    public float circleDuration = 4f;

    [Header("Flee")]
    public Transform exitPoint;
    public float fleeSpeed = 3.5f;

    [Tooltip("ExitPoint'e varış mesafesi (XZ düzleminde ölçülür, Y farkı sayılmaz).")]
    public float arriveDistance = 1f;

    float angle;
    float timer;
    Vector3 circleCenter;

    void Start()
    {
        circleCenter = transform.position + circleCenterOffset;
    }

    void Update()
    {
        if (state == State.Done) return;

        if (state == State.Circle)
        {
            timer += Time.deltaTime;
            angle += circleAngularSpeed * Time.deltaTime;

            float rad = angle * Mathf.Deg2Rad;
            Vector3 target = circleCenter + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * circleRadius;

            MoveTowardsXZ(target, fleeSpeed);
            LookTowardsXZ(target);

            if (timer >= circleDuration)
                state = State.Flee;
        }
        else if (state == State.Flee)
        {
            if (exitPoint == null) return;

            Vector3 target = exitPoint.position;

            MoveTowardsXZ(target, fleeSpeed);
            LookTowardsXZ(target);

            // ✅ Y farkını saymadan (XZ) varış kontrolü
            if (DistanceXZ(transform.position, target) <= arriveDistance)
                state = State.Done;
        }
    }

    // --- Helpers ---

    static float DistanceXZ(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }

    void MoveTowardsXZ(Vector3 target, float speed)
    {
        Vector3 pos = transform.position;
        Vector3 t = target;

        // Sadece XZ'de hesapla
        pos.y = 0f;
        t.y = 0f;

        Vector3 dir = (t - pos);
        if (dir.sqrMagnitude < 0.0001f) return;

        Vector3 step = dir.normalized * speed * Time.deltaTime;

        // Gerçek pozisyonda sadece XZ hareket et, Y aynı kalsın
        transform.position += new Vector3(step.x, 0f, step.z);
    }

    void LookTowardsXZ(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion desired = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, desired, 10f * Time.deltaTime);
    }
}
