using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using HoloToolkit.Unity.InputModule;
using System;

public class ObjectSelect : MonoBehaviour, IInputClickHandler {

    AccessPanel aPanel;
    Vector3 objPos;

    // Use this for initialization
    void Start() {
        if (AccessPanel.Instance == null) {
            Debug.Log("No AccessPanel Instance");
        }
        else {
            aPanel = AccessPanel.Instance;
        }
    }


    /// <summary>
    /// The filePathBase is sent as a general path which is then specified in the functions that populate
    /// the actual information inside of the menues. The file path for each of the different texts in the 
    /// menues are seperated as follows:
    /// 
    /// DamageInfo Panel:
    ///     if you want the description this will be in "Resources/Description/" folder named  "SomeAssetName_desc"
    ///     if you want the materialInfo it would be in "Resources/Description/" folder named  "SomeAssetName_mat"
    ///     
    /// Assessment Panel:
    ///     if you want the question it will be in "Resources/QuestionAndAnswer/" folder and named "SomeAsset_quest"
    ///     if you want the answers it will be in  "Resources/QuestionAndAnswer/" folder and named "SomeAsset_ans"
    /// </summary>
    public void OnSelect() {

        //  get the name of the selected object
        string selectedObjName = this.gameObject.name;
        string[] split = selectedObjName.Split('(');

        aPanel.getXMLParser().setPanels(split[0]);

        //changeDescription(aPanel.getDamageInfo().transform.FindChild("InfoPanel").gameObject, split[0]);
        //changeAssessment(aPanel.getAssessmentPanel(), split[0]);

        objPos = this.transform.position;
        aPanel.setDamageActive(true);

        //
        //  to be optimized later
        //
        GameObject cam = GameObject.Find("Main Camera");
        Vector3 camPos = cam.transform.position;

        // Rachel's fix
        
        Assessable assessable = this.GetComponent<Assessable>();
        if (assessable != null)
        {
           // position = surface.transform.position + ((plane.PlaneThickness + (.5f * Math.Abs(collider.size.z) * item.transform.localScale.z)) * plane.SurfaceNormal);

            aPanel.getDamageInfo().transform.position = objPos + (.75f * assessable.getPlane().SurfaceNormal);
            aPanel.getDamageInfo().transform.position += new Vector3(0, -.8f, 0);

        }
        else
        {
            Debug.Log("No panel");
            aPanel.getDamageInfo().transform.position = new Vector3(objPos.x, (objPos.y - .5f), (objPos.z + .8f));

        }
        

    }

    /// <summary>
    /// This function is used to update the DamageInfo Panel, this requires the object to have a description
    /// and material description. It then generates the filepath based on the object's name that is passed to
    /// find the correct text file for the description and material information.
    /// 
    /// Once we have the access to the correct file path we need to target the correct GUIText fields on the menu.
    /// We can then read from the txt file and get the text for the respective areas. 
    /// Finally you set the GUIText.text to the proper text that is pulled from the file. 
    /// </summary>
    /// <param name="objMenu"> gameobject of the container of the two menues </param>
    /// <param name="objName"> name of the object selected </param>
    /// <param name="fp"> File path </param>
    private void changeDescription(GameObject infoPanel, string objName) {

        //  Adds the proper file locations to the path to change the description and material info
        string descPath = "Description/" + objName + "_desc";
        string matPath = "Description/" + objName + "_mat";
        string soundPath = "Description/" + objName + "_audio";
        string imgPath = "Description/" + objName + "_img";

        //  getting the two end points for where the text should go.
        Text damageText = infoPanel.transform.FindChild("DamgeInfoParagraph").GetComponent<Text>();
        Text materialText = infoPanel.transform.FindChild("MaterialInfoParagraph").GetComponent<Text>();
        Text title = infoPanel.transform.FindChild("ObjectTitle").GetComponent<Text>();
        
        
        //  Setting the image based on file name.
        Image img = infoPanel.transform.FindChild("ObjectImage").GetComponent<Image>();
        Sprite s =(Sprite)Resources.Load<Sprite>(imgPath);
        img.sprite = s;

        title.text = objName;
        //  get description of the object.
        TextAsset txt = (TextAsset)Resources.Load(descPath, typeof(TextAsset));
        string desc = txt.text;

        //  get materialInfo of the object.
        txt = (TextAsset)Resources.Load(matPath, typeof(TextAsset));
        string matInfo = txt.text;

        //  Change the two areas.
        aPanel.setAudioClip(soundPath);
        damageText.text = desc;
        materialText.text = matInfo;
    }

   

