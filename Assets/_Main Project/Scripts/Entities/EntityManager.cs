using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : Singleton<EntityManager> {

   

    [Header("Default Wave Variables")]
    public NPC unitTemplate;
    public int spawnCount = 10;

    [Header("Data")]
    public NameDatabase nameDatabase;
    public WeaponDatabse weapondatabase;

    public static Dictionary<Entity.EntityType, List<Entity>> ActiveEntities { get; private set; } = new Dictionary<Entity.EntityType, List<Entity>>();


    private static int idCounter = 0;

    private Task gameEndTask;


    public static void SpawnWave() {
        for (int i = 0; i < Instance.spawnCount; i++) {
            Instantiate(Instance.unitTemplate, TargetUtilities.GetViewportToWorldPoint(0.2f, 0.2f, 0.8f, 0.8f), Quaternion.identity);
        }
    }


    public static void RegisterEntity(Entity target) {
        if (ActiveEntities.ContainsKey(target.entityType) == true) {
            ActiveEntities[target.entityType].Add(target);
        }
        else {

            List<Entity> newEntityList = new List<Entity> { target };

            ActiveEntities.Add(target.entityType, newEntityList);
        }

        target.SessionID = idCounter;
        idCounter++;
        
    }


    public static void RemoveEntity(Entity target) {
        if (ActiveEntities.TryGetValue(target.entityType, out List<Entity> results) == true) {
            results.RemoveIfContains(target);


            if(Instance.gameEndTask == null || Instance.gameEndTask.Running == false) {
                Instance.gameEndTask = new Task(Instance.CheckForGameEnd());
            }

        }

    }

   
   private IEnumerator CheckForGameEnd() {
        WaitForSeconds waiter = new WaitForSeconds(1);

        yield return waiter;

        if (ActiveEntities.ContainsKey(Entity.EntityType.Unit) == false)
            yield break;

        if (ActiveEntities[Entity.EntityType.Unit].Count <= 1) {
            //Debug.Log("End of Game");

            if (ActiveEntities[Entity.EntityType.Unit].Count > 0) {
                string lastUnitStanding = ActiveEntities[Entity.EntityType.Unit][0].EntityName;
                //Debug.Log($"{lastUnitStanding} is the last man standing");
            }

            //Debug.Log(ScoreManager.GetTopScore().name + " had the most kills");

            PanelManager.OpenPanel<ScorePanel>();
        }
    }
   
    public static void KillAllUnits() {
        if (ActiveEntities.ContainsKey(Entity.EntityType.Unit) == false)
            return;

        for (int i = ActiveEntities[Entity.EntityType.Unit].Count -1; i >= 0; i--) {
            ActiveEntities[Entity.EntityType.Unit][i].Die(ActiveEntities[Entity.EntityType.Unit][i]);
        }
    }

}



