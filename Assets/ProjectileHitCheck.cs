﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitCheck : MonoBehaviour {

    float damage;
    public float timeToLive = 3.0f;
    public bool isGrenade = false;

    float closeAreaEffect = 5f;
    float mediumAreaEffect  = 10;
    float farAreaEffect  = 15;

	void Start () 
    {
        damage = 1.0f;
	}
	
	void Update () 
    {
        Destroy(this.gameObject, timeToLive);
	}

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);

            StartCoroutine(this.TimedExplosion());
        }
        else
        {
            Debug.Log("Hit somethin else?");
        }
    }

    IEnumerator TimedExplosion()
    {
        yield return new WaitForSeconds(5.0f);

        Destroy(this.gameObject);

        var colls = Physics.OverlapSphere(transform.position, farAreaEffect);

        foreach(Collider col in colls)
        {
            
            if (col.CompareTag("Wall"))
            { 
                
                float distance = Vector3.Distance(col.transform.position, transform.position);
                damage = 0.5f;
                if (distance <= closeAreaEffect)
                {
                    damage = 1; // but if inside close area, change to max damage
                }
                else if (distance <= mediumAreaEffect)
                {
                    damage = 0.75f; // else if inside medium area, change to medium damage
                }
                // apply the selected damage
                col.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
                
                Debug.Log(col.gameObject.name + " hit. Health: " + col.gameObject.GetComponent<ApplyHit>().hitPoints);
            }
        }
    }
    void Explosion()
    {
        var colls = Physics.OverlapSphere(transform.position, farAreaEffect);

        foreach (Collider col in colls)
        {
            Debug.Log(col.gameObject.name);
            if (col.CompareTag("Wall"))
            {
                float distance = Vector3.Distance(col.transform.position, transform.position);
                damage = 0.5f;
                if (distance <= closeAreaEffect)
                {
                    damage = 1; // but if inside close area, change to max damage
                }
                else if (distance <= mediumAreaEffect)
                {
                    damage = 0.75f; // else if inside medium area, change to medium damage
                }
                // apply the selected damage
                col.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
