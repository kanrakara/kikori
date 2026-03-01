using UnityEngine;

public class GameManager : MonoBehaviour
{
    // staticにすることで、どこからでも GameManager.でアクセスできる
    public static float axePower = 10.0f;        // 斧の強さ（木へのダメージ量）
    public static float maxHp = 100.0f;     // 木の最大体力
    public static float playerPower = 5.0f;     // playerの力（斧を振る速さ）

    // ゲーム開始時に初期化したい場合はここに書く
    void Awake()
    {
        // 必要に応じて保存データから読み込む処理など
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
