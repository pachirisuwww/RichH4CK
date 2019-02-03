using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetMananger : NetworkManager
{
    public static NetMananger Instance;
    public QueueTextLog Logs;

    internal enum PeerType
    {
        None,
        Server,
        Client,
    }
    internal PeerType peerType;

    public ProcessManager ProcessMgr;
    public Option OptionPanel;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void DoStartServer()
    {
        if (peerType == PeerType.None)
            StartServer();
        else
            Logs.AddLog(string.Format("You are {0} already ", peerType));
    }

    public void DoStopServer()
    {
        if (peerType == PeerType.Server)
            StopServer();
        else
            Logs.AddLog("You are not Server");
    }

    public void DoStartClient()
    {
        if (peerType == PeerType.None)
            StartClient();
        else
            Logs.AddLog(string.Format("You are {0} already ", peerType));
    }
    
    public void OnChangeIP(string val)
    {
        networkAddress = val;
    }

    public void OnChangePort(string val)
    {
        int result;
        if (int.TryParse(val, out result))
            networkPort = result;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        peerType = PeerType.Client;

        Logs.AddLog("OnClientConnect");

        ProcessMgr.enabled = true;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        peerType = PeerType.None;

        Logs.AddLog("OnClientDisconnect");

        ProcessMgr.enabled = false;
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);

        peerType = PeerType.None;

        Logs.AddLog("OnClientError");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        peerType = PeerType.Server;

        Logs.AddLog("OnStartServer");

        SceneManager.LoadScene("MainUI", LoadSceneMode.Additive);

        OptionPanel.Display(false);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        peerType = PeerType.None;

        Logs.AddLog("OnStopServer");

        SceneManager.UnloadSceneAsync("MainUI");
    }
}
