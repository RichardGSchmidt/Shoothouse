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

    Actors targetActor;

    float damage = 1;
    float attackDelay = 1;
    float nextAttack;

    float attackDistanceThreshold = 1.5f;

    bool hasTarget;

    protected override void Start ()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetActor = target.GetComponent<Actors>();
            targetActor.OnDeath += OnTargetDeath;
            StartCoroutine(UpdatePath());
        }
	}

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }


    // Update is called once per frame
    void Update()
    {

        if (hasTarget&&Time.time > nextAttack)
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
        bool hasAttacked = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAttacked)
            {
                hasAttacked = true;
                targetActor.TakeDmg(damage);
            }
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

        while (hasTarget&&!dead)
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
