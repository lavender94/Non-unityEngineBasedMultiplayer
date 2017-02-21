using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPlayer
{
    public static class NetworkManager
    {
        private static UDPSender udpSender = null;
        private static UDPReceiver udpReceiver = null;

        public static void registerUdpSender(UDPSender sender)
        {
            udpSender = sender;
        }

        public static void registerUdpReceiver(UDPReceiver receiver)
        {
            udpReceiver = receiver;
        }

        public static void sendMessage(string msg)
        {
            if (udpSender != null)
                udpSender.sendMessage(msg);
        }
    }
}
