using UnityEngine;

public class BackRoomsTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform tpPoint;
    [SerializeField] private GameObject backRooms;
    [SerializeField] private GameObject directionalLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            if (backRooms != null) backRooms.SetActive(true);

            if (player != null && tpPoint != null)
            {
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                    controller.enabled = false;

                player.transform.position = tpPoint.position;

                if (controller != null)
                    controller.enabled = true;

                directionalLight.SetActive(false);
            }
        }
    }
}