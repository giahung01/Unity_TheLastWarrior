using UnityEngine;

public class FlyEnemy : Enemy
{
    [Header("Fly Setting")]
    [SerializeField] protected Vector2 limitBoxSize;
    private Vector2 originalPosition;

    protected override void Start()
    {
        base.Start();
        originalPosition = transform.position;
        rb.gravityScale = 0;
    }
    protected override void Update()
    {
        base.Update();
        if (DetectPlayer() && isDetected == false && !PlayerManager.instance.player.IsDead)
        {
            stateMachine.ChangeState(battleState);
        }
    }

    public bool IsInsideFlyBox(Vector2 current)
    {
        Vector2 minLimit = originalPosition - limitBoxSize / 2f;
        Vector2 maxLimit = originalPosition + limitBoxSize / 2f;

        if (current.x > minLimit.x && current.x < maxLimit.x &&
            current.y > minLimit.y && current.y < maxLimit.y)
        {
            return true;
        }
        return false;
    }
    public void BackToFLyBox()
    {
        transform.position = Vector2.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime); 
    }
    public bool isInOriginalPos()
    {
        BackToFLyBox();
        if ((Vector2)transform.position == originalPosition)
        {
            return true;
        }
        return false;
    }
   
    public bool DetectPlayer()
    {
        var detectPosition = new Vector2(transform.position.x + additionalDetectPosition.x * EntityDir, transform.position.y);
        var colliders = Physics2D.OverlapCircleAll(detectPosition, detectPlayerDistance);
        if (colliders != null)
        {
            foreach(var collider in colliders)
            {
                LayerMask hitLayer = 1 << collider.gameObject.layer;
                if (hitLayer == whatIsPlayer)
                    return true;
            }
        }

        return false;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectPlayerDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(originalPosition, limitBoxSize);
    }
}
