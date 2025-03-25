using UnityEngine;
using UnityEngine.UI;

public class MicrophoneRecorder : MonoBehaviour
{
    private AudioClip recordedClip;
    private int sampleRate = 16000;
    private bool isRecording = false;

    public WhisperWrapper whisperWrapper;
    public Button recordButton; // Tambahkan referensi ke tombol
    public Text buttonText;     // Tambahkan referensi ke teks tombol

    void Start()
    {
        if (whisperWrapper == null)
        {
            whisperWrapper = Object.FindFirstObjectByType<WhisperWrapper>();
        }

        // Pastikan tombol ada dan atur teks awalnya
        if (recordButton != null && buttonText != null)
        {
            recordButton.onClick.AddListener(ToggleRecording);
            buttonText.text = "Start Recording";
        }
    }

    public void ToggleRecording()
    {
        if (isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    public void StartRecording()
    {
        if (!isRecording)
        {
            recordedClip = Microphone.Start(null, false, 5, sampleRate);
            isRecording = true;
            Debug.Log("Recording started...");

            if (buttonText != null)
                buttonText.text = "Stop Recording";
        }
    }

    public void StopRecording()
    {
        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            Debug.Log("Recording stopped...");
            ProcessRecording();

            if (buttonText != null)
                buttonText.text = "Start Recording";
        }
    }

    private void ProcessRecording()
    {
        if (recordedClip != null)
        {
            float[] samples = new float[recordedClip.samples];
            recordedClip.GetData(samples, 0);
            string transcription = whisperWrapper.TranscribeAudio(samples);
            Debug.Log("Transcription: " + transcription);
        }
    }
}
