using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;

    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private Vector3 originalRotation;

    public enum BillboardType { LookAtCamera, CameraForward };

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles;
    }

    void LateUpdate()
    {
        switch (billboardType)
        {
            case BillboardType.LookAtCamera: // 카메라 위치를 향한 방향으로 회전
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                transform.Rotate(0, 180, 0);
                break;
            case BillboardType.CameraForward: // 카메라가 보는 방향으로 회전
                transform.forward = Camera.main.transform.forward;
                transform.Rotate(0, 180, 0);
                break;
            default:
                break;

        }

        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);

        // 카메라가 보는 방향으로 회전 다른 구현
        //transform.LookAt(transform.position + Camera.main.transform.forward);
        //transform.Rotate(0, 180, 0);

        // 카메라 회전 그대로 적용
        //transform.rotation = Camera.main.transform.rotation; 
    }
}
