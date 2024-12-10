using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController_02 : UnitBaseController
{
    protected override UnitID GetUnitID() {
        return UnitID.u2;
    }

    protected override void UnitEvent_OnAttack() {
        if(targetGetHit == null) return;
        
        GamePlayController.Instance.SpawnBullet(transform.position, targetGetHit, status.currentAttack);
        lastTimeAttack = Time.time;
        targetGetHit = null;
    }
}
