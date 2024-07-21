using System;
using System.Collections.Generic;
using Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shop : Singleton<Shop> {
    [SerializeField] int refreshDur;
    [SerializeField] List<Transform> shopPositions;
    [SerializeField] List<TextMeshProUGUI> priceTexts;
    List<int> prices = new();
    
    public CountdownTimer refreshTimer;
    
    void Awake() {
        refreshTimer = new CountdownTimer(refreshDur);
        refreshTimer.EndEvent += RefreshShop;
        for (int i = 0; i < shopPositions.Count; i++) {
            prices.Add(0);
        }
    }

    void Start() {
        // spawn free stuff
        for (int i = 0; i < shopPositions.Count; i++) {
            RollProduct(i, true);
        }
        
        refreshTimer.Start();
    }

    public bool Buy(Bit bit) {
        int index = IndexOfProduct(bit);
        if (index < 0) return false;
        if (GameManager.Instance.ModifyCoins(-prices[index])) {
            Transform productTrs = shopPositions[index].GetChild(1);
            productTrs.parent = null;
            productTrs.tag = "Untagged";
            productTrs.GetChild(0).tag = "Untagged";
            
            return true;
        } else {
            return false;
        }
    }

    void RefreshShop() {
        for (int i = 0; i < shopPositions.Count; i++) {
            RollProduct(i);
        }
        
        refreshTimer.Reset();
        refreshTimer.Start();
    }

    void RollProduct(int index, bool initial = false) {
        if (shopPositions[index].childCount > 1) {
            Destroy(shopPositions[index].GetChild(1).gameObject);
        }
        
        int type = Random.Range(1, 4);
        GameObject o = null;
        switch ((BitType)type) {
            case BitType.Frame:
                if (initial) {
                    // omit frame_ from spawn
                    o = Factory.Instance.CreateSalvage(Random.Range(1, 8), BitType.Frame, Vector3.zero);
                    break;
                }
                o = Factory.Instance.CreateSalvage(Random.Range(1, 9), BitType.Frame, Vector3.zero);
                break;
            case BitType.Weapon:
                o = Factory.Instance.CreateSalvage(Random.Range(0, 7), BitType.Weapon, Vector3.zero);
                break;
            case BitType.Thruster:
                o = Factory.Instance.CreateSalvage(Random.Range(0, 4), BitType.Thruster, Vector3.zero);
                break;
        }
        o.transform.parent = shopPositions[index];
        o.transform.localPosition = Vector3.zero;

        o.tag = "Shop";
        o.transform.GetChild(0).tag = "Shop";
        
        // Price
        if (!initial) {
            int price = Random.Range(1, 10);
            prices[index] = price;
            priceTexts[index].text = price.ToString();
        }
    }

    public int IndexOfProduct(Bit bit) {
        for (int i = 0; i < shopPositions.Count; i++) {
            Bit b = shopPositions[i].GetComponentInChildren<Bit>();
            if (b != null && b == bit) {
                return i;
            }
        }
        return -1;
    }
}
