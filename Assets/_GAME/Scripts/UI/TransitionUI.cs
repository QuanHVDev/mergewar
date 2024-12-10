using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionUI : SingletonBehaviourDontDestroy<TransitionUI>{
    [SerializeField] private Image bg;

    [ContextMenu(nameof(ShowUI))]
    public void ShowUI(Action onComplete = null) {
        bg.raycastTarget = true;
        bg.DOFade(1, 1.5f).From(0).OnComplete(() => {
            onComplete?.Invoke();
        });
    }

    [ContextMenu(nameof(HideUI))]
    public void HideUI(Action onComplete = null) {
        Invoke(nameof(TurnOffRaycastBG), 0.75f);
        bg.DOFade(0f, 1.5f).From(1);
    }

    private void TurnOffRaycastBG() {
        bg.raycastTarget = false;
    }
}