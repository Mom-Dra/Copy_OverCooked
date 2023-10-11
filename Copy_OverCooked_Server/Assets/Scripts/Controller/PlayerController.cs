using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    [SerializeField]
    private Player player;

    public void OnMove(InputValue value) // Move
    {
        Vector2 input = value.Get<Vector2>();
        player.SetMoveDirection(new Vector3(input.x, 0f, input.y));
    }

    public void OnGrabAndPut() // Space 
    {
        player.GrabAndPut();
    }

    public void OnInteractAndThrow() // Ctrl
    {
        player.InteractAndThrow();
    }

    public void OnDash() // Alt
    {
        player.Dash();
    }
}
