using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController_03 : UnitBaseController
{
    protected override UnitID GetUnitID() {
        return UnitID.u3;
    }

    protected override void UnitEvent_OnAttack() {
        if(targetGetHit == null) return;
        
        GamePlayController.Instance.SpawnMagic(transform.position, targetGetHit, status.currentAttack);
        lastTimeAttack = Time.time;
        targetGetHit = null;
    }
}
