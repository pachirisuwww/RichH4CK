using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetAutoDestroy : MonoBehaviour
{
    void Update()
    {
        if (NetMananger.Instance && NetMananger.Instance.peerType == NetMananger.PeerType.None)
            Destroy(gameObject);
    }
}
