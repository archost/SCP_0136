using UnityEngine;

public class AudioHandle
{
    public AudioSource Source { get; private set; }
    public AudioCategory Category { get; }
    public bool IsValid => Source != null;

    public static AudioHandle Invalid => new AudioHandle(null, AudioCategory.Oneshot);

    public AudioHandle(AudioSource source, AudioCategory category)
    {
        Source = source;
        Category = category;
    }

    public void Invalidate()
    {
        Source = null;
    }
}