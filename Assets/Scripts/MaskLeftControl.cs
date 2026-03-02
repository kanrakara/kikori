using UnityEngine;

public class MaskLeftControl : MonoBehaviour
{
    // こちらで事前に相手を指定しておく
    public Transform targetToDetach;
    // true ワールド座標を維持、falseにするとローカル値を基準に再計算
    public bool keepWorldPosition = true;

    // オブジェクトの親子関係の解除
    public void DetachLeftParent()
    {
        // 念の為nullチェック
        if (targetToDetach == null)
        {
            return;
        }
        // 親子関係の解除
        targetToDetach.SetParent(null, keepWorldPosition);
    }

}
