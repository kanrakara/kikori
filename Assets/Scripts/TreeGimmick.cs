using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TreeGimmick : MonoBehaviour
{
    // 動かしたいスプライトマスクをインスペクターで指定
    public GameObject maskObjectLeft;
    public GameObject maskObjectRight;

    //スプライトマスクの初期位置を指定
    public float leftShift;
    public float rightShift;

    //スプライトマスクの移動した量
    private float nowOffsetLeft = 0.0f;
    private float nowOffsetRight = 0.0f;

    // この木の現在の体力
    public float currentHp;

    // スプライトマスクを移動させる距離の最大値
    public float maxMoveDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GameManagerで設定された木の最大体力を初期値として代入
        currentHp = GameManager.maxHp;

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
            // maxMoveDistanceの45%が移動量の最大
            if (nowOffsetLeft > maxMoveDistance * 0.45f)
            {
                // maskObjectRightを光らせる
                return;
            }
            else if (maskObjectLeft != null)
            {
                // 木へのダメージ処理
                currentHp -= GameManager.axePower;
                // もしcurrentHPが0以下になったら、0になる
                currentHp = Mathf.Max(currentHp, 0);

                // スプライトマスクを移動させる
                nowOffsetLeft += maxMoveDistance * (GameManager.axePower / GameManager.maxHp);
                maskObjectLeft.transform.localPosition = new Vector3(nowOffsetLeft + leftShift, 0, 0);
            }
        }
        // Leftと同様に
        else if (layerInt == LayerMask.NameToLayer("TreeRight"))
        {
            if (nowOffsetRight > maxMoveDistance * 0.45f)
            {
                //mashObjectLeftを光らせる
                return;
            }
            else if (maskObjectRight != null)
            {
                currentHp -= GameManager.axePower;

                nowOffsetRight += maxMoveDistance * (GameManager.axePower / GameManager.maxHp);
                maskObjectRight.transform.localPosition = new Vector3(-nowOffsetRight + rightShift, 0, 0);
            }
        }

        // ダメージが85%以上で、木が倒れる
        if (currentHp < GameManager.maxHp * 0.15f)
        {
            Debug.Log("木が倒れました！");
        }

    }




    // Update is called once per frame
    void Update()
    {

    }
}
