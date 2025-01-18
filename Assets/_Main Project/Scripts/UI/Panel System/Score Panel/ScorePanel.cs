using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ScorePanel : BasePanel
{
    [Header("Template")]
    public ScoreEntry template;
    public Transform scoreHolder;


    private Task scoreGenerationTask;
    private List<ScoreEntry> entries = new List<ScoreEntry>();

    protected override void Awake() {
        base.Awake();

        template.gameObject.SetActive(false);
    }

    public override void Open() {
        base.Open();

       scoreGenerationTask = new Task(PopulateScores());
    }


    private IEnumerator PopulateScores() {
        WaitForSeconds waiter = new WaitForSeconds(0.2f);
        
        List<ScoreManager.ScoreEntryData> scores = ScoreManager.GetScores().OrderByDescending(x => x.kills.Count()).ToList();

        entries.PopulateList(scores.Count, template, scoreHolder, false);

        for (int i = 0; i < entries.Count; i++) {
            entries[i].gameObject.SetActive(true);
            entries[i].Setup(scores[i], i + 1);

            yield return waiter;
        }
    }

    public void OnRestartClicked() {
        scoreGenerationTask.Stop();
        scoreGenerationTask = null;
        
        EntityManager.KillAllUnits();
        ScoreManager.Reset();
        EntityManager.SpawnWave();
        Close();
        
    }
}
