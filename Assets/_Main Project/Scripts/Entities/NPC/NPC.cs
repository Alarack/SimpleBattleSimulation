using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LL.Events;

using System;


public class NPC : Entity
{

    public AIBrain Brain { get; protected set; }

    public Timer spawnInTimer;
    public float spawnDelayTime = 0.5f;
    private Collider2D myCollider;

    public bool active;

    protected override void Awake() {
        base.Awake();
        Brain = GetComponent<AIBrain>();
        myCollider = GetComponent<Collider2D>();
        
        if(myCollider != null )
            myCollider.enabled = false;

        if (spawnDelayTime > 0f)
            spawnInTimer = new Timer(spawnDelayTime, OnSpawnInComplete, false);
        else
            OnSpawnInComplete(null);

    }



    protected override void Update() {
        base.Update();

        if(spawnInTimer != null && active == false) {
            spawnInTimer.UpdateClock();
        }

    }


 
    private void OnSpawnInComplete(EventData data) {
        active = true;
        if(myCollider != null) 
            myCollider.enabled = true;
    }


    public override void Die(Entity source)
    {
        base.Die(source);

        EntityManager.RemoveEntity(this);

        SpawnDeathVFX();
        Destroy(gameObject, 0.1f);
    }

}
