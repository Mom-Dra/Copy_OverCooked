using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        applyDashSpeed = 1f;

        dashTimeWaitForsecond = new WaitForSeconds(dashTime);

        dashDelayWaitForSecond = new WaitForSeconds(dashCoolDownTime);

        canDash = true;
    }

    private void FixedUpdate()
    {
        if(hand != null){
            hand.transform.position = transform.position + (transform.forward * 0.8f);
        }

        rigid.MovePosition(rigid.position + moveDirection * speed * applyDashSpeed * Time.deltaTime);
        Debug.Log(moveDirection.magnitude * applyDashSpeed);
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveDirection = new Vector3(input.x, 0f, input.y);
    }

    public void OnGrabAndPut()
    {
        if (hand != null)
        {
            // Put
        }

        Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), transform.forward * distance, Color.red, 3.0f);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Interactable");
        if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0), transform.forward * distance, out hit, 2, layerMask))
        {
            Debug.Log("Interact!");
            IObject target = hit.transform.GetComponent<IObject>();
            if (target.IsGrabable)
            {
                // Grab
            }
        }
    }

    public void OnInteractAndThrow()
    {
        Debug.Log("InteractAndThrow");
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
