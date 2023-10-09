using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : Player
{
    private Rigidbody rigid;

    private Vector3 moveDirection;

    private bool IsDash = false;
    private bool IsWalk = false;
    private bool IsChop = false;
    private Animator animator;

    [Header("Dash")]
    [SerializeField]
    private float dashSpeed;
    private float applyDashSpeed;

    // 몇도 동안 대쉬를 할 것인가
    [SerializeField]
    private float dashTime;
    private WaitForSeconds dashTimeWaitForsecond;

    // 대쉬 쿨타임
    [SerializeField]
    private float dashCoolDownTime;
    private WaitForSeconds dashDelayWaitForSecond;

    private bool canDash;

    private Hand hand;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        applyDashSpeed = 1f;

        dashTimeWaitForsecond = new WaitForSeconds(dashTime);

        dashDelayWaitForSecond = new WaitForSeconds(dashCoolDownTime);

        canDash = true;

        hand = transform.GetChild(0).GetComponent<Hand>();

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveDirection * Speed * applyDashSpeed * Time.deltaTime);
        //Debug.Log(moveDirection.magnitude * applyDashSpeed);
    }

    public void OnMove(InputValue value)
    {
        if (!IsDash)
        {
            animator.SetBool("IsWalk", true);
        }
        Vector2 input = value.Get<Vector2>();
        moveDirection = new Vector3(input.x, 0f, input.y);
        transform.LookAt(transform.position + moveDirection);
        animator.SetBool("IsWalk", false);
    }

    public void OnGrabAndPut() // Space 
    {
        hand.GrabAndPut();
    }

    public void OnInteractAndThrow() // Ctrl
    {
        hand.InteractAndThorw();
    }

    public void OnDash()
    {
        if (canDash)
        {
            StartCoroutine(DashCoroutine());
            StartCoroutine(CoolDownDash());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        animator.SetBool("IsDash", true);
        IsDash = true;
        IsWalk = false;
        applyDashSpeed = dashSpeed;
        yield return dashTimeWaitForsecond;

        applyDashSpeed = 1f;
        animator.SetBool("IsDash", false);
    }

    private IEnumerator CoolDownDash()
    {
        yield return dashDelayWaitForSecond;

        canDash = true;
    }
}
