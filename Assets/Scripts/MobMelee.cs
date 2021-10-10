using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMelee : Mob
{
    /** Collider qui d�tecte si l'attaque du mob a touch�. */
    Collider2D clawCollider;
    /** Timer depuis le d�but de l'attaque. */
    float attackTimer = 0;

    // Initialisation des valeurs propres au mob de m�l�e.
    void Start()
    {
        hp = 3;
        dmg = 2;
        speed = 50f;
        reach = 0.4f;
        mobRb.AddForce(direction * speed);
        clawCollider = transform.GetChild(0).GetComponent<Collider2D>();
        clawCollider.enabled = false;
    }

    void Update()
    {

        tmpTimer += Time.deltaTime;

        //Si le joueur est � bonne distance.
        if (DetectPlayer())
        {
            // Lancement de l'attaque.
            if (!attacking)
            {
                attacking = true;
                mobRb.AddForce(-direction * speed);
            }
        }

        if (attacking)
        {
            // On laisse 0.5 secondes au joueur pour esquiver, puis on active le collider d'attaque.
            if (tmpTimer >= 0.5f) clawCollider.enabled = true;
            if (tmpTimer >= 1f) clawCollider.enabled = false;

            // Fin de l'attaque
            if (tmpTimer >= 2f)
            {
                tmpTimer = 0;
                attacking = false;
                if (!DetectPlayer()) mobRb.AddForce(direction * speed);
                // Si le joueur est toujours � port�e on relance l'attaque.
                else attacking = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player")) collider.transform.GetComponent<DummyPlayer>().Hit(dmg);
    }
}
