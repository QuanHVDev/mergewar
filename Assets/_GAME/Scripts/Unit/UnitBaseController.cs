using System;
using UnityEngine;
using UnityEngine.AI;

public enum UnitStar{NoneStar, OneStar, TwoStar, ThreeStar}

public class UnitBaseController : MonoBehaviour, IUnitController, IGetHit, IPoolable{

    public Action OnDead;
    
    public void OnSpawnCallback() {
        GamePlayController.Instance.OnStartWave += GamePlayController_OnStart;
        GamePlayController.Instance.OnWinWave += GamePlayController_OnWinWave;
        OnDead = null;

    }

    public void OnRecycleCallback() {
        GamePlayController.Instance.OnStartWave -= GamePlayController_OnStart;
        GamePlayController.Instance.OnWinWave -= GamePlayController_OnWinWave;
    }
    
    private UnitData data; 
    protected UnitStatus status; 
    private UnitState state;
    private UnitAnimationsController unitAnimations;
    private Collider collider;
    private uint level;
    protected float lastTimeAttack;
    private UnitStar unitStar;
    
    protected virtual UnitID GetUnitID() => UnitID.StringEmpty;
    public UnitID UnitID => GetUnitID();
    public uint Level => level;
    public UnitStar UnitStar => unitStar;
    
    private UnitEvent unitEvent;
    [SerializeField] private HealthBarPos healthBarPos;
    private HealthBarCanvas healthBarCanvas;
    
    private void Start() {
        unitAnimations = GetComponent<UnitAnimationsController>();
        
        unitEvent = GetComponentInChildren<UnitEvent>();
        if (unitEvent) {
            unitEvent.OnAttack += UnitEvent_OnAttack;
            unitEvent.OnEndAttack += UnitEvent_OnEndAttack;
            unitEvent.OnEndDead += UnitEvent_OnEndDead;
        }
    }

    private void UnitEvent_OnEndDead() {
        gameObject.SetActive(false);
    }


    protected virtual void UnitEvent_OnAttack() {
        if (targetGetHit == null) return;
        
        targetGetHit.GetHit(status.currentAttack);
        lastTimeAttack = Time.time;

        targetGetHit = null;
    }

    private void UnitEvent_OnEndAttack() {
        if(GamePlayController.Instance.State == GameState.StartWave) ChangeState(UnitState.Idle);
    }
    
    public void Init(UnitDataSO unitDataSo, uint level) {
        collider = GetComponent<Collider>();
        collider.enabled = true;
        

        data = new UnitData();
        data.maxHP = unitDataSo.Health;
        data.maxARange = unitDataSo.ARange;
        data.maxAttack = unitDataSo.Attack;
        data.maxAttackSpeed = unitDataSo.ASpeed;
        data.maxDetectionRadius = unitDataSo.DetectionRadius;
        data.maxHp_Bonus = unitDataSo.HpBonus;
        data.maxAttack_Bonus = unitDataSo.AttackBonus;
        
        if (healthBarCanvas == null && healthBarPos != null) {
            healthBarCanvas = GamePlayController.Instance.SpawnHealthBar(healthBarPos);
        }

        lastTimeAttack = Time.time;
        this.level = level;
        unitStar = UnitStar.NoneStar;
        
        SetStatus();
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetUnitDefaultPos();
    }
    
    public void SetUnitDefaultStatus() {
        SetStatus();
    }

    private void SetStatus() {
        status = new UnitStatus();
        status.currentHealth = data.maxHP + (int)level * data.maxHp_Bonus;
        status.currentAttack = data.maxAttack + (int)level * data.maxAttack_Bonus;
        status.currentAttackSpeed = data.maxAttackSpeed;
        status.currentDetectionRadius = data.maxDetectionRadius;
        status.currentARange = data.maxARange;

        if (healthBarCanvas) {
            healthBarCanvas.UpdateHealthBar(1);
            healthBarCanvas.SetLevel(this.level);
        }
    }

    public bool Merge(uint level) {
        if (unitStar == UnitStar.ThreeStar) return false;
        
        this.level += level;
        unitStar = UpgradeStar(unitStar);
        switch (unitStar) {
            case UnitStar.OneStar:
                this.level += 10;
                break;
            case UnitStar.TwoStar:
                this.level += 5;
                break;
            case UnitStar.ThreeStar:
                break;
            default:
                Logs.LogWarning($"missing bonus with {UnitStar}");
                break;
                
        }

        healthBarCanvas.SetLevel(this.level);
        SetStatus();
        return true;
    }

    private UnitStar UpgradeStar(UnitStar currentStar) {
        switch (currentStar) {
            case UnitStar.NoneStar:
                return UnitStar.OneStar;
            case UnitStar.OneStar:
                return UnitStar.TwoStar;
            case UnitStar.TwoStar:
                return UnitStar.ThreeStar;
        }
        
        Logs.LogError("over star!");
        return UnitStar.NoneStar;
    }
    
