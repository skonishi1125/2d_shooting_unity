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

        Debug.Log(transform.position.x);
        // 親が -width まで来たら、右に width だけ戻す
        if (transform.position.x <= -width)
        {
            transform.position += Vector3.right * width;
        }
    }
}
