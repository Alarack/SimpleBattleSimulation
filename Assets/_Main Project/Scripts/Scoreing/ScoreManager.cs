using UnityEngine;
using System.Collections.Generic;
using LL.Events;
using System.Linq;

public class ScoreManager : Singleton<ScoreManager>
{

    private Dictionary<int, ScoreEntryData> scoreEntries = new Dictionary<int, ScoreEntryData>();



    private void OnEnable() {
        EventManager.RegisterListener(GameEvent.UnitDied, OnUnitDied);
    }

    private void OnDisable() {
        EventManager.RemoveMyListeners(this);
    }

    public static ScoreEntryData GetTopScore() {
       return Instance.scoreEntries.First().Value;
    }

    public static List<ScoreEntryData> GetScores() {
        return Instance.scoreEntries.Values.ToList();
    }

    public static void Reset() {
        Instance.scoreEntries.Clear();
    }



    private void OnUnitDied(EventData eventData) {

        Entity killer = eventData.GetEntity("Killer");
        Entity victim = eventData.GetEntity("Victim");


        UpdateScoreEntry(killer, victim);
    }

    private void UpdateScoreEntry(Entity killer, Entity victim) {
        if(killer == null) 
            return;

        if(scoreEntries.TryGetValue(killer.SessionID, out ScoreEntryData targetEntry) == true) {
            targetEntry.kills.Add(victim.EntityName);
        }
        else {
            scoreEntries.Add(killer.SessionID, 
                new ScoreEntryData(killer.EntityName, killer.SessionID, new List<string> { victim.EntityName }));
        }


        scoreEntries.OrderByDescending(x => x.Value.kills.Count);

    }


    [System.Serializable]
    public struct ScoreEntryData {
        public string name;
        public int id;
        public List<string> kills;

        public ScoreEntryData(string name, int id, List<string> kills) {
            this.name = name;
            this.id = id;
            this.kills = kills;
        }

        public static bool operator ==(ScoreEntryData a, ScoreEntryData b) {
            return a.id == b.id;
        }

        public static bool operator !=(ScoreEntryData a, ScoreEntryData b) {
            return a.id != b.id;
        }

        public override bool Equals(object o) {
            if(o == null || !(o is ScoreEntryData))
                return false;
            else 
                return ((ScoreEntryData)o).id == id;
        }

        public override int GetHashCode() {
            return 0;
        }
        
    }

}
