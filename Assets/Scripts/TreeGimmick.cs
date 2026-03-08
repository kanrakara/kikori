using UnityEngine;

public class TreeGimmick : MonoBehaviour
{
    // 動かしたいスプライトマスクをインスペクターで指定
    public GameObject maskObjectLeft;
    public GameObject maskObjectRight;
    // 切り倒し時、スプライトマスクの親子関係を解除するため指定しておく
    public MaskLeftControl maskLeftcontrol;
    public MaskRightControl maskRightcontrol;
    // スプライトマスクの初期位置を指定
    public float leftShift;
    public float rightShift;
    // スプライトマスクを移動させる距離の最大値
    public float maxMoveDistance;
    // スプライトマスクの移動した量
    private float nowOffsetLeft = 0.0f;
    private float nowOffsetRight = 0.0f;

    // この木の現在の体力
    public float currentHp;

    public GameObject treeVisual;       // 木の画像オブジェクト
    public float fallSpeed = 10f;      // 倒れる速さ
    private bool isFallen = false;
    private float targetZAngle = 0f;    // 最終的な回転角度
    private float currentFallSpeed = 0f; // 現在の落下速度
    public float acceleration = 200f;   // 加速度（値が大きいほど早く加速する）
    // 回転制御用、Quaternion.identityは、全て0に設定と同じ
    private Quaternion targetRotation = Quaternion.identity;

    public GameObject playerObj;    // PlayerObjectを取っておく
    public GameObject uiManager;    // UIManagerも同様

    private bool isCleared = false;  // エンディング重複実行防止用

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GameManagerで設定された木の最大体力を初期値として代入
        currentHp = GameManager.maxHp;

        // スプライトマスクの初期位置
        if (maskObjectLeft != null)
            maskObjectLeft.transform.localPosition = new Vector3(leftShift, 0, 0);

        if (maskObjectRight != null)
            maskObjectRight.transform.localPosition = new Vector3(rightShift, 0, 0);

    }


    // プレイヤーから呼び出すためのメソッド
    public void MaskGimmick(int layerInt)
    {
        // レイヤーの内容で条件分岐
        if (layerInt == LayerMask.NameToLayer("TreeLeft"))
        {
            if (maskObjectLeft != null)
            {
                // 木へのダメージ処理
                currentHp -= GameManager.axePower;
                // 切る音を鳴らす
                SoundManager.instance.PlaySE(SEType.TreeHit, 0.1f);
                // もしcurrentHPが0以下になったら、0になる
                currentHp = Mathf.Max(currentHp, 0);

                // スプライトマスクを移動させる
                nowOffsetLeft += maxMoveDistance * (GameManager.axePower / GameManager.maxHp);
                // 最大移動量を超えないように
                if (nowOffsetLeft > maxMoveDistance / 2) { nowOffsetLeft = maxMoveDistance / 2; }
                maskObjectLeft.transform.localPosition = new Vector3(nowOffsetLeft + leftShift, 0, 0);
            }
        }
        // Leftと同様に
        else if (layerInt == LayerMask.NameToLayer("TreeRight"))
        {
            if (maskObjectRight != null)
            {
                currentHp -= GameManager.axePower;
                SoundManager.instance.PlaySE(SEType.TreeHit, 0.1f);
                nowOffsetRight += maxMoveDistance * (GameManager.axePower / GameManager.maxHp);
                if (nowOffsetRight > maxMoveDistance / 2) { nowOffsetRight = maxMoveDistance / 2; }
                maskObjectRight.transform.localPosition = new Vector3(-nowOffsetRight + rightShift, 0, 0);
            }
        }

        // ダメージが85%以上で、木が倒れる
        if (currentHp < GameManager.maxHp * 0.15f)
        {

            PlayerScript playerStop = playerObj.GetComponent<PlayerScript>();
            playerStop.GameStop();

            // スプライトマスクの親子関係を解除し、下側の削り具合を確定させる
            maskLeftcontrol.DetachLeftParent();
            maskRightcontrol.DetachRightParent();

            // 木の倒れる音を鳴らす
            SoundManager.instance.PlaySE(SEType.TreeFall);
            // Debug.Log("木が倒れました！");
            // 倒れる処理。倒れるフラグが立ったら、Update 内で徐々に回転させる
            Fall(layerInt);
        }


    }

    // プロパティを使って、スプライトマスクの移動量が既定値以上かどうかの判定を返す
    public bool CheckOffsetLeft
    {
        get
        {
            // maxMoveDistanceの45%が移動量の最大、それ以外ならtrueを返す
            if (nowOffsetLeft > maxMoveDistance * 0.45f) { return true; }
            else { return false; }
        }
    }

    public bool CheckOffsetRight
    {
        get
        {
            if (nowOffsetRight > maxMoveDistance * 0.45f) { return true; }
            else { return false; }
        }
    }

    // 木が倒れる動き
    private void Fall(int hitLayer)
    {
        if (isFallen) return; // 二重発動防止
        isFallen = true;

        // 叩いた方向によって倒れる向きを決める
        // TreeLeftを叩いたら右に倒れる
        if (hitLayer == LayerMask.NameToLayer("TreeLeft"))
        {
            targetZAngle = -90f;
        }
        else
        {
            targetZAngle = 90f;
        }

        // 算出した角度をQuaternionに変換して代入
        targetRotation = Quaternion.Euler(0, 0, targetZAngle);

    }



    // Update is called once per frame
    void Update()
    {
        // 倒れるフラグが立っていたら回転させる
        if (isFallen && treeVisual != null)
        {
            // 速度を加速させる
            currentFallSpeed += acceleration * Time.deltaTime;

            // 回転させる
            treeVisual.transform.localRotation = Quaternion.RotateTowards(
                treeVisual.transform.localRotation,     // 現在の角度
                targetRotation,                         // 目標の角度（90度 or -90度）
                currentFallSpeed * Time.deltaTime       // このフレームで動かす量
            );

            // 完了判定
            // 現在の角度と目標の角度がどれくらい離れているかを計算
            if (Quaternion.Angle(treeVisual.transform.localRotation, targetRotation) < 0.1f)
            {
                treeVisual.transform.localRotation = targetRotation; // 角度をぴったり合わせる
                currentFallSpeed = 0f;
                isFallen = false;

                // 画面を揺らす（時間は0.3秒、強さは0.2）staticで、直接呼び出せるようにしてある
                if (CameraShake.instance != null)
                {
                    CameraShake.instance.Shake(0.3f, 0.2f);
                }

                // 着地音を鳴らす
                SoundManager.instance.PlaySE(SEType.TreeLand);

                // 報酬をプラス
                GameManager.money += GameManager.remuneration;

                // すでにクリア済みなら何もしない
                if (isCleared) return;

                // ゲームクリアチェック
                if (GameManager.money >= GameManager.clearMoney)
                {
                    // 条件を満たせば
                    isCleared = true;
                    // クリア演出実行
                    UIManager clear = uiManager.GetComponent<UIManager>();
                    clear.GameClear();
                }
                else
                {
                // ResultUIを動作させる
                UIManager resultOn = uiManager.GetComponent<UIManager>();
                resultOn.ShowResultUI();
                }
            }
        }
    }
}
