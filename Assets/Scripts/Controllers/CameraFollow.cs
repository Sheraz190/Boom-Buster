using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform player;
    private Vector3 _offset = new Vector3(2, 0, -10);
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    private float _speed = 1.5f;
    #endregion

    private void FixedUpdate()
    {
        if(player==null)
        {
            return;
        }
        Vector3 targetPos = new Vector3(player.position.x + 8, 0, -10);
        Vector3 cameraPos = Vector3.Lerp(transform.position, targetPos, _speed);
        float clampedX = Mathf.Clamp(cameraPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(cameraPos.y, minBounds.y, maxBounds.y);
        transform.position = new Vector3(clampedX, clampedY, cameraPos.z);
    }
}
