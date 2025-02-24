using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float up, down, left, right;
    public float width, height;
    public bool drawHelperGizmos;

    void Update()
    {
        Vector3 pos = player.transform.position;
        Vector3 newpos = new Vector3(0, 0, transform.position.z);

        // the camera will track the player while staying within the set borders
        newpos.y = Mathf.Min(Mathf.Max(pos.y, down + (height / 2)), up - (height / 2));
        newpos.x = Mathf.Min(Mathf.Max(pos.x, left + (width / 2)), right - (width / 2));

        transform.position = newpos;
    }

    private void OnDrawGizmos()
    {
        // This will draw little boxes representing the borders of the camera area
        if (drawHelperGizmos)
        {
            Gizmos.DrawCube(new Vector2(left + (right - left) / 2, up + .5f), new Vector2(1, 1)); // up
            Gizmos.DrawCube(new Vector2(left + (right - left) / 2, down - .5f), new Vector2(1, 1)); // down
            Gizmos.DrawCube(new Vector2(left - .5f, down + (up - down) / 2), new Vector2(1, 1)); // left
            Gizmos.DrawCube(new Vector2(right + .5f, down + (up - down) / 2), new Vector2(1, 1)); // right
            Gizmos.DrawWireCube(transform.position, new Vector2(width, height));
        }
    }
}
