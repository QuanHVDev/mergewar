using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim_01 : EnemyAnimationBase{
    [SerializeField] private Animator animator;
    private enum Enemy_01_Anim{
        Run,
        Victory,
        Dead,
        N_IdleNormal,
        N_Attack01,
        N_Attack02,
        B_Attack,
        B_SenseSomethingRPT,
        B_Taunting
    }

    public override void Play_Idle() {
        animator.Play(nameof(Enemy_01_Anim.N_IdleNormal));
    }

    public override void Play_Attack() {
        animator.Play(nameof(Enemy_01_Anim.N_Attack02));
    }

    public override void Play_Dead() {
        animator.Play(nameof(Enemy_01_Anim.Dead));
    }

    public override void Play_Run() {
        animator.Play(nameof(Enemy_01_Anim.Run));
    }

    [ContextMenu(nameof(B_Taunting))]
    public void B_Taunting() {
        animator.Play(nameof(Enemy_01_Anim.B_Taunting));
    }
    
    [ContextMenu(nameof(Victory))]
    public void Victory() {
        animator.Play(nameof(Enemy_01_Anim.Victory));
    }
    
}
