using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Image shopRefreshTimer;

    [SerializeField] Button mainMenuButton;

    void Start() {
        Shop.Instance.refreshTimer.TickEvent += UpdateShopRefreshTimer;
        Ref.Player.PlayerInput.InputCancel += ToggleMainMenuButtonHelper;
    }

    public void UpdateCoinText(int newVal) {
        coinText.text = newVal.ToString();
    }

    void UpdateShopRefreshTimer(float percent) {
        shopRefreshTimer.fillAmount = percent;
    }

    void ToggleMainMenuButtonHelper() { ToggleMainMenuButton(!mainMenuButton.gameObject.activeSelf);}
    public void ToggleMainMenuButton(bool enable) {
        mainMenuButton.gameObject.SetActive(enable);
    }
}
