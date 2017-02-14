using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actors : MonoBehaviour, IDamageable {

    public float startingHealth;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

	protected virtual void Start () {
        health = startingHealth;
		
	}
	
    public void TakeHit(float dmg, RaycastHit hit)
    {
        //for particle effects.
        TakeDmg(dmg);
    }

    public void TakeDmg(float dmg)
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

        if(OnDeath!=null)
        {
            OnDeath();
        }

        GameObject.Destroy(gameObject);
    }
}
