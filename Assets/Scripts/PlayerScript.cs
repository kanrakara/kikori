using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rbody;              // Rigidbody2D型の変数
    public float speed = 3.0f;      // 移動速度
    private float axisH = 0.0f;     // x方向の入力

    // 見た目を変えたい子オブジェクトのスプライトをインスペクターから入れる
    public SpriteRenderer spriteRenderer;
    private int activeOrder = 22;       // 接触中のレイヤー順序
    private int defaultOrder = 18;      // 通常時のレイヤー順序

    private bool isInArea = false; // エリア内にいるかどうかのフラグ
    private GameObject currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidboody2Dを取ってくる
        rbody = GetComponent<Rigidbody2D>();
    }


    // InputSystem により、On + Actionsの名前で呼び出せる。
    public void OnMove(InputValue value)
    {
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
    // コライダーに接触中
    private void OnTriggerEnter2D(Collider2D other)
    {

        // 接触した相手のタグを確認する
        if (other.CompareTag("Tree"))
        {
            spriteRenderer.sortingOrder = activeOrder;
            isInArea = true;
            // currentTarget に相手情報を取得
            currentTarget = other.gameObject;
        }
    }

    // コライダーから離れた瞬間に呼ばれる
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tree"))
        {
            spriteRenderer.sortingOrder = defaultOrder;
            isInArea = false;
            // 相手から離れたら、currentTargetを空にする
            currentTarget = null;
        }
    }

    // 木を切るアクション
    public void OnAttack(InputValue value)
    {
        // アタックボタンが押された瞬間、かつ、エリア内にいる時だけ実行
        if (value.isPressed && isInArea && currentTarget != null)
        {
            TreeGimmick gimmick = currentTarget.GetComponentInParent<TreeGimmick>();
            if (gimmick != null)
            {
                // 接触している相手の「レイヤー番号」を引数として渡す
                int layer = currentTarget.layer;
                gimmick.MaskGimmick(layer);
                LoggingAction();
            }
        }
    }

    private void LoggingAction()
    {

    }
}
