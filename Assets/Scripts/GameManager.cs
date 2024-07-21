using UnityEngine;

public class GameManager : Singleton<GameManager> {
    

    void Start() {
        Setup();
    }

    void Setup() {
        Factory.Instance.CreateFrame(Random.Range(1, 9), new Vector3(-4, -2));
        Factory.Instance.CreateFrame(Random.Range(1, 9), new Vector3(-2, -2));
        Factory.Instance.CreateWeapon(Random.Range(0,7), new Vector3(0, -2));
        Factory.Instance.CreateThruster(Random.Range(0, 4), new Vector3(2, -2));
        Factory.Instance.CreateThruster(Random.Range(0, 4), new Vector3(4, -2));
    }
}
