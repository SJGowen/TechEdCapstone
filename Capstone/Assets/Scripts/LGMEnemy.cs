using UnityEngine;

public class LGMEnemy : EnemyAI
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
    }

    public override void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyHit(_damageDone, _hitDirection, _hitForce); // calling the Enemyhit Method from its base class
    }
}
