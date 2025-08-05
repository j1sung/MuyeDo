using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector2 minBounds; // 카메라 이동 가능 최소 좌표
    public Vector2 maxBounds; // 카메라 이동 가능 최대 좌표
    private Vector3 offset; 
    

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        //desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);
    }
}
