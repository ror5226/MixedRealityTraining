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
        aPanel.setAssessmentActive(true);
        aPanel.getAssessmentPanel().transform.position = aPanel.getDamageInfo().transform.position;
        aPanel.setDamageActive(false);
        aPanel.setCorrectPanelActive(false);
    }

    public void closeMenus() {
        aPanel.setAssessmentActive(false);
        aPanel.setDamageActive(false);
        aPanel.setCorrectPanelActive(false);
    }
}
