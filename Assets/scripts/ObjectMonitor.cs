using UnityEngine;

namespace MultiPlayer
{
    public abstract class ObjectMonitor : MonoBehaviour
    {
        int fps = 30;
        float spf;
        int frameCount;

        public ObjectMonitor()
        {
            spf = 1.0f / fps;
            frameCount = 0;
        }

        // do not override Update() in inheritance class
        public void Update()
        {
            frameCount += 1;
            if (frameCount > spf / Time.deltaTime)
            {
                frameCount = 0;
                trackState();
            }
        }

        protected abstract void trackState(); // function used to detect state change, called every fixed number of frames
    }
}