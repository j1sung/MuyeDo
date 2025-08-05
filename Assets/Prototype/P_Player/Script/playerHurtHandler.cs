using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHurtHandler : MonoBehaviour
{
    private float hurtDuration;
    private bool isHurt = false;

    public bool IsHurt => isHurt;

    /*public playerHurtHandler(MonoBehaviour host, float duration)
    {
        coroutineHost = host;
        hurtDuration = duration;
    }*/
    public void Init(float duration)
    {

        this.hurtDuration = duration;}

    public void TakeHit(System.Action onEnd = null)
    {
        Debug.Log("TakeHit �����");
        if (!isHurt)
        {
            StartCoroutine(HurtRoutine(onEnd));
        }
    }

    private IEnumerator HurtRoutine(System.Action onEnd = null)
    {

        Debug.Log("HurtRoutine �����");
        isHurt = true;

        yield return new WaitForSeconds(hurtDuration); // �ǰ� �� ��� ���߰� �˹� or ����ȭ

        isHurt = false;
    }
}
