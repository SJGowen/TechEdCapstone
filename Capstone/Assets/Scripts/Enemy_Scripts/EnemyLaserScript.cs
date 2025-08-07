using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLaserScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] Rigidbody2D _lgmLaser;
    [SerializeField] public float _laserForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lgmLaser = GetComponent<Rigidbody2D>(); // laser bullet
        player = GameObject.FindGameObjectWithTag("Player"); // Find Player



        Vector3 direction = player.transform.position - transform.position;

        _lgmLaser.linearVelocity = new Vector2(direction.x, direction.y).normalized * _laserForce;

        float _lgmLaserRot = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, _lgmLaserRot + 90);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().health -= 1;
            Destroy(gameObject);
        }
    }
}
