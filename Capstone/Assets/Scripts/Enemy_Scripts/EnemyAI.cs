using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] protected float _enemyHealth; // Enemy's health
    [SerializeField] protected float _enemyRecoilLength; // Enemy's recoil distance
    [SerializeField] protected float _enemyRecoilFactor; // Strength of the recoil when enemy takes damage
    [SerializeField] protected bool _enemyIsRecoilling = false; // whether the enemy is recoiling or not

    [SerializeField] protected PlayerController player; // PlayerControllerscript.cs
    
    [SerializeField] protected float _enemySpeed; //Enemy's Speed
    [SerializeField] protected float _enemydamage; // Damage that enemy does to player

    protected float _enemyRecoilTimer; // How long the enemy is recoilling for
    protected Rigidbody2D _enemyRigidBody; // Enemy RigidBody

    protected bool _enemyIsFlipped = false;


    protected virtual void Awake()
    {
        _enemyRigidBody = GetComponent<Rigidbody2D>(); // Getting that enemy's Rigidbody
        player = PlayerController.Instance; // PlayerControllerScript is referenced as "player" in this script and its children
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_enemyHealth <= 0) // That Enemy has no health left
        {
            Destroy(gameObject); // Destroys that Enemy 
        }
        if (_enemyIsRecoilling)
        {
            if (_enemyRecoilTimer < _enemyRecoilLength)
            {
                _enemyRecoilTimer += Time.deltaTime;
            }
            else
            {
                _enemyIsRecoilling = false;
                _enemyRecoilTimer = 0;
            }
        }
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        _enemyHealth -= _damageDone; // health lost is equivalent to damage done by player
        if (!_enemyIsRecoilling)
        {
            _enemyRigidBody.AddForce(-_hitForce * _enemyRecoilFactor * _hitDirection);
            // Force from player hit moves that enemy to new position in relation to the hitforce, hit direction and that enemy's recoil factor
            _enemyIsRecoilling = true; // Set Enemy recoilling state to true
        }
    }

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !player.pState.invincible)
        {
            Attack();
            player.HitStopTime(0, 2, 0.2f); // add line back in after pulling from updated dev  branch

        }
    }

    protected virtual void Attack()
    {
        player.TakeDamage(_enemydamage); // Deals varying damage to the player
    }

    // protected virtual void LookAtplayer()
    // {
    //     Vector3 flipped = transform.localScale;
    //     flipped.z *= -1f;

    //     if (transform.position.x > player.transform.position.x && _enemyIsFlipped)
    //     {
    //         transform.localScale = flipped;
    //         transform.Rotate(0f, 180f, 0f);
    //         _enemyIsFlipped = false;
    //     }
    //     else if (transform.position.x < player.transform.position.x && _enemyIsFlipped)
    //     {
    //         transform.localScale = flipped;
    //         transform.Rotate(0f, 180f, 0f);
    //         _enemyIsFlipped = true;
    //     }

    // }
} 



