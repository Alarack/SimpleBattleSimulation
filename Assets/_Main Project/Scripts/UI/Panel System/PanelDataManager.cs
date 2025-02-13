using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDataManager : Singleton<PanelDataManager>
{
    public PanelMapData panelMapData;
    public Transform canvasRoot;


    public static List<string> closeOnEscapePanels = new List<string>();


    private void Start() {
        for (int i = 0; i < panelMapData.panelPrefabs.Count; i++) {
            if (panelMapData.panelPrefabs[i].autoOpen == true) {
                PanelManager.OpenPanel(panelMapData.panelPrefabs[i].panelID);
            }
        }


        closeOnEscapePanels = panelMapData.GetEscapeClosingPanels();
    }

    private void Update() {

      

    }


    private bool ClosePanelsOnEscape() {
        bool closed = false;




        return closed;

    }

}
