using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShieldSpawner : MonoBehaviour
{
    [Tooltip("The number of seconds that the shield remains active")] [SerializeField] float duration;
    [SerializeField] GameObject prefabToSpawn;
    [Tooltip("Minimum time between consecutive spawns, in seconds")] [SerializeField] float minTimeBetweenSpawns = 1f;
    [Tooltip("Maximum time between consecutive spawns, in seconds")] [SerializeField] float maxTimeBetweenSpawns = 3f;
    [Tooltip("Maximum distance in X between spawner and spawned objects, in meters")] [SerializeField] float maxXDistance = 0.5f;
    [Tooltip("Maximum distance in Y between spawner and spawned objects, in meters")] [SerializeField] float maxYDistance = 0.5f;
    GameObject myPrefab;
    GameObject player;
    private void Start()
    {
        this.StartCoroutine(SpawnRoutine());
    }
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            myPrefab = GameObject.Find("Shield(Clone)");
            player = GameObject.Find("PlayerSpaceship");
            var destroyComponent = player.GetComponent<DestroyOnTrigger2D>();
            if (myPrefab==null && destroyComponent.enabled == false)
            {
                yield return new WaitUntil(() => destroyComponent.enabled);
            }
            if (myPrefab == null)
            {
                Debug.Log("Shield created!");
                float timeBetweenSpawns = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
                yield return new WaitForSeconds(timeBetweenSpawns);
                Vector3 positionOfSpawnedObject = new Vector3(
                    transform.position.x + Random.Range(-maxXDistance, +maxXDistance),
                    transform.position.y + Random.Range(-maxYDistance, +maxYDistance),
                    transform.position.z);
                GameObject newObject = Instantiate(prefabToSpawn.gameObject, positionOfSpawnedObject, Quaternion.identity);
            }
            if (myPrefab != null)
            {
                yield return new WaitUntil(() =>( myPrefab = GameObject.Find("Shield(Clone)") )==null);
            }

        }
    }


}
