using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [SerializeField] private MusicZoneType _zoneType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.EnterMusicZone(_zoneType);
        }
    }
}