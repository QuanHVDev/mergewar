using UnityEngine;

public class EnemyController_01 : EnemyBaseController{
    public override Vector3 GetPosition() {
        return transform.position + new Vector3(0,1.5f,0);
    }
}