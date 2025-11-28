using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void OnClickReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
