using UnityEngine;

public class MainMenu : BasePanel
{


    protected override void Update() {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Escape) && IsOpen == false) {
            Open();
        }
    }


    public void OnStartClicked() {
        EntityManager.KillAllUnits();
        ScoreManager.Reset();
        EntityManager.SpawnWave();
        Close();
    }

    public void OnExitClicked() {
        Application.Quit(); 
    }


}
