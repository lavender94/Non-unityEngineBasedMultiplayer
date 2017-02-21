using System;
using UnityEngine;

namespace MultiPlayer
{
    public static class MessageSpawn
    {
        [Serializable]
        public class SpawnMsgModel
        {
            public string owner;
            public string name;
            public string prefabClassName;
            public string parentName; // can be reached by transform.parent.gameObject.name, avoid to use this if unnecessary
            public string objStateArgs;

            public SpawnMsgModel() { }
            public SpawnMsgModel(string owner, string name, string prefabClassName, string objStateArgs)
            {
                this.owner = owner;
                this.name = name;
                this.prefabClassName = prefabClassName;
                this.objStateArgs = objStateArgs;
                parentName = null;
            }
            public SpawnMsgModel(string owner, string name, string prefabClassName, string parentName, string objStateArgs)
            {
                this.owner = owner;
                this.name = name;
                this.prefabClassName = prefabClassName;
                this.parentName = parentName;
                this.objStateArgs = objStateArgs;
            }
        }

        public static readonly string TYPE_NAME = "spawn";

        static MessageSpawn()
        {
            MessageModelMapper.map(TYPE_NAME, processMsg);
        }

        public static void processMsg(string args)
        {
            processMsg(JsonUtility.FromJson<SpawnMsgModel>(args));
        }

        public static void processMsg(SpawnMsgModel args)
        {
            //Debug.Log("Spawn: " + args);
            if (ObjectManager.existObject(args.owner, args.name))
            {
                Debug.LogWarning("Object \"" + args.name + "\" for user \"" + args.owner + "\" existed");
                return;
            }
            UnityEngine.Object original = Resources.Load(args.prefabClassName, typeof(GameObject));
            if (original != null)
            {
                GameObject parent = null;
                if (!string.IsNullOrEmpty(args.parentName))
                    parent = GameObject.Find(args.parentName);
                GameObject obj;
                if (parent != null)
                    obj = UnityEngine.Object.Instantiate(original, parent.transform) as GameObject;
                else
                    obj = UnityEngine.Object.Instantiate(original) as GameObject;
                obj.name = ObjectManager.genName(args.owner, args.name);
                NetworkObject nobj = obj.GetComponent<NetworkObject>();
                nobj.owner = args.owner;
                nobj.name_short = args.name;
                nobj.prefabClassName = args.prefabClassName;
                nobj.initState(args.objStateArgs);
            }
            else
                Debug.LogError("Prefab class name \"" + args.prefabClassName + "\" cannot be found");
        }

        public static string serialize(string owner, string objName, string prefabClassName, string objStateArgs)
        {
            return serialize(new SpawnMsgModel(owner, objName, prefabClassName, objStateArgs));
        }

        public static string serialize(string owner, string objName, string prefabClassName, string parentName, string objStateArgs)
        {
            return serialize(new SpawnMsgModel(owner, objName, prefabClassName, parentName, objStateArgs));
        }

        public static string serialize(SpawnMsgModel spawn_msg)
        {
            return JsonUtility.ToJson(spawn_msg);
        }
    }
}