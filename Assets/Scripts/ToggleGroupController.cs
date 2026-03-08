using UnityEngine;
using UnityEngine.UI;

public static class GameData
{
    // 0, 1, 2 のいずれかを保存（デフォルトは0番目）
    public static int selectedToggleIndex = 0;
}


public class ToggleGroupController : MonoBehaviour
{
    public Toggle[] toggles; // インスペクターで3つ登録



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン開始時に保存された状態を復元
        RestoreToggleState();

        // 各トグルに「クリックされた時の命令」を順番に覚えさせる
        for (int i = 0; i < toggles.Length; i++)
        {
            // C#の仕様上、ループ内のiを直接使うとバグるため、コピーを作成
            int currentIndex = i;

            // 「値が変わった時(onValueChanged)」に「自作のメソッド」を実行するよう登録
            toggles[i].onValueChanged.AddListener((bool isOn) =>
            {
                // ONになった時だけ保存処理を行う
                if (isOn)
                {
                    SaveSelection(currentIndex);
                }
            });
        }
    }

    // 保存されているインデックスをUIに反映させる専用メソッド
    void RestoreToggleState()
    {
        int savedIndex = GameData.selectedToggleIndex;

        // 保存された番号が配列の範囲内かチェック（念のため）
        if (savedIndex >= 0 && savedIndex < toggles.Length)
        {
            toggles[savedIndex].isOn = true;
        }
    }

    // 選ばれた番号をStatic変数に書き込む専用メソッド
    void SaveSelection(int index)
    {
        GameData.selectedToggleIndex = index;
        //Debug.Log($"トグル {index} 番が選択されたので保存しました！");
    }
}
