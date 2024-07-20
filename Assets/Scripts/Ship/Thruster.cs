using System;
using UnityEngine;

public class Thruster : Bit {
    public int Pow;

    Rigidbody2D rb;

    bool isThrusting;
    void FixedUpdate() {
        if (isThrusting) {
            ApplyThrust(transform.forward, transform.localPosition, Pow);
        }
    }

    public override void Activate() { isThrusting = true; }

    public override void Deactivate() { isThrusting = false; }

    void ApplyThrust(Vector2 thrustDirection, Vector2 thrustPosition, float thrustAmount) {
        // Apply force at a specific point to generate thrust
        rb.AddForceAtPosition(thrustDirection * thrustAmount, thrustPosition, ForceMode2D.Force);

        // Optionally, apply torque for rotational effects
        // Vector3 offsetFromCenterOfMass = thrustPosition - rb.transform.position;
        // Vector3 torque = Vector3.Cross(offsetFromCenterOfMass, thrustDirection * thrustAmount);
        // rb.AddTorque(torque, ForceMode2D.Force);
    }

    public override bool Attach(Bit root, Collider2D rootSlot) {
        bool ret = base.Attach(root, rootSlot);
        if (ret == false) return ret;

        rb = GetComponentInParent<Rigidbody2D>();

        return ret;
    }
}