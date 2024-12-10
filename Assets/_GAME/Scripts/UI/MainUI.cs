using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : BaseUIElement{
    public Func<bool> OnConvene;
    public Action OnStart;
    public Action OnDebugBlueCoin;

    [SerializeField] private TMP_Text txtAmountBlueCoin;
    [SerializeField] private TMP_Text txtPriceConvene;
    [SerializeField] private Button conveneBtn;
    [SerializeField] private Button startBtn;
    [SerializeField] private Button debugBtn;

    public override void OnAwake() {
        conveneBtn.onClick.AddListener(OnClick_Convene);
        startBtn.onClick.AddListener(OnClick_Start);
        debugBtn.onClick.AddListener(OnClick_DebugBlueCoin);
        EnableUserTurn(false);
    }

    private void OnClick_DebugBlueCoin() {
        OnDebugBlueCoin?.Invoke();
        UpdateTxtAmountBlueCoin();
        Logs.Log("<color=blue> Add blue coin complete!");
    }

    private void OnClick_Start() {
        OnStart?.Invoke();
        EnableUserTurn(false);
    }

    private void OnClick_Convene() {
        if (OnConvene == null) return;
        if (OnConvene.Invoke()) {
            UpdateTxtAmountBlueCoin();
        }
        else {
            ShowCannotConvene();
        }
    }

    private void ShowCannotConvene() {
        Logs.Log("<color=yellow> Not enough coin to convene! </color>");
    }

    public void UpdateTxtAmountBlueCoin() {
        txtAmountBlueCoin.SetText(GamePlayController.Instance.AmountBlueCoin.ToString());
    }

    public void SetTxtPriceConvene(uint value) {
        txtPriceConvene.SetText(value.ToString());
    }

    public void EnableUserTurn(bool enable) {
        conveneBtn.gameObject.SetActive(enable);
        debugBtn.gameObject.SetActive(enable);
        startBtn.gameObject.SetActive(enable);
    }

    public override void Show(float toAlpha = 0.75f) {
        base.Show(toAlpha);
        TransitionUI.Instance.HideUI();
    }
}