using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiPlayer;
using System;

public class SimpleNetworkObject : NetworkObject {
    [Serializable]
    public class ActionPacket
    {
        public string actionName;
        public string args;

        public ActionPacket() { }
        public ActionPacket(string actionName, string args)
        {
            this.actionName = actionName;
            this.args = args;
        }
    }

    private Queue<ActionPacket> actions;

	// Use this for initialization
	public override void Start () {
        prefabClassName = "partical_system";
        base.Start();
        // do something for initialization
        actions = new Queue<ActionPacket>();

        // network test
        ObjectManager.syncObject();
    }
	
	// Update is called once per frame
	void Update () {
        while (actions.Count > 0)
        {
            ActionPacket act_p = actions.Dequeue();
            if (act_p.actionName == "move")
                move(act_p.args);
        }
	}

    public override void distributeAction(string action)
    {
        actions.Enqueue(JsonUtility.FromJson<ActionPacket>(action));
    }

    [Serializable]
    public class StateArgs
    {
        public float position_x, position_y, position_z;
        public float rotation_x, rotation_y, rotation_z, rotation_w;

        public StateArgs()
        {
            position_x = position_y = position_z = 0;
            rotation_x = rotation_y = rotation_z = rotation_w = 0;
        }

        public StateArgs(Vector3 position, Quaternion rotation)
        {
            setPosition(position);
            setRotation(rotation);
        }

        public void setPosition(Vector3 position)
        {
            position_x = position.x;
            position_y = position.y;
            position_z = position.z;
        }

        public void setRotation(Quaternion rotation)
        {
            rotation_x = rotation.x;
            rotation_y = rotation.y;
            rotation_z = rotation.z;
            rotation_w = rotation.w;
        }
    }

    public override string getStateArgs()
    {
        return JsonUtility.ToJson(new StateArgs(transform.position, transform.rotation));
    }

    public override void initState(string stateArgs)
    {
        StateArgs args = JsonUtility.FromJson<StateArgs>(stateArgs);
        transform.position = new Vector3(args.position_x, args.position_y, args.position_z);
        transform.rotation = new Quaternion(args.rotation_x, args.rotation_y, args.rotation_z, args.rotation_w);
    }

    [Serializable]
    public class ActionMoveArgs
    {
        public float p_x, p_y, p_z;
        public float r_x, r_y, r_z, r_w;

        public ActionMoveArgs()
        {
            p_x = p_y = p_z = 0;
            r_x = r_y = r_z = r_w = 0;
        }

        public ActionMoveArgs(Vector3 position, Quaternion rotation)
        {
            setPosition(position);
            setRotation(rotation);
        }

        public void setPosition(Vector3 position)
        {
            p_x = position.x;
            p_y = position.y;
            p_z = position.z;
        }

        public void setRotation(Quaternion rotation)
        {
            r_x = rotation.x;
            r_y = rotation.y;
            r_z = rotation.z;
            r_w = rotation.w;
        }
    }

    void move(string args)
    {
        ActionMoveArgs move_args = JsonUtility.FromJson<ActionMoveArgs>(args);
        transform.position = new Vector3(move_args.p_x, move_args.p_y, move_args.p_z);
        transform.rotation = new Quaternion(move_args.r_x, move_args.r_y, move_args.r_z, move_args.r_w);
    }
}
