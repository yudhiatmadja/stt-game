using System;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;

public class WhisperSTT : MonoBehaviour
{
    // Import fungsi dari whisper.dll
    [DllImport("whisper")]
    private static extern IntPtr whisper_init_from_file(string model_path);

    [DllImport("whisper")]
    private static extern void whisper_free(IntPtr ctx);

    private IntPtr whisperContext;

    void Start()
    {
        // Path ke model ggml-base.bin
        string modelPath = Path.Combine(Application.streamingAssetsPath, "ggml-base.bin");

        if (!File.Exists(modelPath))
        {
            Debug.LogError("Model file not found: " + modelPath);
            return;
        }
        else
        {
            Debug.Log("✅ Model file found: " + modelPath);
        }

        // Load model
        whisperContext = whisper_init_from_file(modelPath);
        if (whisperContext == IntPtr.Zero)
        {
            Debug.LogError("Failed to initialize Whisper model.");
            return;
        }

        Debug.Log("Whisper model loaded successfully!");
    }

    void OnDestroy()
    {
        // Hapus dari memory saat aplikasi ditutup
        if (whisperContext != IntPtr.Zero)
        {
            whisper_free(whisperContext);
        }
    }
}
