using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInteractions : MonoBehaviour {

    AccessPanel aPanel;

    // Use this for initialization
    void Start () {
        if (AccessPanel.Instance == null) {
            Debug.Log("No AccessPanel Instance");
        }
        else {
            aPanel = AccessPanel.Instance;
        }
    }

    public void damageInfo_OpenAssessment() {
        aPanel.setAssessmentPanelVis(true);
        aPanel.getAssessmentPanel().transform.position = aPanel.getInfoPanel().transform.position;
        aPanel.setInfoPanelVis(false);
        aPanel.setCorrectPanelVis(false);
    }

    public void closeMenus() {
        aPanel.setAssessmentPanelVis(false);
        aPanel.setInfoPanelVis(false);
        aPanel.setCorrectPanelVis(false);
    }
}
