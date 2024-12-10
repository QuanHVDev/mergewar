using System;
using UnityEngine;
using UnityEngine.UI;

public class FailWaveUI : BaseUIElement{
    public override void OnAwake() { }
    public Action OnContinuePlay, OnBackHome;
    
    [SerializeField] private Button btnContinuePlay;
    [SerializeField] private Button btnBackHome;

    private void Start() {
        btnContinuePlay.onClick.AddListener(DoContinuePlay);
        btnBackHome.onClick.AddListener(DoBackHome);
    }

    private void DoContinuePlay() {
        OnContinuePlay?.Invoke();
        Hide();
    }
    private void DoBackHome() {
        OnBackHome?.Invoke();
        Hide();
    }
}