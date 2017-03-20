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
    XmlParser xml = new XmlParser();

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
        xml.setup();
    }

    public XmlParser getXMLParser() {
        return xml;
    }

    public void setupPanels(string s) {

        //
        //  set the title of the info panel.
        //
        damageInfo.transform.FindChild("ObjectTitle").GetComponent<Text>().text = s;
        xml.setInfoPanel(s);
        xml.setAssessmentPanel(s);

    }

    public void setDesc(string s) {
        GameObject g = damageInfo.transform.FindChild("InfoPanel").FindChild("DamageInfoParagraph").gameObject;
        g.GetComponent<Text>().text = s;
    }
    public void setMaterial(string s) {
        GameObject g = damageInfo.transform.FindChild("InfoPanel").FindChild("MaterialInfoParagraph").gameObject;
        g.GetComponent<Text>().text = s;
    }

    public void setQuestion(string s) {
        assessmentPanel.transform.FindChild("QuestionPanel").FindChild("QuestionText").GetComponent<Text>().text = s;
    }

    public void setQuestions(string s) {


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

    //
    //  Start of A
    //
    public GameObject getAnsA() {
        return ansA;
    }

    public void setAnsAText(string s) {
        ansA.transform.FindChild("AnswerText").GetComponent<Text>().text = s;
    }

    public void setAnsABool(bool b) {
        ansA.transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = b;
    }

    public void setAnsAVis(bool t) {
        ansA.SetActive(t);
    }


    //
    //  Start of B
    //
    public void setAnsBVis(bool t) {
        ansB.SetActive(t);
    }

    public GameObject getAnsB() {
        return ansB;
    }

    public void setAnsBText(string s) {
        ansB.transform.FindChild("AnswerText").GetComponent<Text>().text = s;
    }

    public void setAnsBBool(bool b) {
        ansB.transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = b;
    }


    //
    //  Start of C
    //
    public void setAnsCVis(bool t) {
        ansC.SetActive(t);
    }

    public GameObject getAnsC() {
        return ansC;
    }

    public void setAnsCBool(bool b) {
        ansC.transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = b;
    }

    public void setAnsCText(string s) {
        ansC.transform.FindChild("AnswerText").GetComponent<Text>().text = s;
    }


    //
    //  Start of D
    //
    public void setAnsDVis(bool t) {
        ansD.SetActive(t);
    }

    public GameObject getAnsD() {
        return ansD;
    }

    public void setAnsDBool(bool b) {
        ansD.transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = b;
    }

    public void setAnsDText(string s) {
        ansD.transform.FindChild("AnswerText").GetComponent<Text>().text = s;
    }


    //
    //  Start of E
    //

    public void setAnsEVis(bool t) {
        ansE.SetActive(t);
    }

    public GameObject getAnsE() {
        return ansE;
    }

    public void setAnsEBool(bool b) {
        ansE.transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = b;
    }

    public void setAnsEText(string s) {
        ansE.transform.FindChild("AnswerText").GetComponent<Text>().text = s;
    }


    //
    //  Start of F
    //
    public GameObject getAnsF() {
        return ansF;
    }

    public void setAnsFBool(bool b) {
        ansF.transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = b;
    }

    public void setAnsFText(string s) {
        ansF.transform.FindChild("AnswerText").GetComponent<Text>().text = s;
    }

    public void setAnsFVis(bool t) {
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
