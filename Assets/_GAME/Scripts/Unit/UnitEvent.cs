using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEvent : MonoBehaviour{
    public Action OnAttack, OnEndAttack, OnEndDead;
    public void DoAttack() {
        OnAttack?.Invoke();
    }

    public void DoEndAttack() {
        OnEndAttack?.Invoke();
    }

    public void DoEndDead() {
        OnEndDead?.Invoke();
    }
}
