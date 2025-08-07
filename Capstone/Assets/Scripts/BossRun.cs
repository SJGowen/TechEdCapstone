using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 7f;
    private Animator _bossAnim;
    [SerializeField] private float _bossTimer; // Time between shots
	[SerializeField] public GameObject _bossLaserBullet; // enemy Laser GameObject 
	[SerializeField] public Transform _bossLaserPos; // enemy Laser Position

    Transform player;
    Rigidbody2D rb;
    BossFight boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bossAnim.Play("Idle_Boss");
        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);


        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            //Attack the player
            _bossAnim.Play("Shooting");
            Shoot();

        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Shooting_Boss");
    }
    
     void Shoot()
    {
        Destroy(Instantiate(_bossLaserBullet, _bossLaserPos.position, Quaternion.identity), 4);
    }
}

