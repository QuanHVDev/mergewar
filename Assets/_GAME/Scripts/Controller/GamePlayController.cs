using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GameState{
    Loading,
    Home,
    SetUp,
    UserTurn,
    StartWave,
    WinWave,
    FailWave,
    EndLevel,
    Pause
}

public class GamePlayController : SingletonBehaviour<GamePlayController>{

    public Action OnStartWave, OnFailWave, OnEndLevel;
    public Action<Vector3> OnWinWave;
    private Action onShowUserTurnUI;
    private Action onUpdateBlueCoin;

    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private MapCombatController mapCombatController;
    [SerializeField] private ListUnitDataSO unitDataSo;
    [SerializeField] private ListEnemyDataSO listEnemyDataSo;

    [SerializeField] private float speedChampDefault = 1.75f;
    private const int widthSize = 5;
    private const int heightSize = 4;
    private GameState state = GameState.Loading;
    public GameState State => state;
    private GridXZ<GridObject> gridUser;
    private GridXZ<GridObject> gridEnemy;

    private const uint MAX_BLUECOIN_START = 10;
    private uint amountBlueCoin = MAX_BLUECOIN_START;
    private uint priceConvene = 3;
    public uint AmountBlueCoin => amountBlueCoin;
    public float SpeedChampDefault => speedChampDefault;
    
    void Start() {
        InitGrid();
        InitUI();
        ChangeState(GameState.Home);
    }

    public Action OnShowHome;
    private void ChangeState(GameState state) {
        if (this.State == state) return;

        this.state = state;
        switch (state) {
            case GameState.Home:
                OnShowHome?.Invoke();
                ShowHome();
                break;
            case GameState.SetUp:
                UIRoot.Ins.Get<MainUI>().Show();
                onUpdateBlueCoin?.Invoke();
                StartCoroutine(InitEnemyAsync());
                break;
            case GameState.UserTurn:
                onShowUserTurnUI?.Invoke();
                for (int x = 0; x < gridUser.GetWidth(); x++) {
                    for (int z = 0; z < gridUser.GetHeight(); z++) {
                        if (gridUser.IsValidGridPosition(new Vector2Int(x, z)) && gridUser.GetGridObject(x, z).HasUnitBase) {
                            gridUser.GetGridObject(x, z).SetupWave();
                        }
                    }
                }
                break;
            case GameState.StartWave:
                amountUnitPlayGame = amountUnit;
                OnStartWave?.Invoke();
                break;
            case GameState.WinWave:
                SFXController.Instance.PlaySndWin();
                Invoke(nameof(DoRunWinWave), 0.5f);
                break;
            case GameState.FailWave:
                SFXController.Instance.PLaySndLose();
                DoRunFailWave();
                break;
            case GameState.EndLevel:
                DoRunEndLevel();
                break;
            case GameState.Pause:
                break;
            default:
                Logs.LogError($"missing change state with {state}");
                return;
        }
    }
    
    private void DoRunEndLevel() {
        OnEndLevel?.Invoke();
        amountUnit = 0;
        amountBlueCoin = MAX_BLUECOIN_START;
        ChangeState(GameState.Home);
    }

    private void ShowHome() {
        UIRoot.Ins.Get<HomeUI>().Show();
    }

    private void DoRunFailWave() {
        OnFailWave?.Invoke();
        UIRoot.Ins.Get<FailWaveUI>().Show();
    }

    private void DoRunWinWave() {
        OnWinWave?.Invoke(mapCombatController.NextWavePos);
        Invoke(nameof(StartNewWave), 3.5f);
    }

    private uint bonusBlueCoin = 6;
    private void StartNewWave() {
        amountBlueCoin += bonusBlueCoin;
        ChangeState(GameState.SetUp);
    }

