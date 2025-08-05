using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake_JS : MonoBehaviour
{
    [SerializeField] float setforce = 0f;
    [SerializeField] float setShakeTime; // shake 시간 설정 변수
    [SerializeField] Vector3 setOffsetPos = Vector3.zero;
    [SerializeField] Vector3 setOffsetRot = Vector3.zero;

    float shakeTime; // 설정된 시간동안 shake

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnShakeCameraPos(setShakeTime);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            OnShakeCameraRot(setShakeTime);
        }
    }

    public void OnShakeCameraPos(float shakeTime = 0.1f)
    {
        this.shakeTime = shakeTime;

        StopCoroutine(ShakeByRotation());
        StopCoroutine(ShakeByPosition());
        StartCoroutine(ShakeByPosition());
    }

    public void OnShakeCameraRot(float shakeTime = 0.1f)
    {
        this.shakeTime = shakeTime;

        StopCoroutine(ShakeByRotation());
        StopCoroutine(ShakeByPosition());
        StartCoroutine(ShakeByRotation());
    }
    
    IEnumerator ShakeByPosition()
    {
        // 흔들리기 직전 회전 값
        Vector3 startPosition = transform.position;

        while (shakeTime > 0.0f)
        {
            float posX = Random.Range(-setOffsetPos.x, setOffsetPos.x);
            float posY = Random.Range(-setOffsetPos.y, setOffsetPos.y);
            float posZ = Random.Range(-setOffsetPos.z, setOffsetPos.z);

            transform.position = startPosition + new Vector3(posX, posY, posZ) * setforce * Time.deltaTime;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        transform.position = startPosition;
    }

    IEnumerator ShakeByRotation()
    {
        Vector3 startRotation = transform.eulerAngles;
        while (shakeTime > 0.0f)
        {
            float rotX = Random.Range(-setOffsetRot.x, setOffsetRot.x);
            float rotY = Random.Range(-setOffsetRot.y, setOffsetRot.y);
            float rotZ = Random.Range(-setOffsetRot.z, setOffsetRot.z);

            Vector3 randomRot = startRotation + new Vector3(rotX, rotY, rotZ);
            Quaternion rot = Quaternion.Euler(randomRot);

            while (Quaternion.Angle(transform.rotation, rot) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, setforce * Time.deltaTime);
                yield return null;
            }

            shakeTime -= Time.deltaTime;

            yield return null;
        }
        
        transform.rotation = Quaternion.Euler(startRotation);
    }
}
