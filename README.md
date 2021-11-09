
Erel's copied game, the unique difference are:

- The shield spawns at random places.

- The shield gets more transparent as the time of the shield runs out.

Our unique add: The shield only spawns after you used it once, meaning there can not be 2 shield at the same time, and not while the shield is in use.

* Also the shield spawns at random timing.

In the following code you may see I changed erel's code and added a new script - RandomShieldSpawner. This script randomly spawns shield around the map, and only once - there cannot be 2 shields at the same time. (That's why we are checking if the shield prefab exists every loop)
```
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
```
The second change we made to erel's code is in the shield's script. When the shield "crushes" into the player it spawns the shield object
on the player and with each second that passes the color of the shield fades.

```
    private IEnumerator ShieldTemporarily(DestroyOnTrigger2D destroyComponent) {
        destroyComponent.enabled = false;
        GameObject player = GameObject.Find("PlayerSpaceship");
        GameObject shield = Instantiate(prefabToSpawn.gameObject, player.transform.position, Quaternion.identity);
        var playerSpeed = player.GetComponent<KeyboardMover>();
        var shieldSpeed = shield.GetComponent<KeyboardMover>();
        shieldSpeed.speed = playerSpeed.speed;
        Material shieldColor = shield.GetComponent<Renderer>().material;
        Color old = shieldColor.color;
        float j = 1f;
        for (float i = duration; i > 0; i--) {
            j -= 0.2f;
            shieldColor.SetColor("_Color",new Color(old.r, old.g, old.b,j));
            Debug.Log("Shield: " + i + " seconds remaining!");
            yield return new WaitForSeconds(1);
        }
        Destroy(shield);
        Debug.Log("Shield gone!");
        destroyComponent.enabled = true;
    }
```
