using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour {

    AccessPanel aPanel;
    public bool correctAnswer;
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
        aPanel.setCorrectPanelActive(true);
        aPanel.getCorrectPanel().transform.position = aPanel.getAssessmentPanel().transform.position;
        aPanel.setAssessmentActive(false);

        if (correctAnswer) {
            aPanel.setCorrectPanelText("You have answered correct. Good Job!");
            aPanel.getCorrectPanel().transform.FindChild("TryAgainButton").gameObject.SetActive(false);
            //  Increment precent finished if we are still doing that here.
        }
        else {
            aPanel.setCorrectPanelText("You have answered incorrectly. Try Again?");
            aPanel.getCorrectPanel().transform.FindChild("TryAgainButton").gameObject.SetActive(true);
        }
    }
}
