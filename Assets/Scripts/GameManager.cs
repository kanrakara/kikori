using UnityEngine;

public class GameManager : MonoBehaviour
{
    // staticにすることで、どこからでも GameManager.でアクセスできる
    public static float axePower = 10.0f;        // 斧の強さ（木へのダメージ量）
    public static float maxHp = 100.0f;     // 木の最大体力
    public static int remuneration = 10000;     // 報酬
    public static float playerPower = 5.0f;     // playerの力（斧を振る速さ）
    public static int money = 30000;        // 現在のお金
    public static int clearMoney = 1000000;     // クリア条件

    // ゲーム開始時に初期化したいものがあれば
    void Awake()
    {
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // BGMを鳴らす
        SoundManager.instance.PlayBGM(BGMType.Field);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
