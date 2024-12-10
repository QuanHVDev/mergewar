using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "DATA/UnitData", order = 2)] 
[Serializable]
public class UnitDataSO : ChampDataSO{
    [Header("Unit")]
    [SerializeField] protected UnitID id;
    [SerializeField] protected UnitBaseController unitController;
    public UnitBaseController UnitController => unitController;
    public UnitID ID => id;

}

[Serializable] public enum UnitID {StringEmpty, u1 , u2, u3, u4}
[Serializable] public enum EnemyID {StringEmpty, e1, e2, e3, e4} 
