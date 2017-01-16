using UnityEngine;
using System.Collections;

public class AssessmentExitButton : MonoBehaviour {

    void OnSelect() {
        GameObject damageInfo = this.transform.parent.transform.parent.FindChild("DamageInfo").gameObject;
        damageInfo.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
    }
}
