using UnityEngine;
using System.Collections;

public class OpenSecondMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnSelect()
    {
        GameObject menuPanel = this.transform.parent.transform.parent.FindChild("SecondMenu").gameObject;
        GameObject startCanvas = this.transform.parent.gameObject;

        //
        //  First set the assessmentPanel to be visable
        //  Then set the assessmentPanel's position to be where the damage info's position.
        //  then hide damageInfoPanel.
        //

        menuPanel.SetActive(true);
        menuPanel.transform.position = startCanvas.transform.position;
        this.transform.parent.gameObject.SetActive(false);
    }
}
