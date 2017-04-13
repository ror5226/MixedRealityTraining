using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour {

    AccessPanel aPanel;
    public bool correctAnswer;
    bool plzDontCheat = false;


	// Use this for initialization
	void Start () {
        if (AccessPanel.Instance == null) {
            Debug.Log("No AccessPanel Instance");
        }
        else {
            aPanel = AccessPanel.Instance;
        }
    }
	
    public void Answer_Button_Click() {
        aPanel.setCorrectPanelVis(true);
        aPanel.getCorrectPanel().transform.position = aPanel.getAssessmentPanel().transform.position;
        aPanel.setAssessmentPanelVis(false);

        if (correctAnswer) {
            aPanel.setCorrectPanelText("You have answered correct.\n\nGood Job!");
            aPanel.getCorrectPanel().transform.FindChild("TryAgainButton").gameObject.SetActive(false);

            //  Increment precent finished if we are still doing that here.
            if (!plzDontCheat) {
                aPanel.setScore(1, aPanel.getTitle());
            }
            ReadText.Instance.Say("You have answered correct.\n\nGood Job!");
        }
        else {
            aPanel.setCorrectPanelText("You have answered incorrectly.\n\nTry Again?");
            aPanel.getCorrectPanel().transform.FindChild("TryAgainButton").gameObject.SetActive(true);
            ReadText.Instance.Say("You have answered incorrectly.\n\nTry Again?");
        }
    }
}
