using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DATA/EnemyData", order = 4)] 
public class EnemyDataSO : ChampDataSO{
    [Header("Enemy")]
    [SerializeField] protected EnemyID id;
    [SerializeField] private EnemyBaseController enemyBaseController;
    
    public EnemyID ID => id;
    public EnemyBaseController EnemyBaseController => enemyBaseController;
}