using UnityEngine;

public class Scene2dSwitcher : MonoBehaviour
{
    public string ListenTag = "2DScene";
    public GameObject[] Listeners;

    private void OnTriggerEnter(Collider other)
    {
        if (null == Listeners)
        {
            return;
        }
        if (other.tag == ListenTag)
        {
            foreach (GameObject listener in Listeners)
            {
                listener.SendMessage("On2DSceneEnter");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (null == Listeners)
        {
            return;
        }
        if (other.tag == ListenTag)
        {
            foreach (GameObject listener in Listeners)
            {
                listener.SendMessage("On2DSceneExit");
            }
        }
    }
}