    private int amountEnemy = 0;
    private int amountUnit = 0;
    private int amountUnitPlayGame = 0;
    private IEnumerator InitEnemyAsync() {
        amountEnemy = Random.Range(1, 2);
        Logs.Log($"<color=yellow>spawn {amountEnemy} enemy</color>");
        List<Vector3> randomPos = new List<Vector3>();

        for (int i = 0; i < amountEnemy; i++) {
            Vector3Int pos = Vector3Int.one * -1;
            GridObject gridObject = null;
            do {
                pos = new Vector3Int(Random.Range(0, widthSize), 0, Random.Range(0, heightSize));
                gridObject = gridEnemy.GetGridObject(pos.x, pos.z);
            } while (randomPos.Contains(pos) || gridObject == null);

            randomPos.Add(pos);
            var data = listEnemyDataSo.GetRandomUnit();
            var enemy = data.EnemyBaseController.Spawn(gridObject.TransInWorld);
            enemy.transform.localPosition = Vector3.zero;
            enemy.Init(data);
            enemy.SetLayerMask(playerMask);
            enemy.OnDead += Enemy_OnDead;
            enemy.transform.Rotate(0, 180, 0);
            gridObject.SetBaseController(enemy);
        }
        
        yield return null;
        ChangeState(GameState.UserTurn);
    }

    private void Enemy_OnDead() {
        amountEnemy--;
        if (amountEnemy <= 0) {
            ChangeState(GameState.WinWave);
        }
    }

    private void InitGrid() {
        gridUser = new GridXZ<GridObject>(widthSize, heightSize, cellSize, mapCombatController.UserGridPosOrigin, (g, x, z) => new GridObject(g, x, z));
        gridEnemy = new GridXZ<GridObject>(widthSize, heightSize, cellSize, mapCombatController.EnemyGridPosOrigin, (g, x, z) => new GridObject(g, x, z));
        mapCombatController.InitGrid(cellSize, widthSize, heightSize, out Transform[,] unitPosTrans, out Transform[,] enemyPosTrans);
        SetTransformInWorldForGrid(unitPosTrans, enemyPosTrans);
    }


    private void SetTransformInWorldForGrid(Transform[,] unitPosTrans, Transform[,] enemyPosTrans) {
        GridObject gridObject;
        for (int x = 0; x < widthSize; x++) {
            for (int z = 0; z < heightSize; z++) {
                gridObject = gridUser.GetGridObject(x, z);
                if(gridObject != null) gridObject.SetWorldTrans(unitPosTrans[x, z]);
                
                gridObject = gridEnemy.GetGridObject(x, z);
                if(gridObject != null) gridObject.SetWorldTrans(enemyPosTrans[x, z]);
            }
        }
    }


    private void InitUI() {
        MainUI mainUI = UIRoot.Ins.Get<MainUI>();
        mainUI.SetTxtPriceConvene(priceConvene);
        mainUI.OnConvene += MainUI_OnConvene;
        mainUI.OnStart += MainUI_OnStart;
        mainUI.OnDebugBlueCoin += MainUI_OnDebugBlueCoin;
        onShowUserTurnUI += () => mainUI.EnableUserTurn(true);
        onUpdateBlueCoin += () => mainUI.UpdateTxtAmountBlueCoin();
        onUpdateBlueCoin?.Invoke();

        FailWaveUI failWaveUI = UIRoot.Ins.Get<FailWaveUI>();
        failWaveUI.OnBackHome += FailWaveUI_OnBackHome;

        HomeUI homeUI = UIRoot.Ins.Get<HomeUI>();
        homeUI.OnPlay += () => { ChangeState(GameState.SetUp); };
    }

    private void FailWaveUI_OnBackHome() {
        DefeatUI defeatUI = UIRoot.Ins.Get<DefeatUI>();
        defeatUI.OnHidedUI += DefeatUI_OnHidedUI;
        defeatUI.Show();
    }

    private void DefeatUI_OnHidedUI() {
        ChangeState(GameState.EndLevel);
    }

    private void MainUI_OnDebugBlueCoin() {
        amountBlueCoin += 10;
    }
    
    private void MainUI_OnStart() {
        ChangeState(GameState.StartWave);
    }

    private bool MainUI_OnConvene() {
        if (GetRandomUnitPos(out List<GridObject> unitPosAvailable) && amountBlueCoin >= priceConvene) {
            if (unitPosAvailable.Count > 0) {
                ConveneUnit(unitPosAvailable[Random.Range(0, unitPosAvailable.Count)]);
                amountBlueCoin -= priceConvene;
                amountUnit++;
                return true;
            }
        }

        return false;
    }

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask enemyMask;
    private Vector3 offSetGrid = new Vector3(0, 0.1f, 0);
    private void ConveneUnit(GridObject gridObject) {
        var data = unitDataSo.GetRandomUnit();
        var unit = data.UnitController.Spawn(gridObject.TransInWorld);
        unit.transform.localPosition = Vector3.zero;
        unit.Init(data, (uint)Random.Range(1, 6));
        unit.SetLayerMask(enemyMask);
        unit.OnDead += Unit_OnDead;
        gridObject.SetBaseController(unit);
    }

