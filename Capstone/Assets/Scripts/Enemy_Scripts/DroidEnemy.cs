using UnityEngine;

public class DroidEnemy : EnemyAI
{
    protected override void Awake()
    {
        base.Awake(); // calling the Awake method from its base class
    }

    public override void Start()
    {
        _enemyRigidBody.gravityScale = 12f; // Applies gravity scale to this Rigidbody
    }
    protected override void Update()
    {
        base.Update(); // calling the Update method from its base class 
        if (!_enemyIsRecoilling)
        {
            transform.position = Vector2.MoveTowards
                (transform.position, new Vector2(player.transform.position.x, transform.position.y),
                 _enemySpeed * Time.deltaTime); // Droid enemy class chase mechanic 
        }
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce); // calling the Enemyhit Method from its base class
        Debug.Log("Hit!");
    }
}

// float _lgmDistance = Vector2.Distance(transform.position, player.transform.position);
//             Debug.Log(_lgmDistance);
