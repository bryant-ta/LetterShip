using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : Singleton<World> {
    [SerializeField] float bound;
    public float minX, maxX;
    public float minY, maxY;
    [SerializeField] float exclusionRadius = 5f; // Radius around (0,0) where objects won't spawn

    [SerializeField] GameObject coinObj;
    [SerializeField] float coinSpawnRate = 1f;
    [SerializeField] int maxCoins = 100;

    [SerializeField] Transform coinHolder;
    
    [SerializeField] float enemySpawnRate = 10f;
    [SerializeField] int maxEnemies = 10;
    [SerializeField] List<string> enemyShipNames = new();

    public int NumCurCoin;
    public int NumCurEnemies;

    Player player;

    void Awake() {
        player = Ref.Player;
        minX = -bound;
        minY = -bound;
        maxX = bound;
        maxY = bound;

        enemyShipNames = Factory.Instance.LoadEnemyNames();
    }

    void Start() { 
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnEnemies());
    }

    void LateUpdate() {
        // stay in bounds
        float x = Mathf.Clamp(player.transform.position.x, minX, maxX);
        float y = Mathf.Clamp(player.transform.position.y, minY, maxY);
        if (player.Ship != null) {
            player.Ship.transform.position = new Vector3(x, y, player.transform.position.z);
        }
    }

    IEnumerator SpawnCoins() {
        float maxDistance = Mathf.Sqrt(Mathf.Pow(maxX, 2) + Mathf.Pow(maxY, 2));
        
        while (true) {
            yield return new WaitForSeconds(coinSpawnRate);

            for (int i = 0; i < 5; i++) {
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

                string enemyName = enemyShipNames[Random.Range(0, enemyShipNames.Count)];
                Ship enemyShip = Factory.Instance.CreateShip(enemyName, randomPosition);
                enemyShip.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

                NumCurEnemies++;

                enemyShip.tag = "Enemy";
                for (int i = 0; i < enemyShip.transform.childCount; i++) {
                    enemyShip.transform.GetChild(i).tag = "Enemy";
                    if (enemyShip.transform.GetChild(i).TryGetComponent(out Bit bit)) {
                        if (bit is Weapon weapon) {
                            weapon.Activate();
                        } else if (bit is Thruster thruster) {
                            thruster.Activate();
                        }
                    }
                }
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
