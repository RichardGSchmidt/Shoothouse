using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Actors {

    NavMeshAgent pathfinder;
    Transform target;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(UpdatePath());
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (target != null&&!dead)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            pathfinder.SetDestination(targetPosition);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
