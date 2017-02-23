using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AccessPanel : HoloToolkit.Unity.Singleton<AccessPanel> {

    GameObject damageInfo;
    GameObject assessmentPanel;
    GameObject correctPanel;

    GameObject answerPanel;
    BoxCollider assessmentCollider;
    GameObject ansA;
    GameObject ansB;
    GameObject ansC;
    GameObject ansD;
    GameObject ansE;
    GameObject ansF;
    new AudioSource audio;

    void Start() {
        assessmentPanel = GameObject.Find("AssessmentPanel");
        damageInfo = GameObject.Find("DamageInfo");
        correctPanel = GameObject.Find("CorrectPanel");
        assessmentCollider = assessmentPanel.transform.GetComponent<BoxCollider>();
        answerPanel = assessmentPanel.transform.FindChild("AnswerPanel").gameObject;
        ansA = answerPanel.transform.FindChild("AnswerA").gameObject;
        ansB = answerPanel.transform.FindChild("AnswerB").gameObject;
        ansC = answerPanel.transform.FindChild("AnswerC").gameObject;
        ansD = answerPanel.transform.FindChild("AnswerD").gameObject;
        ansE = answerPanel.transform.FindChild("AnswerE").gameObject;
        ansF = answerPanel.transform.FindChild("AnswerF").gameObject;
        audio = damageInfo.transform.GetComponent<AudioSource>();
        ansC.SetActive(false);
        ansD.SetActive(false);
        ansF.SetActive(false);
        ansE.SetActive(false);
        damageInfo.SetActive(false);
        assessmentPanel.SetActive(false);
        correctPanel.SetActive(false);
    }

    public void setAudioClip(string s) {
        audio.clip = (AudioClip)Resources.Load(s);
    }

    public void playAudioClip() {
        if (audio.time <= 2)
            audio.Play();
        else
            audio.UnPause();
    } 

    public bool isAudioPlaying() {
        return audio.isPlaying;
    }

    public void pauseAudio() {
        audio.Pause();
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
        Text t = correctPanel.transform.FindChild("CorrectPanelText").FindChild("CorrectText").GetComponent<Text>();
        t.text = s;
    }

    public GameObject getAnsA() {
        return ansA;
    }
    public GameObject getAnsB() {
        return ansB;
    }
    public GameObject getAnsC() {
        return ansC;
    }
    public GameObject getAnsD() {
        return ansD;
    }
    public GameObject getAnsE() {
        return ansE;
    }
    public GameObject getAnsF() {
        return ansF;
    }

    public void setAnsA(bool t) {
        ansA.SetActive(t);
    }
    public void setAnsB(bool t) {
        ansB.SetActive(t);
    }
    public void setAnsC(bool t) {
        ansC.SetActive(t);
    }
    public void setAnsD(bool t) {
        ansD.SetActive(t);
    }
    public void setAnsE(bool t) {
        ansE.SetActive(t);
    }
    public void setAnsF(bool t) {
        ansF.SetActive(t);
    }

    public void setAssessmentRows(int rows) {

        if (rows == 1) {
            assessmentCollider.size = new Vector3(assessmentCollider.size.x, 315);
            assessmentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 315);
            assessmentPanel.transform.FindChild("QuestionPanel").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 315);
            answerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(570, 100);


        }

        else if (rows == 2) {
            assessmentCollider.size = new Vector3(assessmentCollider.size.x, 395);
            assessmentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 395);
            assessmentPanel.transform.FindChild("QuestionPanel").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 395);
            answerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(570, 180);
        }
        else if(rows == 3) {
            assessmentCollider.size = new Vector3(assessmentCollider.size.x, 475);
            assessmentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 475);
            assessmentPanel.transform.FindChild("QuestionPanel").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 475);
            answerPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(570, 260);
        }

    }
}
