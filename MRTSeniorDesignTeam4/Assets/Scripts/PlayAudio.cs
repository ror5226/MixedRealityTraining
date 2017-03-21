using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {
    public TextToSpeechManager TextToSpeech;
   
    public void Play() {
        // Get the name
        var voiceName = System.Enum.GetName(typeof(TextToSpeechVoice), TextToSpeech.Voice);

        // Create message
        var msg = string.Format("This is the {0} voice. It should sound like it's coming from the object you clicked. Feel free to walk around and listen from different angles.", voiceName);

        // Speak message
        TextToSpeech.SpeakText(msg);
    }
	
	
}
