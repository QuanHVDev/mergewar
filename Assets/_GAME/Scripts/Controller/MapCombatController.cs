using UnityEngine;
public class MapCombatController : MonoBehaviour{
    [SerializeField] private Transform userGridPosOrigin;
    [SerializeField] private Transform enemyGridPosOrigin;
    [SerializeField] private Transform unitPosParent;
    [SerializeField] private Transform nextWaveTrans;
    [SerializeField] private GameObject pos_pf;
    [SerializeField] private GameObject pos1_pf;

    public Vector3 EnemyGridPosOrigin => enemyGridPosOrigin.position;
    public Vector3 UserGridPosOrigin => userGridPosOrigin.position;
    public Vector3 NextWavePos => nextWaveTrans.position;

    public void InitGrid(float cellSize, int widthSize, int heightSize, out Transform[,] unitPosTrans, out Transform[,] enemyPosTrans) {
        float offset = cellSize / 2;
        unitPosTrans = new Transform[widthSize, heightSize];
        enemyPosTrans = new Transform[widthSize, heightSize];
        for (int x = 0; x < widthSize; x++) {
            for (int z = 0; z < heightSize; z++) {
                var unitPos = SpawnPosUnit((x + z) % 2 == 0);
                unitPos.name = $"pos x:{x} y:{z}";
                unitPos.transform.parent = userGridPosOrigin;
                unitPos.transform.localPosition = new Vector3(x + offset, 0,  z + offset);
                unitPosTrans[x, z] = unitPos.transform;
                    
                unitPos = SpawnPosUnit((x + z) % 2 == 0);
                unitPos.name = $"pos x:{x} y:{z}";
                unitPos.transform.parent = enemyGridPosOrigin;
                unitPos.transform.localPosition = new Vector3(x + offset, 0, z+ offset);
                enemyPosTrans[x, z] = unitPos.transform;
            }
        }
    }
    
    private GameObject SpawnPosUnit(bool isUnitPos) {
        if(isUnitPos) return Instantiate(pos_pf);
        
        return Instantiate(pos1_pf);
    }
}