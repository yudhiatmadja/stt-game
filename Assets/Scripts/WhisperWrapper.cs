using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WhisperWrapper : MonoBehaviour
{
    private const string DLL_NAME = "whisper";

    [DllImport(DLL_NAME, EntryPoint = "whisper_init")]
    public static extern IntPtr InitWhisper(string modelPath);

    [DllImport(DLL_NAME, EntryPoint = "whisper_transcribe")]
    public static extern IntPtr Transcribe(IntPtr ctx, float[] audioData, int length);

    [DllImport(DLL_NAME, EntryPoint = "whisper_free")]
    public static extern void FreeWhisper(IntPtr ctx);

    private IntPtr whisperContext;

    void Start()
    {
        string modelPath = System.IO.Path.Combine(Application.dataPath, "Plugins/x86_64/whisper.dll");
        if (!System.IO.File.Exists(modelPath))
    {
        Debug.LogError("whisper.dll not found at: " + modelPath);
    }
        whisperContext = InitWhisper(modelPath);
    }

    public string TranscribeAudio(float[] audioData)
    {
        IntPtr resultPtr = Transcribe(whisperContext, audioData, audioData.Length);
        return Marshal.PtrToStringAnsi(resultPtr);
    }

    void OnDestroy()
    {
        FreeWhisper(whisperContext);
    }
}
