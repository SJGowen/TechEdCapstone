using UnityEngine;


public class LGMEnemy : EnemyAI
{
    [SerializeField] public GameObject _enemyLaserBullet; // enemy Laser GameObject 
    [SerializeField] public Transform _enemyLaserPos; // enemy Laser Position
    [SerializeField] private float _lgmTimer; // Time between shots
    [SerializeField] private float _lgmShootDistance = 7f;
    
    // [SerializeField] private float startDelay = 1f;


    private Animator _lgmAnim; // Animations

    protected override void Awake()
    {
        base.Awake(); // calling the Awake method from its base class
    }

    public override void Start()
    {
        _enemyRigidBody.gravityScale = 12f; // Applies gravity scale to this Rigidbody
        _lgmAnim = GetComponent<Animator>();
        
        
    }

    protected override void Update()
    {
        base.Update(); // calling the Update method from its base class

        if (!_enemyIsRecoilling)
        {
            float _lgmDistance = Vector2.Distance(transform.position, player.transform.position);


            if (_lgmDistance < _lgmShootDistance)
            {
                _lgmTimer += Time.deltaTime;


                if (_lgmTimer > 1)
                {
                    _lgmTimer = 0;
                    Shoot();
                }
            }
            else
            {
                _lgmAnim.Play("Idle");
            }
        }
        FlipLGM();
    }

    

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce); // calling the Enemyhit Method from its base class
        Debug.Log("Hit!");
    }

    void Shoot()
    {
        _lgmAnim.Play("Shooting");
        Destroy(Instantiate(_enemyLaserBullet, _enemyLaserPos.position, Quaternion.identity), 4);
    }


    void FlipLGM()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


}

//  protected override void UpdateEnemyStates()
    // {
    //     float _lgmDistance = Vector2.Distance(transform.position, player.transform.position);
    //     switch (currentEnemyState)
    //     {
    //         case EnemyStates.LGM_Idle:

    //             if (_lgmDistance < _lgmShootDistance)
    //             {
    //                 ChangeState(EnemyStates.LGM_Shoot);
    //                 Debug.Log("Changestate");
    //             }
    //             break;

    //         case EnemyStates.LGM_Shoot:
    //             _lgmTimer += Time.deltaTime;
    //             if (_lgmTimer > 1)
    //             {
    //                 Shoot();
    //                 _lgmTimer = 0;
    //             }
    //             FlipLGM();
    //             break;
    //     }
    // }

