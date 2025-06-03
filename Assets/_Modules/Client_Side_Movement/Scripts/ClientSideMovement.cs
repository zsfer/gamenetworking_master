using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class ClientSideMovement : NetworkBehaviour
{
    [SerializeField] float _moveSpeed = 5;

    CharacterController _cc;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!IsLocalPlayer) return;

        var mov = (Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical")).normalized * _moveSpeed;
        _cc.SimpleMove(mov);
    }
}
