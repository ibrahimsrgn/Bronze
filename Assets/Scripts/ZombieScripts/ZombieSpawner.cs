using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Zombie
{
    public GameObject zombiePrefab;
    public float spawnLuck;
}
public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRangeFromPlayer;
    [SerializeField] private float spawnInterval;
    [SerializeField] private bool canSpawn;

    [SerializeField] private Zombie[] zombies;


    [SerializeField] private int maxZombieCount;
    public int zombieCount = 0;


    private void Start()
    {
        StartCoroutine(SpawnZombie());
    }
    public Vector3 FindSpawnPlacesAroundPlayer()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(spawnRangeFromPlayer - 1, spawnRangeFromPlayer + 1);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
        return player.position + offset;

    }
    private GameObject RandomZombiePicker()
    {
        float totalLuck = 0;

        foreach (Zombie zombie in zombies)
        {
            totalLuck += zombie.spawnLuck;
        }
        float dice = Random.Range(0, totalLuck);
        float cumulativeLuck = 0;
        foreach (Zombie zombie1 in zombies)
        {
            cumulativeLuck += zombie1.spawnLuck;
            if (dice < cumulativeLuck)
            {
                return zombie1.zombiePrefab;
            }
        }
        return zombies[0].zombiePrefab;

    }
    private IEnumerator SpawnZombie()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (canSpawn && zombieCount < maxZombieCount)
            {
                zombieCount++;
                GameObject zombie = Instantiate(RandomZombiePicker(), FindSpawnPlacesAroundPlayer(), Quaternion.identity);
                ZombieAi zombieAi = zombie.GetComponent<ZombieAi>();
                zombieAi.player = player;
                zombieAi.zombieSpawner = this;

            }

        }
    }

}
