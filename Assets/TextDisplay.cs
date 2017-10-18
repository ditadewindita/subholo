using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Academy.HoloToolKit.Unity;
using UnityEngine.Windows.Speech;
using System.Text;


public class TextDisplay : MonoBehaviour {
    public Text text;
    private DictationRecognizer dictationRecognizer;
    private string deviceName;
    private int samplingRate = 100;
    private const int messageLength = 20;

    // Use this for initialization

    void Start() {
        int unused;
        Debug.Log("START LOG");

        PhraseRecognitionSystem.Shutdown();

        dictationRecognizer = new DictationRecognizer();
        Microphone.GetDeviceCaps(deviceName, out unused, out samplingRate);

        dictationRecognizer.DictationHypothesis += DictationHypothesis;
        dictationRecognizer.DictationResult += DictationResult;

        dictationRecognizer.DictationComplete += DictationComplete;
        dictationRecognizer.DictationError += DictationError;

        StartRecording();

    }

    // Update is called once per frame
    void Update() {


        //if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        //    SendMessage("RecordStop");

        //counter++;
        //text.text = "Counter: " + counter.ToString();
    }

    private void DictationResult(string text, ConfidenceLevel confidence)
    {
        this.text.text = (new StringBuilder(text)).Append(".").ToString();
        Debug.LogFormat("Dictation result: {0}", text);
    }

    private void DictationHypothesis(string text)
    {
        this.text.text = (new StringBuilder(text)).Append("...").ToString();
        Debug.LogFormat("Dictation hypothesis: {0}", text);

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

    public void StartRecording()
    {

        Debug.Log("START RECORDING DEBUG");
        dictationRecognizer.Start();
        this.text.text = "START!";
        //return Microphone.Start(deviceName, true, messageLength, samplingRate);
    }

    public void StopRecording()
    {

        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            dictationRecognizer.Stop();
        dictationRecognizer.Dispose();
        //Microphone.End(deviceName);
    }


}
