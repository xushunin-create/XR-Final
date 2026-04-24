using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour
{
    public string targetScene;   // 在Inspector里填

    public void LoadScene()
    {
        SceneManager.LoadScene(targetScene);
    }
}