using System;
using UnityEngine;

namespace MultiPlayer
{
    public static class MessageDestroy
    {
        [Serializable]
        public class DestroyMsgModel
        {
            public string owner;
            public string name;

            public DestroyMsgModel() { }
            public DestroyMsgModel(string owner, string name)
            {
                this.owner = owner;
                this.name = name;
            }
        }

        public static readonly string TYPE_NAME = "destroy";

        static MessageDestroy()
        {
            MessageModelMapper.map(TYPE_NAME, processMsg);
        }

        public static void processMsg(string args)
        {
            processMsg(JsonUtility.FromJson<DestroyMsgModel>(args));
        }

        public static void processMsg(DestroyMsgModel args)
        {
            UnityEngine.Object.Destroy(ObjectManager.getObject(args.owner, args.name).gameObject);
        }

        public static string serialize(string owner, string objName)
        {
            return serialize(new DestroyMsgModel(owner, objName));
        }

        public static string serialize(DestroyMsgModel destroy_msg)
        {
            return JsonUtility.ToJson(destroy_msg);
        }
    }
}