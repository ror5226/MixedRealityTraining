using UnityEngine;
using System.Collections;

public class OpenSecondPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnSelect() {
        GameObject assessmentPanel = this.transform.parent.transform.parent.FindChild("AssessmentPanel").gameObject;
        GameObject damageCanvas = this.transform.parent.gameObject;

        //
        //  First set the assessmentPanel to be visable
        //  Then set the assessmentPanel's position to be where the damage info's position.
        //  then hide damageInfoPanel.
        //
        assessmentPanel.SetActive(true);
        assessmentPanel.transform.position = damageCanvas.transform.position;
        this.transform.parent.gameObject.SetActive(false);
    }
}
