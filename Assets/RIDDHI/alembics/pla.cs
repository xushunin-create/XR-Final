using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

public class Pla : MonoBehaviour
{
    public AlembicStreamPlayer player;
    public float speed = 1f;

    void Update()
    {
        if (player == null) return;

        float duration = player.EndTime - player.StartTime;

        player.CurrentTime += Time.deltaTime * speed;

        if (player.CurrentTime >= player.EndTime)
        {
            player.CurrentTime = player.StartTime;
        }
    }
}