    private void FixedUpdate() {
        if (GamePlayController.Instance.State != GameState.StartWave) return;
        
        switch (state) {
            case UnitState.Idle:
            case UnitState.Run:
                ScanForEnemies();
                if (navMeshAgent.remainingDistance <= status.currentARange && lastTimeAttack + status.currentAttackSpeed <= Time.time) {
                    ChangeState(UnitState.Attack);
                }
                break;
            case UnitState.Attack:
            case UnitState.Wait:
            case UnitState.Dead: 
                return;
            default:
                Logs.LogError($"missing UnitState {state}");
                return;
        }

    }

    [Header("Scanner")]
    private LayerMask targetLayer;
    protected IGetHit targetGetHit;
    private NavMeshAgent navMeshAgent;

    public void SetLayerMask(LayerMask targetLayer) {
        this.targetLayer = targetLayer;
    }
    
    void ScanForEnemies() {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, status.currentDetectionRadius, targetLayer);
        if (enemiesInRange.Length == 0) return;
        
        float distance = float.MaxValue;
        Transform targetCol = null;
        foreach (var col in enemiesInRange) {
            if (col.TryGetComponent(out IGetHit target)) {
                if(distance < Vector3.Distance(transform.position, col.transform.position)) continue;

                distance = Vector3.Distance(transform.position, col.transform.position);
                targetGetHit = target;
                targetCol = col.transform;
                if (Vector3.Distance(col.transform.position, transform.position) > status.currentARange) {
                    ChangeState(UnitState.Run);
                    MoveToPosition(col.transform.position);
                }
            }
        }

        if (targetCol) {
            transform.LookAt(targetCol.transform);
            Debug.DrawRay(transform.position, targetCol.transform.position - transform.position, Color.red);
        }
    }
    
    void MoveToPosition(Vector3 position) {
        if (gameObject.activeSelf && navMeshAgent != null) navMeshAgent.SetDestination(position);
    }

    private void ChangeState(UnitState state) {
        if (this.state == state) return;
        this.state = state;
        unitAnimations.RunAnim(false);

        //play animation
        switch (state) {
            case UnitState.Wait:
                break;
            case UnitState.Idle:
                break;
            case UnitState.Run:
                unitAnimations.RunAnim(true);
                break;
            case UnitState.Dead:
                unitAnimations.DeadAnim();
                DoDead();
                break;
            case UnitState.Attack:
                unitAnimations.AttackAnim();
                break;
            default:
                Logs.LogError($"missing UnitState {state}");
                return;
        }
    }
    

    public void DoIdle() {
    }

    public void DoRun() {
    }

    public void DoAttack() {
    }

    public void DoDead() {
        collider.enabled = false;
        OnDead?.Invoke();
        Logs.Log($"<color=red>Unit {gameObject.name} dead!</color>");
    }

    [SerializeField] private Renderer renderer;
    public Renderer GetRender() {
        return renderer;
    }

    public void GetHit(float damage) {
        if (state == UnitState.Dead) return;

        status.currentHealth -= (int)damage;
        if (status.currentHealth <= 0) {
            status.currentHealth = 0;
            ChangeState(UnitState.Dead);
        }
        
        healthBarCanvas.UpdateHealthBar(status.currentHealth * 1.0f / data.maxHP);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, status != null ? status.currentDetectionRadius : 0);
    }

    private void GamePlayController_OnWinWave(Vector3 pos) {
        MoveToPosition(pos);
        ChangeState(UnitState.Run);
        navMeshAgent.stoppingDistance = 0.5f;
        navMeshAgent.speed = 2 * GamePlayController.Instance.SpeedChampDefault;
    }

    private void GamePlayController_OnStart() {
        navMeshAgent.enabled = true;
        ChangeState(UnitState.Idle);
    }

    

    public void SetUnitDefaultPos() {
        gameObject.SetActive(true);
        navMeshAgent.enabled = false;
        navMeshAgent.stoppingDistance = status.currentARange;
        navMeshAgent.speed = GamePlayController.Instance.SpeedChampDefault;
        if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled) navMeshAgent.isStopped = true;
        
        transform.localPosition = new Vector3(0, 0.1f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        ChangeState(UnitState.Wait);
    }
    
}

public enum UnitState {Wait, Idle, Run, Dead, Attack}

[Serializable]
public class UnitData{
    public int maxHP;
    public int maxAttack;
    public float maxAttackSpeed;
    public float maxDetectionRadius;
    public float maxARange;

    public int maxHp_Bonus;
    public int maxAttack_Bonus;
}

[Serializable]
public class UnitStatus{
    public int currentHealth;
    public int currentAttack;
    public float currentAttackSpeed;
    public float currentDetectionRadius;
    public float currentARange;
}