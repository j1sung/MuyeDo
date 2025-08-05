using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private float maxHP = 100f;
    private float currentHP;

    private float maxBattleGauge = 100f;
    private float currentBattleGauge;

    private int maxDash = 3;
    private int currentDash;

    private float dashRecoveryInterval = 5f; // 약진 게이지 자동회복 주기
    private float dashTimer = 0f; // 약진 게이지 자동회복 타이머

    private void Awake()
    {
        currentHP = maxHP;
        currentBattleGauge = 0f;
        currentDash = maxDash;
    }
    private void Start()
    {
        Invoke(nameof(UpdateHUD), 0.1f); // 0.1초 지연 → HUD 초기화 타이밍 보장
    }
    private void Update()
    {
        RecoverDashOverTime(); // 약진 게이지 자동 회복
    }
    // ===== 체력 =====
    // 체력 감소
    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Max(currentHP - damage, 0f); // 데미지가 음수가 될 수 없게끔 Mathf.Max() 사용
        Debug.Log("플레이어 OnHit 호출됨! 피해량: " + currentHP);
        UpdateHUD();

        if (currentHP <= 0)
            PlayerDeath();
    }

    private void PlayerDeath()
    {
        // 사망처리 (게임 종료 UI 발생)
    }

    // 무예 게이지 관리
    public void GainBattleGauge(float amount) 
    {   
        // combat.cs에서 공격이 발생하면 amount 만큼의 무예 게이지를 얻는다.
        currentBattleGauge = Mathf.Min(currentBattleGauge + amount, maxBattleGauge); // 무술 게이지가 maxBattleGague를 넘어갈 수 없게끔 Mathf.Min() 사용
        UpdateHUD();
    }

    // ===== 무예 게이지 =====
    // 스킬 사용
    public bool UseBattleGauge(float amount) 
    // amount 만큼의 무예 게이지를 사용하여 스킬을 발동한다.
    {
        if (currentBattleGauge >= amount)
        {
            currentBattleGauge -= amount;
            return true;
        }
        // 현재 무예 게이지가 amount보다 작으면 스킬 사용 실패
        else{
            return false;
        }
    }

    // ===== 약진 =====
    // 약진 게이지 관리
    public bool UseDash()
    {
        if (currentDash > 0)
        {
            currentDash--;
            UpdateHUD();
            return true;
        }
        // 약진 게이지가 1보다 작으면 약진 실패
        else{
            return false;
        }
    }
    
    // 약진 게이지 자동 회복
    private void RecoverDashOverTime()
    {
        if (currentDash < maxDash) // 약진 게이지가 MAX가 아니라면 dashTime(5초)만큼의 시간 뒤 회복
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashRecoveryInterval)
            {
                currentDash++;
                UpdateHUD();
                dashTimer = 0f;
            }
        }
    }

    // 반격 성공 시
    public void SuccessParrying()
    {
        GainBattleGauge(30f); // 무예 게이지 회복

        if (currentDash < maxDash) // 약진 게이지 회복복
        {
            currentDash++;
        }
        UpdateHUD();
    }
    // HUD 업데이트
    private void UpdateHUD()
    {
        float hpRatio = currentHP / maxHP;
        float maxBattleGaugeRatio = currentBattleGauge / maxBattleGauge;

        PlayerHUD.Instance.SetHP(hpRatio);
        PlayerHUD.Instance.SetBattleGauge(maxBattleGaugeRatio);
        PlayerHUD.Instance.SetDashGauge(currentDash);
    }
}