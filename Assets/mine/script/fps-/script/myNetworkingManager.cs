using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

public class myNetworkingManager : NetworkManager
{
    public bool ShouldBeServer;
    static public int playerCount = 0;
    public GameObject vrPlayerPrefab;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        SpawnMessage message = new SpawnMessage();
        message.Deserialize(extraMessageReader);
        bool isVrPlayer = message.isVrPlayer;

        Transform spawnPoint = this.startPositions[playerCount];

        GameObject newPlayer;

        if (isVrPlayer)
        {
            newPlayer = (GameObject)Instantiate(this.vrPlayerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            newPlayer = (GameObject)Instantiate(this.playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        NetworkServer.AddPlayerForConnection(conn, newPlayer, playerControllerId);

        playerCount++;
    }

    private void Start()
    {
        var settingsPath = Application.dataPath + "/settings.cfg";
        if (File.Exists(settingsPath))
        {
            StreamReader textReader = new StreamReader(settingsPath, System.Text.Encoding.ASCII);
            ShouldBeServer = textReader.ReadLine() == "Server";
            networkAddress = textReader.ReadLine();
            textReader.Close();
        }

        Debug.Log("Starting Network");
        if (ShouldBeServer)
        {
            StartHost();
        }
        else
        {
            StartClient();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void s()
    {
    }
}