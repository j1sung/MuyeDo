using UnityEngine;

public class CameraMove_JS : MonoBehaviour
{
    public Transform target;
    public Vector2 minBounds; // 카메라 이동 가능 최소 좌표
    public Vector2 maxBounds; // 카메라 이동 가능 최대 좌표
    [SerializeField] private Vector3 offset = new Vector3(-0.02796247f, 0.678982f, -0.7336222f);    //플레이어와 카메라의 offset(normal vector)
    public float distance = 1; //카메라와 플레이어의 거리

    private bool justSnapped = false;
    
    private static CameraMove_JS _instance;

    public static CameraMove_JS instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬 내에 이미 존재하는 Move_JS를 찾음
                _instance = FindObjectOfType<CameraMove_JS>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    //    offset = transform.position - target.position;
    //    offset = offset.normalized;
    //    Debug.Log(offset.x + "," + offset.y + "," + offset.z);
        offset = new Vector3(-0.02796247f, 0.678982f, -0.7336222f);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + (offset*distance);

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        //desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        if (justSnapped)
        {
            transform.position = desiredPosition;
            justSnapped = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 3f);
        }

    }

    public void SnapToTarget()
    {
        if (target == null) return;

        Vector3 snapPosition = target.position + (offset*distance);
        snapPosition.x = Mathf.Clamp(snapPosition.x, minBounds.x, maxBounds.x);
        // snapPosition.y = Mathf.Clamp(snapPosition.y, minBounds.y, maxBounds.y);

        transform.position = snapPosition;
        justSnapped = true;
    }
}
