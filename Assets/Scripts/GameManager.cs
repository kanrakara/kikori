using UnityEngine;

public class GameManager : MonoBehaviour
{
    // staticにすることで、どこからでも GameManager.axePower でアクセスできる
    public static float axePower = 1.0f;
    public static float maxHp = 100.0f;

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
