using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyLaserScript : EnemyAI
{
    
    [SerializeField] Rigidbody2D _lgmLaser;
    [SerializeField] public float _laserForce;

    private AudioSource _laserSound;
    public AudioClip _laserBlast;
    // [SerializeField] private float
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {

        _lgmLaser = GetComponent<Rigidbody2D>(); // laser bullet
        _lgmLaser.gravityScale = 0f; // Applies gravity scale to this Rigidbody
        _laserSound = GetComponent<AudioSource>();


        Vector3 direction = player.transform.position - transform.position;
        _lgmLaser.linearVelocity = new Vector2(direction.x, direction.y).normalized * _laserForce;

        float _lgmLaserRot = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _lgmLaserRot + 90);

        _laserSound.PlayOneShot(_laserBlast, 1f);
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
