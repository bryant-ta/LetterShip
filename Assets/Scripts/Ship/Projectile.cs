using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 5f;
    public int Dmg;

    bool isPlayerMade;

    public void Init(int dmg, Vector2 dir, bool isPlayerMade) {
        Dmg = dmg;
        this.isPlayerMade = isPlayerMade;

        // Set the rotation to face the direction
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy the projectile after its lifetime
        Destroy(gameObject, lifetime);
    }

    void Update() {
        // Move the projectile forward in local space
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (isPlayerMade && col.CompareTag("Player") || !isPlayerMade && col.CompareTag("Enemy")) {
            return;
        }
        
        if (col.TryGetComponent(out Bit b) && b.BodyCol == col) {
            b.DoDamage(Dmg);
            Destroy(gameObject);
        }
    }
}