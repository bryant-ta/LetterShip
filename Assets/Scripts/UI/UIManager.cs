using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Image shopRefreshTimer;

    void Start() {
        Shop.Instance.refreshTimer.TickEvent += UpdateShopRefreshTimer;
    }

    public void UpdateCoinText(int newVal) {
        coinText.text = newVal.ToString();
    }

    void UpdateShopRefreshTimer(float percent) {
        shopRefreshTimer.fillAmount = percent;
    }
}
