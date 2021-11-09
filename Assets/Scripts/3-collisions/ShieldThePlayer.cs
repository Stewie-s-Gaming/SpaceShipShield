using System.Collections;
using UnityEngine;

public class ShieldThePlayer : MonoBehaviour {
    [Tooltip("The number of seconds that the shield remains active")] [SerializeField] float duration;
    [SerializeField] GameObject prefabToSpawn;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("Shield triggered by player");
            var destroyComponent = other.GetComponent<DestroyOnTrigger2D>();
            if (destroyComponent) {
                destroyComponent.StartCoroutine(ShieldTemporarily(destroyComponent));
                // NOTE: If you just call "StartCoroutine", then it will not work, 
                //       since the present object is destroyed!
                Destroy(gameObject);  // Destroy the shield itself - prevent double-use
            }
        } else {
            Debug.Log("Shield triggered by "+other.name);
        }
    }
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
}