    private void changeAssessment(GameObject gObj, string objName) {

        //  Adds the proper file locations to the path to change the question and answers
        string questPath = "QuestionAndAnswers/" + objName +  "_quest";
        string ansPath = "QuestionAndAnswers/" + objName + "_ans";

        //  getting the two end points for where the text should go.
        GameObject assessmentPanel = aPanel.getAssessmentPanel();
        Text assessmentText = gObj.transform.FindChild("QuestionPanel").FindChild("QuestionText").GetComponent<Text>();
        TextAsset txt = (TextAsset)Resources.Load(questPath, typeof(TextAsset));
        
        assessmentText.text = txt.text;

        txt = (TextAsset)Resources.Load(ansPath, typeof(TextAsset));
        string tempText = txt.text;
        tempText = tempText.Replace("\r\n", "");
        string[] answers = tempText.Split('#');

        aPanel.setAnsAVis(false);
        aPanel.setAnsBVis(false);
        aPanel.setAnsCVis(false);
        aPanel.setAnsDVis(false);
        aPanel.setAnsEVis(false);
        aPanel.setAnsFVis(false);

        aPanel.setAssessmentActive(true);

        for (int i=0; i < answers.Length-1; i++) {
            switch (i) {

                //
                //  from 0-3 we only have one row of answers
                //
                case 0:
                    aPanel.getAnsA().transform.FindChild("AnswerText").GetComponent<Text>().text = answers[i];
                    aPanel.setAnsAVis(true);
                break;

                case 1:
                if (answers[i] == "T")
                    aPanel.getAnsA().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = true;
                else
                   aPanel.getAnsA().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = false;
                break;

                case 2:
                    aPanel.getAnsB().transform.FindChild("AnswerText").GetComponent<Text>().text = answers[i];
                    aPanel.setAnsBVis(true);
                break;

                case 3:
                    if (answers[i] == "T")
                        aPanel.getAnsB().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = true;
                    else
                        aPanel.getAnsB().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = false;
                break;
                
                //
                //  from 4-7 we have 2 rows of answers
                //
                case 4:
                    aPanel.getAnsC().transform.FindChild("AnswerText").GetComponent<Text>().text = answers[i];
                    aPanel.setAnsCVis(true);
                break;

                case 5:
                    if (answers[i] == "T")
                        aPanel.getAnsC().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = true;
                    else
                        aPanel.getAnsC().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = false;
                break;

                case 6:
                    aPanel.getAnsD().transform.FindChild("AnswerText").GetComponent<Text>().text = answers[i];
                    aPanel.setAnsDVis(true);
                break;

                case 7:
                    if (answers[i] == "T")
                        aPanel.getAnsD().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = true;
                    else
                        aPanel.getAnsD().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = false;
                break;

                //
                //  from 8-11 we have 3 rows of answers
                // 
                case 8:
                    aPanel.getAnsE().transform.FindChild("AnswerText").GetComponent<Text>().text = answers[i];
                    aPanel.setAnsEVis(true);
                break;
                    
                case 9:
                    if (answers[i] == "T")
                        aPanel.getAnsE().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = true;
                    else
                        aPanel.getAnsE().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = false;
                break;

                case 10:
                    aPanel.getAnsF().transform.FindChild("AnswerText").GetComponent<Text>().text = answers[i];
                    aPanel.setAnsFVis(true);
                break;

                case 11:
                    if (answers[i] == "T")
                        aPanel.getAnsF().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = true;
                    else
                        aPanel.getAnsF().transform.FindChild("AnswerButton").GetComponent<AnswerScript>().correctAnswer = false;
                break;

                default:
                break;

            }
        }

        if ( answers.Length-1 == 4) {
            aPanel.setAssessmentRows(1);
        }
        else if (answers.Length-1 == 6) {
            aPanel.setAssessmentRows(2);
        }
        else if (answers.Length-1 == 12) {
            aPanel.setAssessmentRows(3);
        }

        aPanel.setAssessmentActive(false);
    }

    public void OnInputClicked(InputClickedEventData eventData) {
        OnSelect();
    }
}