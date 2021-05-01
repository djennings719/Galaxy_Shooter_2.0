using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement() {
        transform.Translate(Vector3.down * Time.deltaTime * Random.Range(0.3f, 5f));
        CheckBounds();
    }

    private void CheckBounds() {
        if (transform.position.y < -8f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            Player player = other.GetComponent<Player>();
            if (player != null) {
                player.SetPowerUp(Player.PowerUps.TripleShot);
            }
            Destroy(gameObject);
        }
    }
}
