using UnityEngine;
using MultiPlayer;

public class SimpleObjectMonitor : ObjectMonitor {
    NetworkObject obj;

    Vector3 _position;
    Quaternion _rotation;

    // Use this for initialization
    void Start () {
        obj = GetComponent<NetworkObject>();

        _position = transform.position;
        _rotation = transform.rotation;

        /*// test use
        string spawnMsg = MessageSpawn.serialize(ObjectManager.identity, gameObject.name + "_ghost", "partical_system",
            JsonUtility.ToJson(new SimpleNetworkObject.StateArgs(transform.position, transform.rotation)) );
        string msg = MessageModelMapper.serialize(MessageSpawn.TYPE_NAME, spawnMsg);
        MessageModelMapper.processMsg(msg);
        */
	}

    protected override void trackState()
    {
        if (transform.position != _position || transform.rotation != _rotation)
        {
            _position = transform.position;
            _rotation = transform.rotation;
            SimpleNetworkObject.ActionMoveArgs move_args = new SimpleNetworkObject.ActionMoveArgs(transform.position, transform.rotation);
            move_args.p_y += 0.5f;
            SimpleNetworkObject.ActionPacket act_p = new SimpleNetworkObject.ActionPacket("move", JsonUtility.ToJson(move_args));
            string actionMsg = MessageObjAction.serialize(string.IsNullOrEmpty(obj.owner) ? ObjectManager.identity : obj.owner, obj.name_short, JsonUtility.ToJson(act_p));
            //string actionMsg = MessageObjAction.serialize(ObjectManager.identity, gameObject.name + "_ghost", JsonUtility.ToJson(act_p));
            string msg = MessageModelMapper.serialize(MessageObjAction.TYPE_NAME, actionMsg);
            // Send message
            NetworkManager.sendMessage(msg);
            // Temporarily simulates here
            //MessageModelMapper.processMsg(msg);
        }
    }
}
