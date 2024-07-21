using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed = 10f;
    public float lifetime = 5f;
    public int Dmg;

    public void Init(int dmg, Vector2 dir) {
        Dmg = dmg;

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
        if (col.CompareTag("Player")) return;
        if (col.TryGetComponent(out Bit b) && b.BodyCol == col) {
            b.DoDamage(Dmg);
            Destroy(gameObject);
        }
    }
}