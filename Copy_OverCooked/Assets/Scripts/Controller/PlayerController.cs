using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;



public class PlayerController : Player
{
    private Rigidbody rigid;

    private Vector3 moveDirection;

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
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveDirection * speed * applyDashSpeed * Time.deltaTime);
        //Debug.Log(moveDirection.magnitude * applyDashSpeed);
    }

    

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDirection = new Vector3(input.x, 0f, input.y);
        transform.LookAt(transform.position + moveDirection);
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
            canDash = false;
            applyDashSpeed = dashSpeed;

            StartCoroutine(ResetDashSpeedCoroutine());
            StartCoroutine(CoolDownDash());
        }
    }

    private IEnumerator ResetDashSpeedCoroutine()
    {
        yield return dashTimeWaitForsecond;

        applyDashSpeed = 1f;
    }

    private IEnumerator CoolDownDash()
    {
        yield return dashDelayWaitForSecond;

        canDash = true;
    }
}
