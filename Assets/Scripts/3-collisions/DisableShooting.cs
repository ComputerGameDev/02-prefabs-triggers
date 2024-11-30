using System.Collections;
using UnityEngine;

public class ShootDisableTrigger : MonoBehaviour
{
    [Tooltip("How many seconds to disable shooting for.")]
    [SerializeField] private float disableShootingDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"ShootDisableTrigger triggered by {other.name}");

            LaserShooter laserShooter = other.GetComponent<LaserShooter>();
            if (laserShooter != null)
            {
                // Use a persistent CoroutineRunner to handle the coroutine
                CoroutineRunner.Instance.StartCoroutine(DisableShootingForDuration(laserShooter));
            }
            else
            {
                Debug.LogWarning($"LaserShooter component not found on {other.name}");
            }

            // Destroy the icon immediately after the collision
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"ShootDisableTrigger triggered by non-player object: {other.name}");
        }
    }

    private IEnumerator DisableShootingForDuration(LaserShooter laserShooter)
    {
        laserShooter.SetShootingEnabled(false);
        Debug.Log($"Shooting disabled for {laserShooter.name} for {disableShootingDuration} seconds!");

        yield return new WaitForSeconds(disableShootingDuration);

        laserShooter.SetShootingEnabled(true);
        Debug.Log($"Shooting re-enabled for {laserShooter.name} after {disableShootingDuration} seconds.");
    }
}
