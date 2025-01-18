using LL.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity {

    [Header("Visuals")]
    public GameObject impactVFX;

    [Header("Misc")]
    public bool varyInitalSpeed = true;

    [HideInInspector]
    public LayerMask projectileHitMask;

    private Collider2D myCollider;
    public Entity Source { get; private set; }
    private Weapon parentWeapon;


    private Task killTimer;
    private Task smoothScale;
    private float projectileSize;
    private float projectileDamage;

    protected override void Awake() {
        base.Awake();

        if (Stats.Contains(StatName.ProjectileSize) == false) {
            Stats.AddStat(new SimpleStat(StatName.ProjectileSize, 1f));
        }

        myCollider = GetComponent<Collider2D>();

        if(varyInitalSpeed == true) {
            float speedVariance = UnityEngine.Random.Range(-0.2f, 0.2f);
            Stats.AddModifier(StatName.MoveSpeed, speedVariance, StatModType.PercentAdd, this);
        }
       
        killTimer = new Task(KillAfterLifetime());

    }




    public void Setup(Entity source, Weapon parentWeapon, MaskTargeting maskTargeting = MaskTargeting.Same) {
        this.Source = source;
        this.parentWeapon = parentWeapon;
        SetupCollisionIgnore(source.GetComponent<Collider2D>());
        projectileHitMask = LayerTools.SetupHitMask(projectileHitMask, source.gameObject.layer, maskTargeting);

        ProjectileMovement move = Movement as ProjectileMovement;
        if (move != null) {
            move.SetSeekMask(source.gameObject.layer, maskTargeting);
        }

        SetupSize();
        SetupProjectileStats();
    }

    private void SetupProjectileStats() {
        if (Source == null)
            return;

        projectileDamage = Stats[StatName.BaseDamage] + parentWeapon.Stats[StatName.BaseDamage];

    }

    

    private void SetupSize() {
        UpdateProjectleSize();

        transform.localScale = new Vector3(projectileSize, projectileSize, projectileSize);
    }

    private void UpdateProjectleSize() {
        projectileSize = Stats[StatName.ProjectileSize];

        if (projectileSize <= 0)
            projectileSize = 1f;

        float globalSizeMod = 1f + Source.Stats[StatName.GlobalProjectileSizeModifier];

        projectileSize *= globalSizeMod;
    }

    public void SetupCollisionIgnore(Collider2D ownerCollider) {
        if (ownerCollider == null)
            return;
        
        Physics2D.IgnoreCollision(ownerCollider, myCollider);
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (LayerTools.IsLayerInMask(projectileHitMask, other.gameObject.layer) == false) {
            //Debug.LogWarning(LayerMask.LayerToName(other.gameObject.layer) + " is not in the hit mask");
            return;
        }

        ApplyImpact(other);
    }

    private void ApplyImpact(Collider2D other) {


        CreateImpactVFX(other.gameObject.transform.position);

        Entity target = other.gameObject.GetComponent<Entity>();

        if (target != null) {
            StatAdjustmentManager.ApplyStatAdjustment(
                target, 
                -projectileDamage, 
                StatName.Health, 
                StatModType.Flat, 
                StatModifierData.StatVariantTarget.RangeCurrent, 
                Source);
        }

        StartCleanUp();
    }


    private IEnumerator KillAfterLifetime() {

        float baseLifetime = Stats[StatName.ProjectileLifetime];
 
        WaitForSeconds waiter = new WaitForSeconds(baseLifetime);
        yield return waiter;

        StartCleanUp();
    }

    public void StartCleanUp() {

        myCollider.enabled = false;
        Movement.CanMove = false;
        Movement.MyBody.freezeRotation = false;
        Movement.MyBody.linearVelocity = Vector2.zero;

        SpawnDeathVFX();


        if (killTimer != null && killTimer.Running == true)
            killTimer.Stop();

        if (smoothScale != null && smoothScale.Running == true)
            smoothScale.Stop();

        Destroy(gameObject, 0.05f);


    }

    

    private void CreateImpactVFX(Vector2 location, bool variance = true) {
        if (impactVFX == null) {
            //Debug.LogWarning("a projectile: " + EntityName + " has no apply vfx");
            return;
        }

        VFXUtility.SpawnVFX(impactVFX, location, Quaternion.identity, null, 2f, 1f, variance);
    }

  

   

}
