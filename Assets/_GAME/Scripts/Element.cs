using UnityEngine;
public class GridObject {
    private GridXZ<GridObject> grid;
    private int x;
    private int z;
    private UnitBaseController unitController;
    private EnemyBaseController enemyController;
    private Transform transInWorld;

    public GridObject(GridXZ<GridObject> grid, int x, int z) {
        this.grid = grid;
        this.x = x;
        this.z = z;
        unitController = null;
        enemyController = null;
        GamePlayController.Instance.OnEndLevel += ResetData;
    }

    public void SetBaseController(UnitBaseController unitController) {
        this.unitController = unitController;
    }
    
    public void SetBaseController(EnemyBaseController enemyController) {
        this.enemyController = enemyController;
    }

    public void SetupWave() {
        unitController.SetUnitDefaultPos();
        unitController.SetUnitDefaultStatus();
    }

    public void ResetData() {
        if(unitController) unitController.gameObject.Recycle();
        if(enemyController) enemyController.gameObject.Recycle();
        
        this.unitController = null;
        this.enemyController = null;
    }

    public void SetUnitDefaultPos() {
        unitController.SetUnitDefaultPos();
    }

    public bool IsAvailable => !unitController && !enemyController;
    public bool HasUnitBase => unitController;
    public Transform TransInWorld => transInWorld;
    public UnitID UnitID => unitController.UnitID;
    public UnitStar UnitStar => unitController.UnitStar;
    public void Select() {
        Logs.Log($"selected : {x} - {z}");
    }

    public Vector3Int GetXZVector() {
        return new Vector3Int(x, 0, z);
    }

    public void SetWorldTrans(Transform unitPosTran) {
        this.transInWorld = unitPosTran;
    }

    public void UserDrag(Vector3 hitPoint) {
        unitController.transform.position = hitPoint;
    }

    public void Merge(GridObject gridObject) {
        if (unitController.Merge(gridObject.unitController.Level)) {
            gridObject.ResetData();
        }
    }
}