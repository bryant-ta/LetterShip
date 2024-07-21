using System;
using UnityEngine;

public class Coin : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player") && col.TryGetComponent(out Bit b) && b.BodyCol == col) {
            GameManager.Instance.ModifyCoins(1);
            World.Instance.NumCurCoin--;
            Destroy(gameObject);
        }
    }
}
