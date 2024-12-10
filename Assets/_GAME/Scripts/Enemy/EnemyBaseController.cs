using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseController : MonoBehaviour, IUnitController, IGetHit, IPoolable{
    public Action OnDead;
    public Action<float, bool> OnChangeHealth;
    
    private UnitData data;
    private UnitStatus status;
    private UnitState state;
    private NavMeshAgent navMeshAgent;
    private Collider collider;
    private EnemyAnimationBase enemyAnimationBase;
    private UnitEvent enemyEvent;

    [SerializeField] private HealthBarPos healthBarPos;
    private HealthBarCanvas healthBarCanvas;
    
    private void Start() {
        enemyEvent = GetComponentInChildren<UnitEvent>();
        if (enemyEvent) {
            enemyEvent.OnAttack += DoAttack;
            enemyEvent.OnEndAttack += EnemyEvent_OnEndAttack;
            enemyEvent.OnEndDead += EnemyEvent_OnEndDead;
        }
    }

    private void EnemyEvent_OnEndDead() {
        gameObject.SetActive(false);
    }

    private void EnemyEvent_OnEndAttack() {
        ChangeState(UnitState.Idle);
    }
    
    public void Init(EnemyDataSO enemyDataSo) {
        enemyAnimationBase = GetComponent<EnemyAnimationBase>();

        collider = GetComponent<Collider>();
        collider.enabled = true;
        
        data = new UnitData();
        data.maxHP = enemyDataSo.Health;
        data.maxAttack = enemyDataSo.Attack;
        data.maxAttackSpeed = enemyDataSo.ASpeed;
        data.maxDetectionRadius = enemyDataSo.DetectionRadius;
        data.maxARange = enemyDataSo.ARange;

        status = new UnitStatus();
        status.currentHealth = data.maxHP;
        status.currentAttack = data.maxAttack;
        status.currentAttackSpeed = data.maxAttackSpeed;
        status.currentDetectionRadius = data.maxDetectionRadius;
        status.currentARange = data.maxARange;

        navMeshAgent = GetComponent<NavMeshAgent>();
        SetupDefaultNavMesh();

        if (healthBarCanvas == null && healthBarPos != null) {
            healthBarCanvas = GamePlayController.Instance.SpawnHealthBar(healthBarPos);
        }

        if (healthBarCanvas != null) {
            healthBarCanvas.SetLevel(1);
            healthBarCanvas.UpdateHealthBar(status.currentHealth * 1.0f / data.maxHP, true);
        }
        
        ChangeState(UnitState.Wait);
    }

    private void SetupDefaultNavMesh() {
        navMeshAgent.enabled = false;
        navMeshAgent.stoppingDistance = status.currentARange;
        navMeshAgent.speed = GamePlayController.Instance.SpeedChampDefault;
    }

    private void ChangeState(UnitState state) {
        if (this.state == state) return;

        this.state = state;
        //play animation
        switch (state) {
            case UnitState.Wait:
            case UnitState.Idle:
                enemyAnimationBase.Play_Idle();
                break;
            case UnitState.Run:
                enemyAnimationBase.Play_Run();
                break;
            case UnitState.Dead:
                enemyAnimationBase.Play_Dead();
                DoDead();
                break;
            case UnitState.Attack:
                enemyAnimationBase.Play_Attack();
                break;
            default:
                Logs.LogError($"{gameObject.name} missing ChangeState {state}");
                return;
        }
    }
    
    public void DoIdle() {
    }

    public void DoRun() {
    }

    public void DoAttack() {
        if (targetGetHit != null) {
            targetGetHit.GetHit(status.currentAttack);
        }
        targetGetHit = null;
        lastTimeAttack = Time.time;
    }

    public void DoDead() {
        OnDead?.Invoke();
        collider.enabled = false;
        Logs.Log($"<color=red>Enemy {gameObject.name} dead!</color>");
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

    [SerializeField] private Transform rootTrans;
    public virtual Vector3 GetPosition() {
        return rootTrans.position;
    }

    private LayerMask targetLayer;
    public void SetLayerMask(LayerMask targetLayer) {
        this.targetLayer = targetLayer;
    }

    private void FixedUpdate() {
        switch (state) {
            case UnitState.Idle:
            case UnitState.Run:
                ScanForEnemies();
                if (navMeshAgent.remainingDistance <= status.currentARange && lastTimeAttack + status.currentAttackSpeed <= Time.time) {
                    ChangeState(UnitState.Attack);
                }
                break;
        }
    }

    private IGetHit targetGetHit;
    private float lastTimeAttack;
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
        if (navMeshAgent != null) navMeshAgent.SetDestination(position);
    }

    public void OnSpawnCallback() {
        GamePlayController.Instance.OnStartWave += GamePlayController_OnStart;
        GamePlayController.Instance.OnFailWave += GamePlayController_OnFailWave;
        GamePlayController.Instance.OnEndLevel += EnemyEvent_OnEndDead;
    }
    
    private void GamePlayController_OnStart() {
        navMeshAgent.enabled = true;
        ChangeState(UnitState.Idle);
    }

    private void GamePlayController_OnFailWave() {
        ChangeState(UnitState.Wait);
    }

    public void OnRecycleCallback() {
        GamePlayController.Instance.OnFailWave -= GamePlayController_OnFailWave;
        GamePlayController.Instance.OnStartWave -= GamePlayController_OnStart;        
        GamePlayController.Instance.OnEndLevel -= EnemyEvent_OnEndDead;
        OnChangeHealth = null;
        OnDead = null;
    }
}
