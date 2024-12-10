using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "_UnitDataLIST", menuName = "DATA/UnitDataLIST", order = 1)] 
public class ListUnitDataSO : ScriptableObject{
    [SerializeField] private List<UnitDataSO> unitesData;
    public UnitDataSO GetUnitFromID(UnitID id) {
        for (int i = 0; i < unitesData.Count; i++) {
            if (unitesData[i].ID == id) {
                return unitesData[i];
            }
        }

        return null;
    }

    public UnitDataSO GetRandomUnit() {
        return unitesData[Random.Range(0, unitesData.Count)];
    }
}