using System.Collections;
using UnityEngine;

public class FaceEasyDemo : MonoBehaviour
{
    public Transform avatar;
    public Transform anchor;
    public Transform spawnPoint;

    private bool isOut = false;

    void Start()
    {
        avatar.position = anchor.position;
    }

    public void OnButtonClick()
    {
        if (!isOut)
            StartCoroutine(MoveTo(spawnPoint.position));
        else
            StartCoroutine(MoveTo(anchor.position));

        isOut = !isOut;
    }

    IEnumerator MoveTo(Vector3 target)
    {
        float t = 0;
        Vector3 start = avatar.position;

        while (t < 1)
        {
            t += Time.deltaTime * 1.5f;
            float smooth = Mathf.SmoothStep(0, 1, t);
            avatar.position = Vector3.Lerp(start, target, smooth);
            yield return null;
        }
    }
}