import io
import numpy as np
import soundfile as sf
import whisper
import librosa

# Load Whisper model once
model = whisper.load_model("small")

def process_audio(wav_bytes):
    audio_stream = io.BytesIO(wav_bytes)
    audio, sr = sf.read(audio_stream, dtype='int16')

    # Convert ke float32, 16000 Hz, mono
    audio = audio.astype(np.float32) / 32768.0
    if len(audio.shape) > 1:
        audio = librosa.to_mono(audio.T)
    audio = librosa.resample(audio, orig_sr=sr, target_sr=16000)

    # Transcribe pakai Whisper
    result = model.transcribe(audio, language='id')
    return result["text"]
