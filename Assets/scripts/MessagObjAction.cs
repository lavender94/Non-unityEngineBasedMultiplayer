using System;
using UnityEngine;

namespace MultiPlayer
{
    public static class MessageObjAction
    {
        [Serializable]
        public class ObjActionMsgModel
        {
            public string owner;
            public string name; // object name
            public string action;

            public ObjActionMsgModel() { }
            public ObjActionMsgModel(string owner, string name, string action)
            {
                this.owner = owner;
                this.name = name;
                this.action = action;
            }
        }

        public static readonly string TYPE_NAME = "action";

        static MessageObjAction()
        {
            MessageModelMapper.map(TYPE_NAME, processMsg);
        }

        public static void processMsg(string args)
        {
            processMsg(JsonUtility.FromJson<ObjActionMsgModel>(args));
        }

        public static void processMsg(ObjActionMsgModel args)
        {
            ObjectManager.distributeAction(args.owner, args.name, args.action);
        }

        public static string serialize(string owner, string objName, string action)
        {
            return serialize(new ObjActionMsgModel(owner, objName, action));
        }

        public static string serialize(ObjActionMsgModel move_msg)
        {
            return JsonUtility.ToJson(move_msg);
        }
    }
}