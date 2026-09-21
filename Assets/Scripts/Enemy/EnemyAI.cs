using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public DetectionZone attackZone;
    public bool _hasTarget = false;
    public GameObject target;
    private CharacterBase targetChar;
    
    #region references
    
    private CharacterBase charOwner;
    public EnemyAttackControls enemyAttackControlsRef;
    public EnemyMovement enemyMovementRef;

    #endregion

    public bool hasTarget
    {
        get { return _hasTarget; }

        set
        {
            _hasTarget = value;
            //charOwner.animatorRef.SetBool("isAttacking", value);
            if (hasTarget)
            {
                enemyAttackControlsRef.Attack();
            }
        }
    }

    private void Awake()
    {
        charOwner = GetComponent<CharacterBase>();
        enemyMovementRef = GetComponent<EnemyMovement>();
        targetChar = target.GetComponent<CharacterBase>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.canFight || GameManager.instance.isGamePaused)
        {
            return;
        }

        hasTarget = attackZone.detectedColliders.Count > 0;

        if (!hasTarget && !charOwner.isDead && (targetChar != null && !targetChar.isDead) )
        {
            //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, enemyMovementRef.movementSpeed * Time.deltaTime);
            Vector2 direction = (target.transform.position - transform.position).normalized;
            enemyMovementRef.rb.linearVelocity = new Vector2(direction.x * enemyMovementRef.movementSpeed, enemyMovementRef.rb.linearVelocity.y);

            if (target.transform.position.x < transform.position.x)
            {
                enemyMovementRef.movementDirection = EnemyMovement.movementDirectionEnum.Left;
            }
            else
            {
                enemyMovementRef.movementDirection = EnemyMovement.movementDirectionEnum.Right;
            }
        }

        if (enemyAttackControlsRef.isAttacking)
        {
            enemyMovementRef.canMove = false;
        }
        else
        {
            enemyMovementRef.canMove = true;
        }
    }
}
