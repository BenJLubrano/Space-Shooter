using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class EnemyController : Ship
{

    [SerializeField] BaseEnemy enemyType;
    [SerializeField] List<GameObject> targets;
    [SerializeField] Rigidbody2D rb;
    Ship currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if(currentTarget == null)
        {
            LookForTargets();
        }
        else
        {
            if(OutOfRange())
            {
                Deaggro();
            }
        }
        Move();
    }

    void LookForTargets()
    {
        if (targets.Count == 0)
            return;
        GameObject tempTarget = targets[0];
        float closest = Vector2.Distance(gameObject.transform.position, tempTarget.transform.position);
        foreach(GameObject target in targets)
        {
            float tempDist = Vector2.Distance(gameObject.transform.position, target.transform.position);
            if (tempDist < closest)
            {
                tempTarget = target;
                closest = tempDist;
            }
        }
        currentTarget = tempTarget.GetComponent<Ship>();
    }

    public void UpdateTargets(List<GameObject> newTargets)
    {
        targets = newTargets;
    }

    bool OutOfRange()
    {
        return Vector2.Distance(currentTarget.gameObject.transform.position, gameObject.transform.position) > enemyType.aggroRange;
    }

    void Deaggro()
    {
        //going to do other stuff here later
        currentTarget = null;
    }
    
    void Move()
    {
        if(currentTarget != null)
        {
            transform.up = currentTarget.gameObject.transform.position - transform.position;
            rb.AddForce(transform.up * speed);
        }
    }
}
