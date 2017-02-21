using System;
using System.Collections.Generic;
using UnityEngine;

namespace MultiPlayer
{
    public static class ObjectManager
    {
        public static string identity = "local";

        private static Dictionary<string, Dictionary<string, NetworkObject>> objDict; // owner->name->obj, for remote only
        private static Dictionary<string, NetworkObject> localObjDict; // name->obj, for local objs only

        // Use this for initialization
        static ObjectManager()
        {
            objDict = new Dictionary<string, Dictionary<string, NetworkObject>>();
            localObjDict = new Dictionary<string, NetworkObject>();
        }

        public static bool registerObj(string owner, string name_short, NetworkObject networkObject)
        {
            //Debug.Log("register:" + owner + "|" + name_short);
            if (string.IsNullOrEmpty(owner)) // local
                try
                {
                    localObjDict.Add(name_short, networkObject);
                }
                catch (ArgumentException)
                {
                    Debug.LogError("Object named \"" + name_short + "\" has already existed.");
                    UnityEngine.Object.Destroy(networkObject.gameObject);
                    return false;
                }
            else
            {
                if (!objDict.ContainsKey(owner))
                    objDict.Add(owner, new Dictionary<string, NetworkObject>());
                try
                {
                    objDict[owner].Add(name_short, networkObject);
                }
                catch (ArgumentException)
                {
                    Debug.LogError("Owner \"" + owner + "\" already has an object named \"" + name_short + "\".");
                    UnityEngine.Object.Destroy(networkObject.gameObject);
                    return false;
                }
            }
            return true;
        }

        public static string genName(string owner, string name_short)
        {
            return name_short + "_" + owner;
        }

        public static void removeObj(string owner, string name)
        {
            if (string.IsNullOrEmpty(owner)) // local
                localObjDict.Remove(name);
            else
            {
                Dictionary<string, NetworkObject> objNameDict;
                if (objDict.TryGetValue(owner, out objNameDict))
                {
                    objNameDict.Remove(name);
                    if (objNameDict.Count == 0)
                        objDict.Remove(owner);
                }
            }
        }

        public static void removeObj(string owner) // remove all objs belonging to owner
        {
            Dictionary<string, NetworkObject> objNameDict;
            if (objDict.TryGetValue(owner, out objNameDict))
                foreach (KeyValuePair<string, NetworkObject> kv in objNameDict)
                    UnityEngine.Object.Destroy(kv.Value.gameObject);
        }

        public static void clear()
        {
            objDict.Clear();
            localObjDict.Clear();
        }

        public static NetworkObject getObject(string owner, string name)
        {
            if (string.IsNullOrEmpty(owner)) // local
            {
                NetworkObject networkObj;
                if (localObjDict.TryGetValue(name, out networkObj))
                    return networkObj;
            }
            else
            {
                Dictionary<string, NetworkObject> objNameDict;
                if (objDict.TryGetValue(owner, out objNameDict))
                {
                    NetworkObject networkObj;
                    if (objNameDict.TryGetValue(name, out networkObj))
                        return networkObj;
                }
            }
            return null;
        }

        public static void distributeAction(string owner, string objName, string action)
        {
            NetworkObject networkObj = getObject(owner, objName);
            if (networkObj != null)
                networkObj.distributeAction(action);
        }

        public static bool existObject(string owner, string name)
        {
            return getObject(owner, name) != null;
        }

        public static void syncObject() // synchronize local objects
        {
            //Debug.Log("Sync " + localObjDict.Count);
            foreach (KeyValuePair<string, NetworkObject> kv in localObjDict)
            {
                string objStateArgs = kv.Value.getStateArgs();
                string msgSpawn = MessageSpawn.serialize(identity, kv.Key, kv.Value.prefabClassName, objStateArgs);
                string msg = MessageModelMapper.serialize(MessageSpawn.TYPE_NAME, msgSpawn);
                // send msg
                //Debug.Log("Sync obj: " + msg);
                NetworkManager.sendMessage(msg);
            }
        }
    }
}