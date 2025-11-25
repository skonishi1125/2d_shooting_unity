using UnityEngine;

public class ScrollingBackgroundGroup : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;
    [SerializeField] private float width; // 背景1枚分の横幅
    [SerializeField] private SpriteRenderer baseRenderer;

    private void Start()
    {
        if (baseRenderer == null)
            Debug.Log("横幅を指定するためのbaseRendererがセットされていません。");

        width = baseRenderer.bounds.size.x;
    }

    private void Update()
    {
        // 2枚含む親ごと、左へ動かす
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        // 親が -width まで来たら、右に width だけ戻す
        if (transform.position.x <= -width)
        {
            // (-19.2, 0) を (1, 0) * 19.2 = (19.2, 0) だけ加算して、 (0, 0) に戻す
            transform.position += Vector3.right * width;
        }
    }
}
