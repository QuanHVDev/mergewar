using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCanvas : MonoBehaviour, IPoolable{

    [SerializeField] private Image healthBarImage;
    [SerializeField] private TMP_Text levelTxt;
    private Coroutine healthBarCoroutine;

    public void OnSpawnCallback() {
        healthBarCoroutine = null;
    }

    public void OnRecycleCallback() {
        StopAllCoroutines();
        CancelInvoke();
    }

    public void UpdateHealthBar(float percent, bool isForce = false) {
        if (isForce) {
            healthBarImage.fillAmount = percent;
        }
        else {
            if (healthBarCoroutine != null) {
                StopCoroutine(healthBarCoroutine);
            }
            
            healthBarCoroutine = StartCoroutine(UpdateHealthBarAsync(percent));
        }
    }
    
    private IEnumerator UpdateHealthBarAsync(float targetPercent) {
        float initialPercent = healthBarImage.fillAmount;
        float duration = 0.5f; 
        float elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            healthBarImage.fillAmount = Mathf.Lerp(initialPercent, targetPercent, elapsed / duration);
            yield return null; 
        }

        healthBarImage.fillAmount = targetPercent;
    }


    public void SetLevel(uint level) {
        levelTxt.SetText(level.ToString());
    }
}
