using UnityEngine;
using UnityEngine.AI;

public class CatRunToHole : MonoBehaviour
{
    public Transform holePoint;
    public float reachDistance = 0.5f;
    [SerializeField] private Animator anim;
    
    NavMeshAgent agent;
    bool running = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true; // başta beklesin
    }

    public void StartRun()
    {
        if (agent == null || holePoint == null) return;
        running = true;
        agent.isStopped = false;
        if (anim == null)
        {
            if (TryGetComponent<Animator>(out var animator))
            {
                animator.SetFloat("Vert", 1);
                animator.SetFloat("State", 1);
            }
        }
        else
        {
            anim.SetFloat("Vert", 1);
            anim.SetFloat("State", 1);
        }

        agent.SetDestination(holePoint.position);
    }

    void Update()
    {

        if (!running || agent == null) return;

        if (!agent.pathPending && agent.remainingDistance <= reachDistance)
        {
            // deliğe ulaştı
            SendMessage("OnCatReachedHole", SendMessageOptions.DontRequireReceiver);
            running = false;
        }
    }
}
