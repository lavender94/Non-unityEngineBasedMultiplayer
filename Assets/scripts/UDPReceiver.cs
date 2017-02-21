using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MultiPlayer
{
    public class UDPReceiver : MonoBehaviour
    {
        Thread receiveThread;
        
        UdpClient client;

        public int port = 8051;
        
        private Queue<string> _msgQueue;
        private Mutex mutex_msgQueue;
        private WaitHandle[] mutexList_msgQueue;
        
        public void Start()
        {
            init();
            NetworkManager.registerUdpReceiver(this);

            Debug.Log("UDP Receiver start at port " + port.ToString());
        }

        void Update()
        {
            processMsg();
        }

        void OnDestroy()
        {
            if (client != null)
                client.Close();
            if (receiveThread != null)
                if (receiveThread.IsAlive)
                {
                    receiveThread.Abort();
                    receiveThread.Join();
                }
        }

        private void init()
        {
            _msgQueue = new Queue<string>();
            mutex_msgQueue = new Mutex(false, "mutex_msgQueue");
            mutexList_msgQueue = new Mutex[] { mutex_msgQueue };

            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        
        private void ReceiveData()
        {
            client = new UdpClient(port);
            while (true)
            {
                try
                {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = client.Receive(ref anyIP);
                    string message = Encoding.UTF8.GetString(data);

                    WaitHandle.WaitAll(mutexList_msgQueue);
                    _msgQueue.Enqueue(message);
                    foreach (Mutex m in mutexList_msgQueue)
                        m.ReleaseMutex();
                    //Debug.Log("Receive msg: " + message);
                }
                catch (Exception err)
                {
                    //Debug.LogError(err.ToString());
                }
                Thread.Sleep(10);
            }
        }

        public void processMsg()
        {
            Queue<string> msgQueue = getAndClearMsgQueue();
            if (msgQueue != null)
            {
                while (msgQueue.Count != 0)
                    MessageModelMapper.processMsg(msgQueue.Dequeue());
            }
        }

        public Queue<string> getAndClearMsgQueue()
        {
            WaitHandle.WaitAll(mutexList_msgQueue);
            Queue<string> msgQueue = new Queue<string>(_msgQueue);
            _msgQueue.Clear();
            foreach (Mutex m in mutexList_msgQueue)
                m.ReleaseMutex();
            return msgQueue;
        }
    }
}