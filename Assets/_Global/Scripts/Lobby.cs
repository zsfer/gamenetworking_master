using UnityEngine;
using Unity.Netcode;

public class Lobby : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject _mainMenuUI;

    public void StartServer()
    {
        NetworkManager.Singleton.StartHost();
        _mainMenuUI.SetActive(false);
    }

    public void JoinClient()
    {
        NetworkManager.Singleton.StartClient();
        _mainMenuUI.SetActive(false);
    }
}
