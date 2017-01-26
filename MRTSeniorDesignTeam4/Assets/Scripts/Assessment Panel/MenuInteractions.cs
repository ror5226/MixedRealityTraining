using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInteractions : MonoBehaviour {

    GameObject assessmentMenu;
    GameObject damageInfo;

    // Use this for initialization
    void Start () {
        assessmentMenu = GameObject.Find("AssessmentPanel");
        damageInfo = GameObject.Find("DamageInfo");
    }

    public void damageInfo_OpenAssessment() {
        assessmentMenu.transform.position = damageInfo.transform.position;
        damageInfo.SetActive(false);
        assessmentMenu.SetActive(true);
    }

    public void AssessmentMenu_DamageOpen() {
        damageInfo.transform.position = assessmentMenu.transform.position;
        assessmentMenu.SetActive(false);
        damageInfo.SetActive(true);
    }

    public void closeMenus() {
        if(assessmentMenu.activeSelf == true)
            assessmentMenu.SetActive(false);
        else if(damageInfo.activeSelf == true)
            damageInfo.SetActive(false);
    }
}
