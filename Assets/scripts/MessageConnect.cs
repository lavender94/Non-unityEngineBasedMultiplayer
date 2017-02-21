using System;
using UnityEngine;

namespace MultiPlayer
{
    public static class MessageConnect
    {
        [Serializable]
        public class ConnectMsgModel
        {
            public string identity;
            
            public ConnectMsgModel() { }
            public ConnectMsgModel(string identidy)
            {
                this.identity = identity;
            }
        }

        public static readonly string TYPE_NAME = "connect";

        static MessageConnect()
        {
            MessageModelMapper.map(TYPE_NAME, processMsg);
        }

        public static void processMsg(string args)
        {
            processMsg(JsonUtility.FromJson<ConnectMsgModel>(args));
        }

        public static void processMsg(ConnectMsgModel args)
        {
            ObjectManager.syncObject();
        }

        public static string serialize(string identity)
        {
            return serialize(new ConnectMsgModel(identity));
        }

        public static string serialize(ConnectMsgModel connect_msg)
        {
            return JsonUtility.ToJson(connect_msg);
        }
    }
}