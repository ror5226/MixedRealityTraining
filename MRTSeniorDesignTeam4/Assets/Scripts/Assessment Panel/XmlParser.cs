using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using UnityEngine;

public class XmlParser {

    XmlNodeList assetList;
    int numberOfAssets;
    TextAsset xmlRaw;
    XmlNode currentNode = null;
    AccessPanel a;

    public void setup() {
        XmlDocument document = new XmlDocument();
        xmlRaw = (TextAsset)Resources.Load("Assets");
        document.LoadXml(xmlRaw.text);
        document.PreserveWhitespace = false;
        assetList = document.GetElementsByTagName("asset");
        numberOfAssets = assetList.Count;
        if (AccessPanel.Instance == null) {
            Debug.Log("No AccessPanel Instance");
        }
        else {
            a = AccessPanel.Instance;
        }
        removeAllTabs();
    }

    public void setPanels(string s) {
        for (int i = 0; i < assetList.Count; i++) {
            if (assetList[i].Attributes[0].Value == s) {
                currentNode = assetList[i];
                break;
            }
        }
        setInfoPanel(s);
        setAssessmentPanel(s);

    }

    public void setInfoPanel(string s) {
        a.setInfoPanelVis(true);
        a.setTitle(s);
        a.setDesc(currentNode.ChildNodes[0].InnerText);
        //a.setMaterial(getAndReplace(currentNode.ChildNodes[1]));
        a.setImg(currentNode.ChildNodes[1].InnerText);
    }


    public void setAssessmentPanel(string s) {

        XmlNodeList ans = currentNode.ChildNodes[3].ChildNodes;

        a.setQuestion(currentNode.ChildNodes[2].InnerText);
        a.setAnsAVis(false);
        a.setAnsBVis(false);
        a.setAnsCVis(false);
        a.setAnsDVis(false);
        a.setAnsEVis(false);
        a.setAnsFVis(false);

        for (int i = 0; i < ans.Count; i++) {
            string tempAnswer = ans[i].ChildNodes[0].InnerText;
            string bo = ans[i].ChildNodes[1].InnerText;
            bool answerBool;

            if (bo.ToLower() == "true") {
                answerBool = true;
            }
            else if (bo.ToLower() == "false") {
                answerBool = false;
            }
            else {
                Debug.Log("some how not true or false.");
                answerBool = false;
            }
            setAnswer(i,tempAnswer, answerBool);
        }
    }

    private void setAnswer(int i, string s, bool b) {

        switch (i) {

            //  a
            case 0: {
                a.setAnsAText(s);
                a.setAnsABool(b);
                a.setAnsAVis(true);
                a.setAssessmentRows(1);
                break;
            }

            //  b
            case 1: {
                a.setAnsBText(s);
                a.setAnsBBool(b);
                a.setAnsBVis(true);
                break;
            }

            //  c
            case 2: {
                a.setAnsCText(s);
                a.setAnsCBool(b);
                a.setAssessmentRows(2);
                a.setAnsCVis(true);
                break;
            }

            //  d
            case 3: {
                a.setAnsDText(s);
                a.setAnsDBool(b);
                a.setAnsDVis(true);
                break;
            }

            //  e
            case 4: {
                a.setAnsEText(s);
                a.setAnsEBool(b);
                a.setAssessmentRows(3);
                a.setAnsEVis(true);
                break;
            }

            //f
            case 5: {
                a.setAnsFText(s);
                a.setAnsFBool(b);
                a.setAnsFVis(true);
                break;
            }
        }

    }

    private string removeTabs(string s) {
        return s.Replace("      ", "");
    }

    private void removeAllTabs() {
        for(int i=0; i< assetList.Count; i++) {
            XmlNode  n = assetList[i];
            n.ChildNodes[0].InnerText = removeTabs(n.ChildNodes[0].InnerText);
            n.ChildNodes[1].InnerText = removeTabs(n.ChildNodes[1].InnerText);
            n.ChildNodes[2].InnerText = removeTabs(n.ChildNodes[2].InnerText);
        }
    }

}
