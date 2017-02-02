using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AccessPanel : HoloToolkit.Unity.Singleton<AccessPanel> {

    GameObject damageInfo;
    GameObject assessmentPanel;
    GameObject correctPanel;

    void Start() {
        assessmentPanel = GameObject.Find("AssessmentPanel");
        damageInfo = GameObject.Find("DamageInfo");
        correctPanel = GameObject.Find("CorrectPanel");
        damageInfo.SetActive(false);
        assessmentPanel.SetActive(false);
        correctPanel.SetActive(false);
    }

    public GameObject getAssessmentPanel() {
        return assessmentPanel;
    }

    public GameObject getDamageInfo() {
        return damageInfo;
    }

    public GameObject getCorrectPanel() {
        return correctPanel;
    }

    public void setCorrectPanelActive(bool vis) {
        correctPanel.SetActive(vis);
    }

    public void setDamageActive(bool vis) {
        damageInfo.SetActive(vis);
    }

    public void setAssessmentActive(bool vis) {
        assessmentPanel.SetActive(vis);
    }

    public void setCorrectPanelText(string s) {
        Text t = correctPanel.transform.FindChild("CorrectPanel").FindChild("CorrectText").GetComponent<Text>();
        t.text = s;
    }

}
