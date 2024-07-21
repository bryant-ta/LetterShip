using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : Singleton<World> {
    [SerializeField] float minX, maxX;
    [SerializeField] float minY, maxY;
    [SerializeField] float exclusionRadius = 5f; // Radius around (0,0) where objects won't spawn

    [SerializeField] GameObject coinObj;
    [SerializeField] float spawnRate = 1f;
    [SerializeField] int maxObjects = 100;

    public int NumCurCoin;

    Player player;

    void Awake() { player = Ref.Player; }

    void Start() { StartCoroutine(SpawnObjects()); }

    void LateUpdate() {
        // stay in bounds
        float x = Mathf.Clamp(player.transform.position.x, minX, maxX);
        float y = Mathf.Clamp(player.transform.position.y, minY, maxY);
        player.Ship.transform.position = new Vector3(x, y, player.transform.position.z);
    }

    IEnumerator SpawnObjects() {
        float maxDistance = Mathf.Sqrt(Mathf.Pow(maxX, 2) + Mathf.Pow(maxY, 2));
        
        while (true) {
            yield return new WaitForSeconds(spawnRate);

            if (NumCurCoin < maxObjects) {
                Vector2 randomPosition;
                do {
                    randomPosition = new Vector2(
                        Random.Range(minX, maxX),
                        Random.Range(minY, maxY)
                    );
                } while (!ShouldSpawnAtPosition(randomPosition, maxDistance));

                Instantiate(coinObj, randomPosition, Quaternion.identity);
                NumCurCoin++;
            }
        }
    }

    bool ShouldSpawnAtPosition(Vector2 position, float maxDistance) {
        float distanceFromCenter = position.magnitude;
        if (distanceFromCenter < exclusionRadius) {
            return false; // Exclude positions within the exclusion radius
        }
        float spawnChance = distanceFromCenter / maxDistance; // Linearly increase chance with distance
        return Random.value < spawnChance;
    }
}