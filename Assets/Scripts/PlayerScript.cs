using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rbody;              // Rigidbody2D型の変数（自身の定義用）
    public float speed = 3.0f;      // 移動速度
    private float axisH = 0.0f;     // x方向の入力用

    private Animator animator;      // スプライト変更にAnimatorを使用する
    public SpriteRenderer spriteRenderer;       // 見た目を変えたい子オブジェクトのスプライト（斧）をインスペクターから指定
    private bool isLogging = false;     // アニメーション中かどうかのフラグ 

    public TreeGimmick treeGimmick;     // TreeGimmickの場所
    private bool isInArea = false;      // エリア内にいるかどうかのフラグ
    private GameObject currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidboody2Dを取ってくる
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // InputSystem により、On + Actionsの名前で呼び出せる。
    public void OnMove(InputValue value)
    {
        if (isLogging) return;      // 伐採中は移動入力を無視
        // 直接 Vector2 を受け取って、xの値を axisH に入れる
        axisH = value.Get<Vector2>().x;
    }


    // Update is called once per frame
    void Update()
    {
        // 向きの調整     
        if (axisH > 0.0f)
        {
            //Debug.Log("右が押されている");
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            //Debug.Log("左が押されている");
            transform.localScale = new Vector2(-1, 1);  // 左右反転させる
        }

    }

    private void FixedUpdate()
    {
        // 左右移動の速度を更新（y方向は現在の速度を維持して重力を邪魔しない）
        rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
    }

    // 木に接近中の見た目を変更し、エリア内かの判定を加える
    // コライダーに接触したら
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 接触した相手のタグを確認する
        if (other.CompareTag("Tree"))
        {
            // Animatorの"Contact"トリガーを起動
            animator.SetTrigger("Contact");
            // Contactlessのトリガーが残らないようリセット（念のため）
            animator.ResetTrigger("Contactless");
            isInArea = true;
            // currentTarget に相手情報を取得
            currentTarget = other.gameObject;
        }
    }

    // コライダーから離れた瞬間に呼ばれる、エリア内かの判定をfalseに
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tree"))
        {
            // "Contactless"トリガーを起動してIdleに戻す
            animator.SetTrigger("Contactless");
            // Contactのトリガーが残らないようリセット（念のため）
            animator.ResetTrigger("Contact");
            isInArea = false;
            // 相手から離れたら、currentTargetを空にする
            currentTarget = null;
        }
    }

    // 木を切るアクション
    public void OnAttack(InputValue value)
    {
        // アタックボタンが押された瞬間　＆　伐採中ではない ＆ エリア内 ＆ ターゲットあり
        if (value.isPressed && !isLogging && isInArea && currentTarget != null)
        {
            // 接触レイヤー名がTreeLeft且つ、マスクオブジェクトが規定の移動量を超えていれば（判定はあちらで行い済み）
            if (currentTarget.layer == LayerMask.NameToLayer("TreeLeft") && treeGimmick.CheckOffsetLeft)
            {
                // maskObjectRightを光らせたい
                return;
            }
            else if (currentTarget.layer == LayerMask.NameToLayer("TreeRight") && treeGimmick.CheckOffsetRight)
            {
                // maskObjectLeftを光らせたい
                return;
            }
            // 条件全てクリアで斧を振る
            else
            {
                // 直接指定、相手の親のコンポーネント
                TreeGimmick gimmick = currentTarget.GetComponentInParent<TreeGimmick>();
                if (gimmick != null)
                {
                    // 斧を振る際、playerの向きを訂正する
                    if (currentTarget.layer == LayerMask.NameToLayer("TreeLeft"))
                    {
                        transform.localScale = new Vector2(1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector2(-1, 1);
                    }

                    isLogging = true;
                    axisH = 0;      // 移動を止める
                    rbody.linearVelocity = new Vector2(0, rbody.linearVelocity.y);
                    // GameManagerのplayerPowerに応じてアニメーション速度を変更
                    animator.speed = GameManager.playerPower;
                    // Swingアニメーションを起動
                    animator.SetTrigger("Swing");
                }
            }
        }
    }

    // アニメーション中、斧が木に当たったときに発動させる
    public void OnLogging()
    {
        if (currentTarget != null)
        {
            TreeGimmick gimmick = currentTarget.GetComponentInParent<TreeGimmick>();
            if (gimmick != null)
            {
                // 接触している相手の「レイヤー番号」を引数として渡す
                gimmick.MaskGimmick(currentTarget.layer);
            }
        }

    }

    // アニメーションの終了時に発動させる
    public void OnLoggingComplete()
    {
        // 状態をリセット
        isLogging = false;
        animator.speed = 1.0f; // 速度を元に戻す
    }
}
