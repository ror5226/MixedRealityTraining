using UnityEngine;
using System.Collections;

public class CloseMappingMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    void OnSelect() {
        Destroy(this.gameObject);
    }
}
