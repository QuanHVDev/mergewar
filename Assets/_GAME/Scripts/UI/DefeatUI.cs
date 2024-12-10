using System;
using UnityEngine;
using UnityEngine.UI;

public class DefeatUI : BaseUIElement{
    public Action OnHidedUI;
    public override void OnAwake() {
        tabButton.onClick.AddListener(Hide);
    }

    public override void Hide() {
        TransitionUI.Instance.ShowUI(() => {
            OnHidedUI?.Invoke();
            base.Hide();
        });
    }

    [SerializeField] private Button tabButton;
    

}