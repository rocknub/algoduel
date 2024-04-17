using UnityEngine;

namespace AppDebug
{
    public class DebugEvent : MonoBehaviour
    {
        public void DoIt(string value)
        {
            Debug.Log(value);
        }
    }
}