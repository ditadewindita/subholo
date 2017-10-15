using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Academy.HoloToolKit.Unity;
using UnityEngine.Windows.Speech;
using System.Text;

public class TextDisplay : MonoBehaviour {
    public Text text;
    public int counter;

    private DictationRecognizer dictationRecognizer;
    private StringBuilder textSoFar;
    private static string deviceName;
    private int samplingRate;
    private const int messageLength = 10;

	// Use this for initialization
	void Start () {
        int unused;

        dictationRecognizer = new DictationRecognizer();
        Microphone.GetDeviceCaps(deviceName, out unused, out samplingRate);

        StartRecording();

        dictationRecognizer.DictationHypothesis += DictationHypothesis;
        dictationRecognizer.DictationResult += DictationResult;

        dictationRecognizer.DictationComplete += DictationComplete;
        dictationRecognizer.DictationError += DictationError;

        //counter = 0;
        //text.text = "Counter: " + counter.ToString();
	}
	
	// Update is called once per frame
	void Update () {

        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            SendMessage("RecordStop");

        //counter++;
        //text.text = "Counter: " + counter.ToString();
	}

    private void DictationResult(string text, ConfidenceLevel confidence)
    {
        textSoFar.Append(text + ". ");
        this.text.text = textSoFar.ToString();

    }

    private void DictationHypothesis(string text)
    {
        this.text.text = textSoFar.ToString() + " " + text + "...";
    }

    private void DictationComplete(DictationCompletionCause clause)
    {
        if (clause == DictationCompletionCause.TimeoutExceeded)
        {
            Microphone.End(deviceName);
            this.text.text = "BYE!";
            SendMessage("ResetAfterTimeout");
        }
    }

    private void DictationError(string error, int result)
    {
        this.text.text = error + "\nRESULT: " + result;
    }

    public AudioClip StartRecording()
    {
        PhraseRecognitionSystem.Shutdown();

        dictationRecognizer.Start();
        this.text.text = "START!";
        return Microphone.Start(deviceName, false, messageLength, samplingRate);
    }

    public void StopRecording()
    {
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            dictationRecognizer.Stop();

        Microphone.End(deviceName);
    }

}
