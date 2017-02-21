using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace MultiPlayer
{
    public class UDPSender : MonoBehaviour
    {
        public string IP = "127.0.0.1";
        public int port = 8051;
        
        private Queue<string> msgQueue;
        
        IPEndPoint remoteEndPoint;
        UdpClient client;
        
        public void Start()
        {
            init();
            NetworkManager.registerUdpSender(this);

            Debug.Log("UDP Sender start at " + IP + ":" + port.ToString());
        }

        public void init()
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
            client = new UdpClient();

            msgQueue = new Queue<string>();
        }
        
        void Update()
        {
            while (msgQueue.Count != 0)
                send(msgQueue.Dequeue());
        }

        void OnDestroy()
        {
            client.Close();
        }

        private void send(string message)
        {
            //Debug.Log("Send msg: " + message);
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.Send(data, data.Length, remoteEndPoint);
            }
            catch (Exception err)
            {
                //Debug.LogError(err.ToString());
            }
        }

        public void sendMessage(string msg)
        {
            msgQueue.Enqueue(msg);
        }
    }
}