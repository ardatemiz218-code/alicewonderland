using UnityEngine;
using UnityEngine.AI;

public class RabbitGuideController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform holeTarget; // tavşan deliği cube (target)

    [Header("Player Move Detect")]
    public float moveThreshold = 0.02f; // player hareket sayılır eşiği
    public float checkInterval = 0.05f;

    [Header("Animation")]
    public Animator animator;
    public string speedParam = "Speed"; // Animator’da varsa
    public string runBoolParam = "IsRunning"; // Animator’da varsa

    private NavMeshAgent agent;
    private Vector3 lastPlayerPos;
    private float timer;

    private bool hasSpeedParam;
    private bool hasRunBoolParam;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        // Animator param kontrol (yoksa hata vermesin)
        if (animator != null)
        {
            foreach (var p in animator.parameters)
            {
                if (p.name == speedParam) hasSpeedParam = true;
                if (p.name == runBoolParam) hasRunBoolParam = true;
            }
        }
    }

    private void Start()
    {
        if (player != null) lastPlayerPos = player.position;

        if (agent != null)
        {
            agent.isStopped = true; // başta dur
        }
        SetAnimMoving(false);
    }

    private void Update()
    {
        if (player == null || holeTarget == null || agent == null) return;

        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        float movedDist = Vector3.Distance(player.position, lastPlayerPos);
        bool playerMoving = movedDist > moveThreshold;

        lastPlayerPos = player.position;

        if (playerMoving)
        {
            agent.isStopped = false;
            agent.SetDestination(holeTarget.position);
            SetAnimMoving(true);
        }
        else
        {
            agent.isStopped = true;
            SetAnimMoving(false);
        }
    }

    private void SetAnimMoving(bool moving)
    {
        if (animator == null) return;

        if (hasRunBoolParam)
            animator.SetBool(runBoolParam, moving);

        if (hasSpeedParam)
            animator.SetFloat(speedParam, moving ? 1f : 0f);
    }

    // Deliğe girince çağıracağız
    public void HideRabbit()
    {
        gameObject.SetActive(false);
    }
}

