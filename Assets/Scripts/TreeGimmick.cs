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

    //スプライトマスクの現在の移動量
    private float nowOffsetLeft = 0.0f;
    private float nowOffsetRight = 0.0f;

    // この木自身の現在の体力
    public float currentHp;

    // 移動させる距離を調整できるようにする
    public float maxMoveDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GameManagerで設定された最大体力を初期値として代入
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
            if (nowOffsetLeft > maxMoveDistance * 0.45f)
            {
                //maskObjectRightを光らせる
                return;
            }
            else if (maskObjectLeft != null)
            {
                // ダメージ処理
                currentHp -= GameManager.axePower;
                currentHp = Mathf.Max(currentHp, 0);

                nowOffsetLeft += maxMoveDistance * (GameManager.axePower / GameManager.maxHp);
                maskObjectLeft.transform.localPosition = new Vector3(nowOffsetLeft + leftShift, 0, 0);
            }
        }
        else if (layerInt == LayerMask.NameToLayer("TreeRight"))
        {
            if (nowOffsetRight > maxMoveDistance * 0.45f)
            {
                //mashObjectLeftを光らせる
                return;
            }
            else if (maskObjectRight != null)
            {
                // ダメージ処理
                currentHp -= GameManager.axePower;

                nowOffsetRight += maxMoveDistance * (GameManager.axePower / GameManager.maxHp);
                maskObjectRight.transform.localPosition = new Vector3(-nowOffsetRight + rightShift, 0, 0);
            }
        }

        if (currentHp < GameManager.maxHp * 0.1f)
        {
            Debug.Log("木が倒れました！");
        }

    }




    // Update is called once per frame
    void Update()
    {

    }
}
