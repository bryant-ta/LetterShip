using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public int Coins;
    public int InitialCoins;
    
    void Start() { Setup(); }

    void Setup() {
        Ship ship = Factory.Instance.CreateShip("base_player", Vector3.zero);
        Ref.Player.SetShip(ship);
        Ref.Player.Ship.transform.position = new Vector3(0, -5, 0);

        ModifyCoins(InitialCoins);
    }

    public bool ModifyCoins(int delta) {
        int newVal = Coins + delta;
        if (newVal < 0) {
            return false;
        }

        Coins = newVal;
        UIManager.Instance.UpdateCoinText(Coins);
        return true;
    }

    public void Lose() {
        Ref.Player.PlayerInput.StopInput = true;
        UIManager.Instance.ToggleMainMenuButton(true);
        UIManager.Instance.ShowDiedText();
    }
}