using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropItemPrefab;

    private void Start()
    {
        if (dropItemPrefab == null)
        {
            Debug.LogWarning("EnemyDrop: dropItemPrefabが未設定です");
        }
    }

    // 敵に割り当てるコンポーネントなので、transformに落とせばよい
    public void SpawnDrop()
    {
        Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
    }

}
