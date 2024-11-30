using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject runnerObject = new GameObject("CoroutineRunner");
                instance = runnerObject.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runnerObject); // Ensure it persists across scenes
            }
            return instance;
        }
    }
}
