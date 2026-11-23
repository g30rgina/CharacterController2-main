using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _height = 100;

    void LateUpdate()
    {
        transform.position = new Vector3(_player.position.x, _player.position.y + _height, _player.position.z);
        transform.rotation = Quaternion.Euler(90, _player.eulerAngles.y, 0);
    }
}