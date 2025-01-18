using UnityEngine;
using System.Collections.Generic;
using LL.Events;
using TMPro;

public class Entity : MonoBehaviour
{

    public enum SimpleEntityConditions {
        Dead,
        Invincible
    }

    public enum EntityType {
        Unit,
        Projectile,
    }



    [SerializeField]
    private string entityName;
    public string EntityName { get { return string.IsNullOrEmpty(entityName) == false ? entityName : gameObject.name; } }

    public TextMeshProUGUI nameText;

    public EntityType entityType;

    [Header("Stat Definitions")]
    public StatDataGroup statDefinitions;


    [Header("VFX")]
    public GameObject spawnEffectPrefab;
    public GameObject deathEffectPrefab;

    [Header("Sprite")]
    public SpriteRenderer mainSprite;

    [Header("Indicators")]
    public GameObject facingIndicator;

    public StatCollection Stats { get; protected set; }
    public EntityMovement Movement { get; private set; }

    public int SessionID { get; set; }

    public List<SimpleEntityConditions> Conditions { get; protected set; } = new List<SimpleEntityConditions>();

    public bool IsDead { get { return Conditions.Contains(SimpleEntityConditions.Dead); } }
    public bool IsInvincible { get { return Conditions.Contains (SimpleEntityConditions.Invincible); } }



    protected virtual void Awake() {
        Stats = new StatCollection(this, statDefinitions);

        Movement = GetComponent<EntityMovement>();

     

        if(entityType == EntityType.Unit) {
            GenerateName();
            RandomizeSpriteColor();
            EntityManager.RegisterEntity(this);
        }
            

    }

    protected virtual void Update() {

    }

    protected virtual void OnEnable() {
        RegisterStatListeners();
        SpawnEntranceEffect();
    }

    protected virtual void OnDisable() {
        EventManager.RemoveMyListeners(this);
    }

    protected virtual void GenerateName() {
        entityName = EntityManager.Instance.nameDatabase.GenerateName();
        
        if(nameText != null ) 
            nameText.text = entityName;
    }

    protected virtual void RandomizeSpriteColor() {
        mainSprite.color = VFXUtility.GenerateRandomColor();
    }

    public void AddCondition(SimpleEntityConditions condition) {
        Conditions.AddUnique(condition);
    }

    public void RemoveCondition(SimpleEntityConditions condition) {
        Conditions.RemoveIfContains(condition);
    }

    protected virtual void RegisterStatListeners() {
        EventManager.RegisterListener(GameEvent.UnitStatAdjusted, OnStatChanged);
    }


    protected virtual void OnStatChanged(EventData data) {
        StatName stat = (StatName)data.GetInt("Stat");

        Entity target = data.GetEntity("Target");

        if (target != this)
            return;

        if (stat != StatName.Health)
            return;

    
        Entity cause = data.GetEntity("Source");

        UpdateHealthBar();

        if (Stats[StatName.Health] <= 0) {
            Die(cause);
        }
    }

    private void UpdateHealthBar() {

    }

    public Transform GetOriginPoint() {
        return facingIndicator == null ? transform : facingIndicator.transform;
    }


    public virtual void Die(Entity source) {
        if (IsDead == true)
            return;

        AddCondition(SimpleEntityConditions.Dead);

        EventData data = new EventData();
        data.AddEntity("Victim", this);
        data.AddEntity("Killer", source);

        EventManager.SendEvent(GameEvent.UnitDied, data);
    }

    #region VFX

    protected void SpawnDeathVFX(float scale = 1f) {
        float desiredScale = 1f;

        if (scale != 1f) {
            desiredScale = scale;
        }

        VFXUtility.SpawnVFX(deathEffectPrefab, transform.position, Quaternion.identity, null, 2f, desiredScale);
    }

    protected void SpawnEntranceEffect(float scale = 1f) {
        float desiredScale = 1f;

        if (scale != 1f) {
            desiredScale = scale;
        }

        VFXUtility.SpawnVFX(spawnEffectPrefab, transform.position, Quaternion.identity, null, 2f, desiredScale);
    }


    #endregion
}
