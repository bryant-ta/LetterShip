using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
    [SerializeField] GameObject target;
    void Update() {
        if (target != null) {
            transform.position = target.transform.position;
            transform.rotation = target.transform.rotation;
        }
    }

    public void SetTarget(GameObject target) {
        this.target = target;
    }
}
