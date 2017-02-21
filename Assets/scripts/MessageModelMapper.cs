using System;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPlayer
{
    public static class MessageModelMapper
    {
        [Serializable]
        public class MessageModel
        {
            public string type;
            public string args; // json format args of specified type
            
            public MessageModel() { }
            public MessageModel(string msgType, string args)
            {
                type = msgType;
                this.args = args;
            }
        }

        public delegate void msgHandler(string args);

        private static Dictionary<string, msgHandler> mapper;

        static MessageModelMapper()
        {
            mapper = new Dictionary<string, msgHandler>();
        }

        public static void map(string msgType, msgHandler handler)
        {
            try
            {
                mapper.Add(msgType, handler);
            }
            catch (ArgumentException)
            {
                Debug.LogError("Message type \"" + msgType + "\" has already been mapped.");
            }
        }

        public static void processMsg(MessageModel msg)
        {
            //Debug.Log("Process " + msg);
            msgHandler handler;
            if (mapper.TryGetValue(msg.type, out handler))
                handler(msg.args);
        }

        public static void processMsg(string msg_json)
        {
            processMsg(JsonUtility.FromJson<MessageModel>(msg_json));
        }

        public static string serialize(string msgType, string args)
        {
            return serialize(new MessageModel(msgType, args));
        }

        public static string serialize(MessageModel msg)
        {
            return JsonUtility.ToJson(msg);
        }
    }
}
