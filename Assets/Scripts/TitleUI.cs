using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleMessage;
    [SerializeField] private CanvasGroup startButtonGroup;

    private void Awake()
    {
        titleMessage.alpha = 0f;
        startButtonGroup.alpha = 0f;

        // alpha 1f に、0.6f かけて変化
        // SetEase: 最初早くて、最後ゆっくり止まる
        titleMessage.DOFade(1f, 0.6f).SetEase(Ease.OutQuad);

        var rt = (RectTransform)startButtonGroup.transform;
        var originalPos = rt.anchoredPosition;

        // 初期位置を少し下げておいて、引き上げる
        rt.anchoredPosition = originalPos + new Vector2(0f, -80f);
        //  originalPos へ 2f かけて, SetEaseの挙動で、 0.1f ディレイをかけて動作させる
        rt.DOAnchorPos(originalPos, 2f).SetEase(Ease.OutCubic).SetDelay(0.1f);

        // フェード
        startButtonGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad).SetDelay(0.1f);

    }
    public void OnClickStart()
    {
         SceneManager.LoadScene("Stage1");
    }

}
