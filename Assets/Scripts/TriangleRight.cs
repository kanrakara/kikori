using UnityEngine;

public class TriangleRightt : MonoBehaviour
{
    public float fadeDuration = 2.0f;
    private SpriteRenderer sr;
    private float timer = 0.0f;


    public void CheckRight()
    {
        sr = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
        // 全てのInvokeをキャンセル
        CancelInvoke();
        timer = 0.0f;
        gameObject.SetActive(true);
        // 指定した秒数後にGameObject自体の無効
        Invoke("OffSpriteRight", fadeDuration);
    }

    void OffSpriteRight()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        // アルファ値を時間の経過に合わせて直接計算する
        Color colorRight = sr.color;
        // Mathf.Lerp は、2つの値の間を割合（％）で指定して取り出す関数で、(最大値,最小値,割合)
        // 第3引数の「割合」に応じて、開始値から終了値に向かって値を返す。折角なので使っただけ。
        colorRight.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        sr.color = colorRight;
    }
}
