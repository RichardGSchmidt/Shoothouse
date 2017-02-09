﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actors : MonoBehaviour, IDamageable {

    public float startingHealth;
    protected float health;
    protected bool dead;

	protected virtual void Start () {
        health = startingHealth;
		
	}
	
    public void TakeHit(float dmg, RaycastHit hit)
    {
        health -= dmg;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    protected void Die()
    {
        dead = true;
        GameObject.Destroy(gameObject);
    }
}