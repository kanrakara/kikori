using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // カメラを揺らすエフェクト
    // どこからでも CameraShake.instance.Shake() で呼べるようにする
    public static CameraShake instance;

    void Awake() { instance = this; }

    // ある程度の時間をかけて行いたいので、コルーチンを使う
    // durationで動かす時間、magnitudeで動かす大きさ
    public void Shake(float duration = 0.2f, float magnitude = 0.1f)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    // IEnumeratorはコルーチンの型
    private IEnumerator DoShake(float duration, float magnitude)
    {
        Vector3 pos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // ランダムにカメラをずらす
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;
            // ここで処理を中断、1フレーム待つ指示
            yield return null;
        }

        // 元の位置に戻す
        transform.localPosition = pos; 
    }
}
