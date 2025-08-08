using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class DroidEnemy : EnemyAI
{
    private float _droidTimer;
    [SerializeField] private float _droidFlipWaitTime;
    [SerializeField] private float _droidLedgeCheckX;
    [SerializeField] private float _droidLedgeCheckY;

    [SerializeField] private LayerMask whatIsGround;
    private AudioSource _droidSound;
    public AudioClip _robotNoise;

    protected override void Awake()
    {
        base.Awake(); // calling the Awake method from its base class
    }

    public override void Start()
    {
        _enemyRigidBody.gravityScale = 12f; // Applies gravity scale to this Rigidbody
        _droidSound = GetComponent<AudioSource>();
        _droidSound.Play();
    }
     protected override void UpdateEnemyStates()
    {
       
        switch (currentEnemyState)
        {
            case EnemyStates.Droid_idle:

                Vector3 _droidLedgeCheckStartPoint = transform.localScale.x > 0 ? new Vector3(_droidLedgeCheckX, 0) : new Vector3(-_droidLedgeCheckX, 0);
                Vector2 _droidWallCheckDir = transform.localScale.x > 0 ? transform.right :  -transform.right;

                if (!Physics2D.Raycast(transform.position + _droidLedgeCheckStartPoint, Vector2.down, _droidLedgeCheckY, whatIsGround)
                    || Physics2D.Raycast(transform.position, _droidWallCheckDir, _droidLedgeCheckX, whatIsGround))
                {
                    ChangeState(EnemyStates.Droid_Flip);
                }

                if (transform.localScale.x > 0)
                {
                    _enemyRigidBody.linearVelocity = new Vector2(_enemySpeed, _enemyRigidBody.linearVelocity.y);
                }
                else
                {
                    _enemyRigidBody.linearVelocity = new Vector2(-_enemySpeed, _enemyRigidBody.linearVelocity.y);
                }
                break;

            case EnemyStates.Droid_Flip:
                _droidTimer += Time.deltaTime;

                if (_droidTimer > _droidFlipWaitTime)
                {
                    _droidTimer = 0;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                    ChangeState(EnemyStates.Droid_idle);
                }
                break;
        }
    }


   
}