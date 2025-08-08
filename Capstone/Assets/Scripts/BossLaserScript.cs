using UnityEngine;

public class BossLaserScript : MonoBehaviour
{ 
    [SerializeField] Rigidbody2D _bossLaser;
    [SerializeField] public float _bosslaserForce;
    [SerializeField] public float _bossDamage;

    private AudioSource _bossLaserSound;
    public AudioClip _bossLaserBlast;

    [SerializeField] protected PlayerController player; // PlayerControllerscript.cs


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        player = PlayerController.Instance;
        _bossLaser = GetComponent<Rigidbody2D>(); // laser bullet
        _bossLaser.gravityScale = 0f; // Applies gravity scale to this Rigidbody
        _bossLaserSound = GetComponent<AudioSource>();




        Vector3 direction = player.transform.position - transform.position;
        _bossLaser.linearVelocity = new Vector2(direction.x, direction.y).normalized * _bosslaserForce;

        float _lgmLaserRot = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _lgmLaserRot + 90);

        _bossLaserSound.PlayOneShot(_bossLaserBlast, 1f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(_bossDamage);
            Destroy(gameObject);
        }
    }

    
}
