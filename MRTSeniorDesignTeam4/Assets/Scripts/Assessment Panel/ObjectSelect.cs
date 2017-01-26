using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ObjectSelect : MonoBehaviour {

    GameObject assessmentMenu;
    GameObject damageInfo;
    Vector3 objPos;

    // Use this for initialization
    void Start() {
        assessmentMenu = GameObject.Find("AssessmentPanel");
        damageInfo = GameObject.Find("DamageInfo");
        damageInfo.SetActive(false);
        assessmentMenu.SetActive(false);
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
        GameObject infoPanel = damageInfo.transform.FindChild("InfoPanel").gameObject;

        changeDescription(infoPanel, selectedObjName);
        changeAssessment(assessmentMenu, selectedObjName);

        objPos = this.transform.position;
        damageInfo.SetActive(true);
        damageInfo.transform.position = new Vector3(objPos.x, (objPos.y + (float).5), (objPos.z - (float).5));

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

        //  getting the two end points for where the text should go.
        Text damageText = infoPanel.transform.FindChild("DamgeInfoParagraph").GetComponent<Text>();
        Text materialText = infoPanel.transform.FindChild("MaterialInfoParagraph").GetComponent<Text>();

        //  get description of the object.
        TextAsset txt = (TextAsset)Resources.Load(descPath, typeof(TextAsset));
        string desc = txt.text;

        //  get materialInfo of the object.
        txt = (TextAsset)Resources.Load(matPath, typeof(TextAsset));
        string matInfo = txt.text;

        //  Change the two areas.
        damageText.text = desc;
        materialText.text = matInfo;
    }

    private void changeAssessment(GameObject aPanel, string objName) {

        //  Adds the proper file locations to the path to change the question and answers
        string questPath = "QuestionAndAnswers/" + objName +  "_quest";
        string ansPath = "QuestionAndAnswers/" + objName + "_ans";

        //  getting the two end points for where the text should go.
        Text assessmentText = aPanel.transform.FindChild("QuestionPanel").FindChild("QuestionText").GetComponent<Text>();
        GameObject answerPanel = aPanel.transform.FindChild("AnswerPanel").gameObject;

        TextAsset txt = (TextAsset)Resources.Load(questPath, typeof(TextAsset));
        string question = txt.text;

        assessmentText.text = question;

    }

}