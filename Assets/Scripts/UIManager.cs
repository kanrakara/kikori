using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;        // UIで表示用、Textオブジェクトをアタッチ

    // 動かしたいUI(Result)　
    // RectTransformはUI専用のTransformで画面サイズが変わってもレイアウトを崩さない
    public RectTransform targetUI;

    // インスペクターから各ボタンをアタッチ
    public Button axeButton;
    public Button speedButton;
    public Button anzenButton;

    public ParticleSystem snowSystem; // Inspectorで雪のパーティクルをアタッチ

    public void ShowResultUI()
    {
        // DOTween を使って移動を作成する
        // 念の為UIを画面外の所定の位置に移動させておく
        targetUI.anchoredPosition = new Vector2(800f, 0f);
        // 0.7秒かけて(270, 0)まで移動させる　.SetEase(Ease.OutBack) をつけると、少し行き過ぎて戻る動きになる
        targetUI.DOAnchorPos(new Vector2(270f, 0f), 0.7f).SetEase(Ease.OutBack);
    }



    // Update is called once per frame
    void Update()
    {
        // 毎フレーム、static変数の値を文字列にしてテキストに代入、3桁区切り表示ToString("N0")
        goldText.text = "所持金: " + GameManager.money.ToString("N0") + " 円";

        axeButton.interactable = (GameManager.money >= 30000);
        speedButton.interactable = (GameManager.money >= 5000);
    }

    public void ResetScene()
    {
        // 現在アクティブなシーンの名前を取得してロードし直す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleLow()
    {
        GameManager.maxHp = 100f;
        GameManager.remuneration = 10000;
    }

    public void ToggleMiddle()
    {
        GameManager.maxHp = 300f;
        GameManager.remuneration = 50000;
    }
    public void ToggleHigh()
    {
        GameManager.maxHp = 1000f;
        GameManager.remuneration = 200000;
    }

    public void PlayerPowerUP()
    {
        GameManager.money -= 5000;
        GameManager.playerPower += 0.5f;
    }

    public void AxePowerUP()
    {
        GameManager.money -= 30000;
        GameManager.axePower += 5.0f;
    }

    public void Anzen()
    {
        GameManager.money += 800000;
    }

    public void GameClear()
    {
        // BGMを止める
        SoundManager.instance.StopBGM();
        // クリアSEを鳴らす
        SoundManager.instance.PlaySE(SEType.Clear);
        // 雪を降らせる
        StartCoroutine(SnowRoutine());
    }
    private IEnumerator SnowRoutine()
    {
        // 雪を降らせる
        snowSystem.Play();
        //Debug.Log("雪が降り始めました");
        {
            // 6秒(SE分の時間)待機
            yield return new WaitForSecondsRealtime(6.0f);

            // 雪を止める（新規の放出をストップし、残っている雪は消えるまで待つ）
            snowSystem.Stop();
            //Debug.Log("雪が止まりました");

            // シーンの移動
            SceneManager.LoadScene("Ending");
        }
    }
}
