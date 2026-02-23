using UnityEngine;

public class TreeTopGimmick : MonoBehaviour
{
    // 動かしたいスプライトマスクをインスペクターで指定
    public GameObject maskObjectLeft;
    public GameObject maskObjectRight;

    // 移動させる距離や速度を調整できるようにする
    public Vector3 maskMove = new Vector3(2f, 0f, 0f);
    public float moveDuration = 1.0f;

    // プレイヤーから呼び出すためのメソッド
    public void MaskGimmick(int layerInt)
    {
        // レイヤー番号によって処理を分岐させる
        // レイヤー7（TreeLeft）
        if(layerInt == 7)
        {
            //maskObjectLeft.transform.localPosition += maskMove;
            Debug.Log("7取得" );
        }
        // レイヤー8（TreeRight）
        else if(layerInt == 8)
        {
            Debug.Log("8取得" );
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
