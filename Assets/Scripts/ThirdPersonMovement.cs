using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -20f;

    [Header("Animation")]
    public Animator animator;                 // Boş bırakabilirsin (otomatik bulur)
    public string isMovingParam = "isMoving"; // Animator'daki bool parametre adı

    private CharacterController cc;
    private Vector3 velocity;
    private Camera cam;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        cam = Camera.main;

        // Animator'ı otomatik bul (model child'da ise de bulur)
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (cam == null) cam = Camera.main;

        MoveAndAnimate();
    }

    void MoveAndAnimate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool isMoving = Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f;

        // Animasyon parametresi
        if (animator != null)
            animator.SetBool(isMovingParam, isMoving);

        // Hareket
        if (isMoving && cam != null)
        {
            Vector3 camForward = cam.transform.forward;
            Vector3 camRight = cam.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            Vector3 moveDir = (camForward.normalized * v + camRight.normalized * h).normalized;

            if (moveDir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            cc.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        // Gravity
        if (cc.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }
}
