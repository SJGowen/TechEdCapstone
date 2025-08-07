using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLaserScript : EnemyAI
{
    
    [SerializeField] Rigidbody2D _lgmLaser;
    [SerializeField] public float _laserForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {

        _lgmLaser = GetComponent<Rigidbody2D>(); // laser bullet
        _lgmLaser.gravityScale = 0f; // Applies gravity scale to this Rigidbody


        Vector3 direction = player.transform.position - transform.position;
        _lgmLaser.linearVelocity = new Vector2(direction.x, direction.y).normalized * _laserForce;

        float _lgmLaserRot = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _lgmLaserRot + 90);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
            Destroy(gameObject);
        }
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
