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
    [SerializeField]
    private Transform cameraTransform;

    private InputAction moveAction;
    private readonly int speedHash = Animator.StringToHash("Speed");

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    private void FixedUpdate()
    {
        // 入力値
        var input = moveAction.ReadValue<Vector2>();

        // カメラの向き
        var forward = cameraTransform.forward;  // Z軸
        var right = cameraTransform.right;      // X軸

        // 上下方向の成分を除去
        forward.y = 0f;
        right.y = 0f;

        // 入力値とカメラの向きに応じて移動ベクトルを計算
        var moveVec = forward * input.y + right * input.x;

        // 移動入力の大きさを取得（小さなノイズは0として扱う）
        var moveMag = moveVec.magnitude > 0.1f ? moveVec.magnitude : 0f;

        // 実際の移動
        characterController.Move(moveVec * moveMag * locomotionSpeed * Time.fixedDeltaTime);

        if (moveVec.sqrMagnitude > 0.1f)
        {
            // 向きを移動方向に合わせる
            var targetRotation = Quaternion.LookRotation(moveVec);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // アニメーション操作
        animator.SetFloat(speedHash, moveMag);
    }
}
