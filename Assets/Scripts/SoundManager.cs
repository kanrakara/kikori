using UnityEngine;
using System.Collections.Generic;

// 音の種類を定義、enumで列挙型とし、プログラム内で名前を打ち間違えた時点でエラーが出るようにする
public enum BGMType { None, Title, Field }
public enum SEType { TreeHit, TreeFall, TreeLand, Clear }

public class SoundManager : MonoBehaviour
{
    // 自分自身をstaticの型にしてしまう。クラスを元に実体化したクラス（シングルトン）
    // 実体化した自分自身をstaticなものとして全シーンで唯一無二のクラスとして扱える。
    public static SoundManager instance;

    // struct (構造体)型を使用する。名前（enum）と音（Clip）をセットで管理するため
    // struct はインスペクターに現れないので、[System.Serializable]使用
    [System.Serializable]
    public struct BGMData { public BGMType type; public AudioClip clip; }
    [System.Serializable]
    public struct SEData { public SEType type; public AudioClip clip; }

    public List<BGMData> bgmList;
    public List<SEData> seList;

    private AudioSource bgmSource;
    private AudioSource seSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // インスペクターで追加した2つのAudioSourceを取得する
            AudioSource[] sources = GetComponents<AudioSource>();

            // 1つ目をBGM、2つ目をSEとして割り当てる
            if (sources.Length >= 2)
            {
                bgmSource = sources[0];
                seSource = sources[1];
            }
        }
        // 他のシーンなどで別のSoundManagerがstatic化していたら(elseで)Destroyする（シーンごとにSoundManagerを置いておきたい）
        else { Destroy(gameObject); }
    }


    // BGM再生
    public void PlayBGM(BGMType type)
    {
        BGMData data = bgmList.Find(b => b.type == type);
        if (data.clip != null)
        {
            if (bgmSource.clip == data.clip) return;        // 同じ曲なら何もしない
            bgmSource.clip = data.clip;
            bgmSource.Play();
        }
    }


    // SE再生、ピッチ変化を組み込む。引数にデフォルト値0を設定し、指定がなければピッチ変化が起こらない
    public void PlaySE(SEType type, float pitchRandomness = 0f)
    {
        SEData data = seList.Find(s => s.type == type);
        if (data.clip != null)
        {
            // ピッチ変化、第2引数で揺れ幅を指定、引数省略で0
            seSource.pitch = 1.0f + Random.Range(-pitchRandomness, pitchRandomness);
            // SEなので、一度鳴らすだけ
            seSource.PlayOneShot(data.clip);
        }
    }


    // BGMを止める
    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            bgmSource.clip = null;      // 次回再生時にスムーズに開始するためクリア
        }
    }


}
