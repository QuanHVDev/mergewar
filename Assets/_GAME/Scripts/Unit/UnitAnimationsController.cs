using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAnimationsController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public void RunAnim(bool inRun) {
        anim.SetBool("Run", inRun);
    }

    public void AttackAnim()
    {
        anim.SetTrigger("Attack");
    }

    public void DeadAnim()
    {
        anim.SetTrigger("Dead");
    }
}
