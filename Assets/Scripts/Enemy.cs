using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Actors {

    public enum State
    {
        Idle,
        Chasing,
        Attacking
    };
    State currentState;
    NavMeshAgent pathfinder;
    Transform target;
    float attackDelay = 1;
    float nextAttack;

    float attackDistanceThreshold = 1.5f;

    protected override void Start ()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();

        currentState = State.Chasing;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
	}

    // Update is called once per frame
    void Update()
    {

        if (Time.time > nextAttack)
        { 
            float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;

            if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThreshold, 2))
            {
                nextAttack = Time.time + attackDelay;
                StartCoroutine(Attack());
            }
        }

	}

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;
        Vector3 originalPos = transform.position;
        Vector3 attackPos = target.position;

        float attackSpeed = 3;
        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPos, attackPos, interpolation);

            yield return null;
        }
        currentState = State.Chasing;
        pathfinder.enabled = true;

    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (target != null&&!dead)
        {
            if (currentState == State.Chasing)
            {
                Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
                pathfinder.SetDestination(targetPosition);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
