using UnityEngine;
using UnityEngine.InputSystem;


// 결국 이건 서버측 코드에서 필요 없음
// 어차피 키보드 Input은 클라이언트에서 보내줄 것이기 때문 
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

    public void OnCtrlKeyUp()
    {
        player.CtrlKeyUp();
    }

    public void OnDash() // Alt
    {
        player.Dash();
    }
}