    private void Unit_OnDead() {
        amountUnitPlayGame--;
        if (amountUnitPlayGame <= 0) {
            ChangeState(GameState.FailWave);
        }
    }

    private bool GetRandomUnitPos(out List<GridObject> unitPosAvailable) {
        GridObject gridObject;
        unitPosAvailable = new List<GridObject>();
        for (int x = 0; x < gridUser.GetWidth(); x++) {
            for (int z = 0; z < gridUser.GetHeight(); z++) {
                gridObject = gridUser.GetGridObject(x, z);
                if (gridObject != null && gridObject.IsAvailable) {
                    unitPosAvailable.Add(gridObject);
                }
            }
        }

        return unitPosAvailable.Count > 0;
    }
    
    void Update() {
        if (State == GameState.UserTurn) { PlayingGame(); }
    }


    private Coroutine mergeCoroutine;
    private void PlayingGame() {
        isDragging = mergeCoroutine != null;
        if (Input.touchCount != 1 ) return;
        if (Input.touches[0].phase != TouchPhase.Began || mergeCoroutine != null) return;

        var ray = mainCamera.ScreenPointToRay(Input.touches[0].position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 hitPoint = hit.point;
            GridObject obj = gridUser.GetGridObject(hitPoint);
            if (obj != null && obj.HasUnitBase) {
                Logs.Log("Touched Ground at: " + hitPoint);
                mergeCoroutine = StartCoroutine(MergeAsync(obj));
            }
            else {
                obj = gridEnemy.GetGridObject(hitPoint);
                if (obj != null) {
                    obj.Select();
                }
            }
        }
    }

    private IEnumerator MergeAsync(GridObject gridObject) {
        gridObject.Select();
        Ray ray = mainCamera.ScreenPointToRay(Input.touches[0].position);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);
        
        yield return new WaitWhile(() => {
            if (Input.touchCount == 1) {
                ray = mainCamera.ScreenPointToRay(Input.touches[0].position);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)) gridObject.UserDrag(hit.point);
            }
            
            return Input.touchCount == 1;
        });
        
        GridObject targetMergeObject = gridUser.GetGridObject(hit.point);
        if (targetMergeObject == null
            || !targetMergeObject.HasUnitBase 
            || targetMergeObject.UnitID != gridObject.UnitID 
            || targetMergeObject.UnitStar != gridObject.UnitStar
            || gridObject == targetMergeObject
            || targetMergeObject.UnitStar == UnitStar.ThreeStar) 
        {
            MergeFail();
            gridObject.SetUnitDefaultPos();
            yield break;
        }

        OnMerge?.Invoke(gridObject as IGetHit);
        targetMergeObject.Merge(gridObject);
        Logs.Log("Merge successful!");
        amountUnit--;
        mergeCoroutine = null;
    }

    private bool isDragging;
    private void MergeFail() {
        Logs.Log("Merge fail!");
        mergeCoroutine = null;
    }

    [SerializeField] private ArrowWeapon arrowBullet;
    public void SpawnBullet(Vector3 startPos, IGetHit target, int attack) {
        var arrow = arrowBullet.Spawn(startPos);
        arrow.Init(startPos, target.GetPosition(), 2, 0.35f, () => {
            target.GetHit(attack); 
        });
    }
    
    [SerializeField] private ArrowWeapon magicBullet;
    public void SpawnMagic(Vector3 startPos, IGetHit target, int attack) {
        var magic = magicBullet.Spawn(startPos);
        magic.Init(startPos, target.GetPosition(), 0.5f, 0.75f, () => {
            target.GetHit(attack);
        });
    }

    public Action<IGetHit> OnMerge;

    [SerializeField] private HealthBarCanvas healthBarCanvasPf;
    public HealthBarCanvas SpawnHealthBar(HealthBarPos healthBarPos) {
        return Instantiate(healthBarCanvasPf, healthBarPos.transform);
    }

}
