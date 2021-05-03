using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpTags { TripleShotPowerUp, SpeedBoostPowerUp, ShieldsPowerUp }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement() {
        transform.Translate(Vector3.down * Time.deltaTime * UnityEngine.Random.Range(0.3f, 5f));
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
                switch (Enum.Parse(typeof(PowerUpTags), gameObject.tag)) {
                    case PowerUpTags.TripleShotPowerUp:
                        player.SetPowerUp(PowerUpTags.TripleShotPowerUp);
                        break;
                    case PowerUpTags.SpeedBoostPowerUp:
                        player.SetPowerUp(PowerUpTags.SpeedBoostPowerUp);
                        break;
                    case PowerUpTags.ShieldsPowerUp:
                        player.SetPowerUp(PowerUpTags.ShieldsPowerUp);
                        break;
                    default:
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
