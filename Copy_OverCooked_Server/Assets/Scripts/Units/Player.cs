using System.Collections;
using UnityEngine;

public enum EAnimationType
{
    Dash,
    Chop,


}

public class Player : MonoBehaviour
{
    [Header("Info")]
    [SerializeField]
    private int Id;

    private Rigidbody rigid;
    private Vector3 moveDirection;


    [Header("Status")]
    [SerializeField]
    private float Speed;

    [SerializeField]
    private float dashSpeed;
    private float applyDashSpeed;


    [SerializeField]
    private float dashTime;
    private WaitForSeconds dashTimeWaitForsecond;


    [SerializeField]
    private float dashCoolDownTime;
    private WaitForSeconds dashDelayWaitForSecond;

    private bool isDashable;


    private Hand hand;
    private Animator animator;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        applyDashSpeed = 1f;

        dashTimeWaitForsecond = new WaitForSeconds(dashTime);
        dashDelayWaitForSecond = new WaitForSeconds(dashCoolDownTime);

        isDashable = true;

        hand = GetComponentInChildren<Hand>();
        //hand.SetPlayer(this);

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveDirection * Speed * applyDashSpeed * Time.deltaTime);
    }


    public void SetBoolAnimation(EAnimationType animationType, bool condition)
    {
        switch (animationType)
        {
            case EAnimationType.Dash:
                break;
            case EAnimationType.Chop:
                transform.Find("ChoppingArm").gameObject.SetActive(condition);
                break;
        }

        animator.SetBool($"Is{animationType}", condition);
    }


    public void SetMoveDirection(Vector3 moveDirection)
    {
        this.moveDirection = moveDirection;
        transform.LookAt(transform.position + moveDirection);
    }


    public void GrabAndPut()
    {
        hand.GrabAndPut();
    }

    public void InteractAndThrow()
    {
        hand.InteractAndThrow();
    }

    public void Dash()
    {
        if (isDashable)
        {
            StartCoroutine(DashCoroutine());
            StartCoroutine(CoolDownDash());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashable = false;
        //animator.SetBool("IsDash", true);
        applyDashSpeed = dashSpeed;
        yield return dashTimeWaitForsecond;

        applyDashSpeed = 1f;
        //animator.SetBool("IsDash", false);
    }

    private IEnumerator CoolDownDash()
    {
        yield return dashDelayWaitForSecond;

        isDashable = true;
    }
}
