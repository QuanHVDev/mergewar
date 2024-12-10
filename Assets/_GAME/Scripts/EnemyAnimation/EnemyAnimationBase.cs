using UnityEngine;

public class EnemyAnimationBase : MonoBehaviour{
    public virtual void Play_Idle() { }
    public virtual void Play_Run() { }
    public virtual void Play_Attack() { }
    public virtual void Play_Dead() { }
}