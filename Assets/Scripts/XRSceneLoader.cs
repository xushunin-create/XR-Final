using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSceneLoader : MonoBehaviour
{
    public string sceneName;

    void Start()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>()
            .selectEntered.AddListener(OnClick);
    }

    void OnClick(SelectEnterEventArgs args)
    {
        SceneManager.LoadScene(sceneName);
    }
}