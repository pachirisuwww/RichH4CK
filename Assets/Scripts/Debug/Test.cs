using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyMemory;
using System.Net;
using System;
using System.Net.NetworkInformation;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
        Console.WriteLine(hostName);
        // Get the IP  
        string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
        Debug.Log("My IP Address is :" + myIP);


        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        //do what you want with the IP here... add it to a list, just get the first and break out. Whatever.
                        Debug.Log(ip.Address.ToString());
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
