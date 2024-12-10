using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_EnemyDataLIST", menuName = "DATA/EnemyDataLIST", order = 3)] 
public class ListEnemyDataSO: ScriptableObject{
    [SerializeField] private List<EnemyDataSO> enemiesData;
    public EnemyDataSO GetEnemyFromID(EnemyID id) {
        for (int i = 0; i < enemiesData.Count; i++) {
            if (enemiesData[i].ID == id) {
                return enemiesData[i];
            }
        }

        return null;
    }

    public EnemyDataSO GetRandomUnit() {
        return enemiesData[Random.Range(0, enemiesData.Count)];
    }
}