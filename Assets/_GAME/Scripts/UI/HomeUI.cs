using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUIElement{
    public Action OnPlay;
    [SerializeField] private Button btnPlay;
    public override void OnAwake() {
        btnPlay.onClick.AddListener(Hide);
    }

    public override void Show(float toAlpha = 0.75f) {
        base.Show(toAlpha);
        TransitionUI.Instance.HideUI();
    }

    public override void Hide() {
        TransitionUI.Instance.ShowUI(() => {
            OnPlay?.Invoke();
            base.Hide();
        });
    }
}