using UnityEngine;

public class Weapon : Bit {
    public int Atk;
    public int AtkSpd;
    public GameObject projectileObj;

    bool isAttacking;
    float atkInterval;
    float nextAtkTime;

    void Start() {
        atkInterval = 1f / AtkSpd;
    }

    void Update() {
        if (isAttacking && Time.time >= nextAtkTime) {
            Shoot();
            nextAtkTime = Time.time + atkInterval;
        }
    }

    void Shoot() {
        switch (Id) {
            case 0: // UDLR
                MakeProjectile(0);
                MakeProjectile(1);
                MakeProjectile(2);
                MakeProjectile(3);
                break;
            case 1: // U
                MakeProjectile(0);
                break;
            case 2: // R
                MakeProjectile(1);
                break;
            case 3: // D
                MakeProjectile(2);
                break;
            case 4: // L
                MakeProjectile(3);
                break;
            case 5: // UD
                MakeProjectile(0);
                MakeProjectile(2);
                break;
            case 6: // RL
                MakeProjectile(1);
                MakeProjectile(3);
                break;
            default:
                Debug.LogError("unhandled weapon id");
                break;
        }
    }

    void MakeProjectile(int dir) {
        GameObject pObj = Instantiate(projectileObj, transform.position, transform.rotation);
        Projectile p = pObj.GetComponent<Projectile>();
        p.Init(Atk, Util.IntToDir(dir));
    }

    public override void Activate() {
        isAttacking = true;
    }

    public override void Deactivate() {
        isAttacking = false;
    }
}