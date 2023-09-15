using UnityEngine;

public abstract class Player : Unit
{
    protected Animator animator;

    protected Hand hand;

    public Interactor GetInteractor()
    {
        return hand.interactor;
    }

    public void StartChopAnimation()
    {
        transform.Find("ChoppingArm").gameObject.SetActive(true);
        animator.SetBool("IsChop", true);
    }

    public void StopChopAnimation()
    {
        animator.SetBool("IsChop", false);
        transform.Find("ChoppingArm").gameObject.SetActive(false);
    }

    public void SetBoolAnimation(string animation, bool condition)
    {
        animator.SetBool(animation, condition);
    }

}
