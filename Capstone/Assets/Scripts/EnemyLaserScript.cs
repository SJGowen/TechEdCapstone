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
        _lgmLaser = GetComponent<Rigidbody2D>();
        //player = 

        Vector3 direction = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
