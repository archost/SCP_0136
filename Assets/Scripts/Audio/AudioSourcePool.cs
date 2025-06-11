using UnityEngine;
using System.Collections.Generic;

public class AudioSourcePool
{
    private Queue<AudioSource> _available = new Queue<AudioSource>();
    private List<AudioSource> _active = new List<AudioSource>();
    private GameObject _parent;

    public IEnumerable<AudioSource> ActiveSources => _active;

    public AudioSourcePool(GameObject parent, int initialSize)
    {
        _parent = parent;
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewSource();
        }
    }

    public AudioSource Get()
    {
        if (_available.Count == 0)
        {
            CreateNewSource();
        }

        var source = _available.Dequeue();
        _active.Add(source);
        return source;
    }

    public void Return(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        _active.Remove(source);
        _available.Enqueue(source);
    }

    public bool Contains(AudioSource source) => _available.Contains(source);

    private void CreateNewSource()
    {
        var go = new GameObject("AudioSource");
        go.transform.SetParent(_parent.transform);
        var source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        _available.Enqueue(source);
    }
}
