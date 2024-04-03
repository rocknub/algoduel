using Character;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class MultiplayerManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    public Transform playersOriginsParent;
    public Transform[] playersOrigins;
    public int maxPlayers;

    private void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += SetPlayerTransform;

        SetPlayersOrigins();
    }

    private Transform[] SetPlayersOrigins()
    {
        maxPlayers = playerInputManager.maxPlayerCount < playersOriginsParent.childCount
            ? playerInputManager.maxPlayerCount
            : playersOriginsParent.childCount;
        playersOrigins = new Transform[maxPlayers];
        for (int i = 0; i < maxPlayers; i++)
        {
            playersOrigins[i] = playersOriginsParent.GetChild(i);
        }
        return playersOrigins;
    }

    public void SetPlayerTransform(PlayerInput playerInput)
    {
        if (playerInputManager.playerCount > maxPlayers)
            return;
        Player player = playerInput.transform.GetComponentInParent<Player>();
        player.gameObject.SetActive(true);
        player.transform.SetParent(playersOrigins[playerInputManager.playerCount-1], false);
        player.GoToOrigin();
        Debug.Log($"{player.name} is Player {playerInputManager.playerCount}");
    }
    
    
}
