using UnityEngine;

public class GameOverOnTrigger2D : MonoBehaviour
{
    [Tooltip("Every object tagged with this tag will trigger game over")]
    [SerializeField] private string triggeringTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == triggeringTag && enabled)
        {
            Debug.Log("Game over!");

            // Quit application in runtime builds
            Application.Quit();

            // Stop play mode in the Unity Editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    private void Update()
    {
        /* Just to show the enabled checkbox in Editor */
    }
}
