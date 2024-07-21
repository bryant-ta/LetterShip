using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : Singleton<World> {
    [SerializeField] float minX, maxX;
    [SerializeField] float minY, maxY;
    [SerializeField] float exclusionRadius = 5f; // Radius around (0,0) where objects won't spawn

    [SerializeField] GameObject coinObj;
    [SerializeField] GameObject[] enemyPool;
    [SerializeField] float coinSpawnRate = 1f;
    [SerializeField] float enemySpawnRate = 10f;
    [SerializeField] int maxCoins = 100;
    [SerializeField] int maxEnemies = 10;

    [SerializeField] Transform coinHolder;

    public int NumCurCoin;
    public int NumCurEnemies;

    Player player;

    void Awake() { player = Ref.Player; }

    void Start() { 
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnEnemies());
    }

    void LateUpdate() {
        // stay in bounds
        float x = Mathf.Clamp(player.transform.position.x, minX, maxX);
        float y = Mathf.Clamp(player.transform.position.y, minY, maxY);
        player.Ship.transform.position = new Vector3(x, y, player.transform.position.z);
        
        WrapEnemies();
    }

    IEnumerator SpawnCoins() {
        float maxDistance = Mathf.Sqrt(Mathf.Pow(maxX, 2) + Mathf.Pow(maxY, 2));
        
        while (true) {
            yield return new WaitForSeconds(coinSpawnRate);

            if (NumCurCoin < maxCoins) {
                Vector2 randomPosition;
                do {
                    randomPosition = new Vector2(
                        Random.Range(minX, maxX),
                        Random.Range(minY, maxY)
                    );
                } while (!ShouldSpawnAtPosition(randomPosition, maxDistance));

                GameObject o = Instantiate(coinObj, randomPosition, Quaternion.identity);
                o.transform.parent = coinHolder;
                NumCurCoin++;
            }
        }
    }

    IEnumerator SpawnEnemies() {
        while (true) {
            yield return new WaitForSeconds(enemySpawnRate);

            if (NumCurEnemies < maxEnemies) {
                Vector2 randomPosition;
                do {
                    randomPosition = new Vector2(
                        Random.Range(minX, maxX),
                        Random.Range(minY, maxY)
                    );
                } while (randomPosition.magnitude < exclusionRadius);

                GameObject enemyPrefab = enemyPool[Random.Range(0, enemyPool.Length)];
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

                GameObject enemyObj = Instantiate(enemyPrefab, randomPosition, randomRotation);
                NumCurEnemies++;

                Ship enemyShip = enemyObj.GetComponent<Ship>();
                enemyShip.ActivateAllSecondary();
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
    
    void WrapEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies) {
            Vector3 pos = enemy.transform.position;
            bool wrapped = false;

            if (pos.x > maxX) {
                pos.x = minX;
                wrapped = true;
            } else if (pos.x < minX) {
                pos.x = maxX;
                wrapped = true;
            }

            if (pos.y > maxY) {
                pos.y = minY;
                wrapped = true;
            } else if (pos.y < minY) {
                pos.y = maxY;
                wrapped = true;
            }

            if (wrapped) {
                enemy.transform.position = pos;
            }
        }
    }
}
