using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector2 minBounds; // ī�޶� �̵� ���� �ּ� ��ǥ
    public Vector2 maxBounds; // ī�޶� �̵� ���� �ִ� ��ǥ
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
