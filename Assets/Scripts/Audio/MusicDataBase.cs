using UnityEngine;

[CreateAssetMenu(fileName = "MusicDatabase", menuName = "Audio/Music Database")]
public class MusicDatabase : ScriptableObject
{
    [System.Serializable]
    public class MusicZoneClip
    {
        public MusicZoneType zoneType;
        public AudioClip clip;
    }

    public MusicZoneClip[] musicZones;

    public AudioClip GetClipForZone(MusicZoneType zoneType)
    {
        foreach (var zone in musicZones)
        {
            if (zone.zoneType == zoneType)
                return zone.clip;
        }
        return null;
    }
}