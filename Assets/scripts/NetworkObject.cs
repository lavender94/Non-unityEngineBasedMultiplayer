using UnityEngine;

namespace MultiPlayer
{
    public abstract class NetworkObject : MonoBehaviour
    {
        public string prefabClassName = null;
        // use ObjectManager.genName(owner, name_short) to generate gameobject name
        public string owner = null;
        public string name_short = null; 

        // When inherited, always call the Start() from baseclass first, using base.Start()
        public virtual void Start()
        {
            if (string.IsNullOrEmpty(name_short))
                name_short = gameObject.name;
            ObjectManager.registerObj(owner, name_short, this);
        }

        public abstract void distributeAction(string action);
        public abstract void initState(string stateArgs);
        public virtual string getStateArgs()
        {
            return null;
        }

        public virtual void onDestory()
        {
            ObjectManager.removeObj(owner, name_short);
        }
    }
}