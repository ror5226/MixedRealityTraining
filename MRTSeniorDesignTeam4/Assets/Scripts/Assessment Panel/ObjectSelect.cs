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

        ////  getting the two end points for where the text should go.
        //Text damageText = infoPanel.transform.FindChild("DamgeInfoParagraph").GetComponent<Text>();
        //Text materialText = infoPanel.transform.FindChild("MaterialInfoParagraph").GetComponent<Text>();
        //Text title = infoPanel.transform.FindChild("ObjectTitle").GetComponent<Text>();
        
        
        ////  Setting the image based on file name.
        //Image img = infoPanel.transform.FindChild("ObjectImage").GetComponent<Image>();
        //Sprite s =(Sprite)Resources.Load<Sprite>(imgPath);
        //img.sprite = s;

        //title.text = objName;
        ////  get description of the object.
        //TextAsset txt = (TextAsset)Resources.Load(descPath, typeof(TextAsset));
        //string desc = txt.text;

        ////  get materialInfo of the object.
        //txt = (TextAsset)Resources.Load(matPath, typeof(TextAsset));
        //string matInfo = txt.text;

        ////  Change the two areas.
        //aPanel.setAudioClip(soundPath);
        //damageText.text = desc;
        //materialText.text = matInfo;



    /// <summary>
    ///     This is triggered by doing the air tap.
    ///     This method first gets the name of the object to pass to the XmlParser's method setPanels
    ///     Then use use the Access Panel, which is how use access the xmlparser's method, so that we can
    ///     set the Info Panel to be active as well as set the other panels that may be up to not active.
    ///     finally we reposition the info Panel.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputClickedEventData eventData){

        string selectedObjName = this.gameObject.name;
        string[] split = selectedObjName.Split('(');

        aPanel.setCorrectPanelVis(false);
        aPanel.setAssessmentPanelVis(false);

        aPanel.getXMLParser().setPanels(split[0]);
        aPanel.setInfoPanelVis(true);


        objPos = this.transform.position;
        //Vector3 camPos = GameObject.Find("Main Camera").transform.position;
        Vector3 camPos = GameObject.Find("HoloLensCamera").transform.position;

        aPanel.setInfoPanelPosition(this.transform.forward.x, camPos.y, this.transform.forward.z);

        // Rachel's fix
        /*
        Assessable assessable = this.GetComponent<Assessable>();
        if (assessable != null)
        {
            // position = surface.transform.position + ((plane.PlaneThickness + (.5f * Math.Abs(collider.size.z) * item.transform.localScale.z)) * plane.SurfaceNormal);
            Vector3 v = objPos + (.75f * assessable.getPlane().SurfaceNormal);
            aPanel.setInfoPanelPosition(v.x,camPos.y, v.z);
            //aPanel.getInfoPanel().transform.position += new Vector3(0, -.8f, 0);

        }
        else
        {
            Debug.Log("No panel");
            aPanel.getInfoPanel().transform.position = new Vector3(objPos.x, (objPos.y - .5f), (objPos.z + .8f));

        }*/
    }
}