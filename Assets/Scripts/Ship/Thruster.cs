using System;
using UnityEngine;

public class Thruster : Bit {
    public int Pow;

    Rigidbody2D rb;
    
    float maxAngularVelocity = 10;

    bool isThrusting;
    void FixedUpdate() {
        if (isThrusting) {
            if (rb == null) {
                rb = GetComponentInParent<Rigidbody2D>();
            }
            
            ApplyThrust(transform.localPosition, Pow);

            if (rb.angularVelocity > maxAngularVelocity) {
                rb.angularVelocity = maxAngularVelocity;
            } else if (rb.angularVelocity < -maxAngularVelocity) {
                rb.angularVelocity = -maxAngularVelocity;
            }
        }
    }

    public override void Activate() { isThrusting = true; }

    public override void Deactivate() { isThrusting = false; }

    void ApplyThrust(Vector2 thrustPosition, float thrustAmount) {
        // Apply force at a specific point to generate thrust

        Vector2 thrustDir;
        switch (Id) {
            case 0: // U
                thrustDir = transform.up;
                break;
            case 1: // R
                thrustDir = transform.right;
                break;
            case 2: // D
                thrustDir = -transform.up;
                break;
            case 3: // L
                thrustDir = -transform.right;
                break;
            default:
                thrustDir = Vector2.zero;
                break;
        }
        thrustPosition = transform.TransformPoint(thrustPosition);
        
        rb.AddForceAtPosition(thrustDir * thrustAmount, thrustPosition, ForceMode2D.Force);
    }

    // Generally cant use Attach in Bit types due to ship editor not being able to recreate
    
    // public override bool Attach(Bit root, Collider2D rootSlot) {
    //     bool ret = base.Attach(root, rootSlot);
    //     if (ret == false) return ret;
    //
    //     rb = GetComponentInParent<Rigidbody2D>();
    //
    //     return ret;
    // }

    public override void Dettach() {
        base.Dettach();
        rb = null;
    }
}