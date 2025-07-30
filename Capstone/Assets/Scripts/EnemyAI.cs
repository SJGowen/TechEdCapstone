using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] protected float _enemyHealth; // Enemy's health
    [SerializeField] protected float _enemyRecoilLength;
    [SerializeField] protected float _enemyRecoilFactor; // Strength of the recoil when enemy takes damage
    [SerializeField] protected bool _enemyIsRecoilling = false; // whether the enemy is recoiling or not

    // [SerialzeField] protected PlayerController player; 
    [SerializeField] protected float _enemySpeed; //Enemy's Speed


    protected float _enemyRecoilTimer; // How long the enemy is recoilling for
    protected Rigidbody2D _enemyRigidBody; //Enemy RigidBody


    public virtual void Awake()
    {
        _enemyRigidBody = GetComponent<Rigidbody2D>(); // Getting that enemy's Rigidbody
        // player = PlayerController.Instance; // referencing the Playercontroller script
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
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
        }
    }

} 

/*
//FOR THE PLAYER CONTROLLER

void Hit(Transform _attackTransform, Vector2 _attackArea) // this is to reference the player hitting an enemy
{
    Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

    if(objectsToHit.Length > 0)
    { 
        Debug.Log("Hit");
    }
    for(int i = 0; i < objectsToHit.Length; i++) 
    // before: set i(index) to zero, condition: loop starts if "i" is less than number of enteries in the ObjectsToHit.Length array; after: "i" is added to if condition hasn't been met
    {
        if(objectsToHit[i].getComponent<Enemy>() != null)
        {
            objectsToHit[i].GetComponent<Enemy>().EnemyHit(dammage, (transform.position - objectsToHit[i].transform.position).normalized, 100);
            // Determines the direction of the player's hit, "100" is the hit force value
        }
    }
}*/
