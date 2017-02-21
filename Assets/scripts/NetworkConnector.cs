using UnityEngine;

namespace MultiPlayer
{
    public class NetworkConnector : MonoBehaviour
    {
        void Start()
        {
            string connectMsg = MessageConnect.serialize(ObjectManager.identity);
            string msg = MessageModelMapper.serialize(MessageConnect.TYPE_NAME, connectMsg);
            NetworkManager.sendMessage(msg);
            ObjectManager.syncObject();
        }
    }
}