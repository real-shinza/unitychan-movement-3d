using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private float locomotionSpeed;
    private InputAction moveAction;
    private readonly int speedHash = Animator.StringToHash("Speed");

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        // 入力値
        var input = moveAction.ReadValue<Vector2>();

        // 入力方向をワールド空間に変換
        var move = new Vector3(input.x, 0f, input.y);

        // 移動入力の大きさを取得（小さなノイズは0として扱う）
        var moveAmount = move.magnitude > 0.1f ? move.magnitude : 0f;

        // 実際の移動
        characterController.Move(move * moveAmount * locomotionSpeed * Time.fixedDeltaTime);

        // 向きを移動方向に合わせる
        if (move.sqrMagnitude > 0.1f)
        {
            var targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // アニメーション操作
        animator.SetFloat(speedHash, moveAmount);
    }
}
