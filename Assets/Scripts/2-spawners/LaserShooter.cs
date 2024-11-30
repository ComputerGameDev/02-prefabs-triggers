using UnityEngine;

public class LaserShooter : ClickSpawner
{
    [SerializeField]
    [Tooltip("How many points to add to the shooter, if the laser hits its target.")]
    private int pointsToAdd = 1;

    private NumberField scoreField;  // Reference to the score text field
    private bool canShoot = true;   // Controls whether the player can shoot

    private void Start()
    {
        // Find the NumberField component in children
        scoreField = GetComponentInChildren<NumberField>();
        if (!scoreField)
        {
            Debug.LogError($"No child of {gameObject.name} has a NumberField component!");
        }
    }

    private void Update()
    {
        // Allow shooting only if canShoot is true
        if (canShoot && spawnAction.WasPressedThisFrame())
        {
            spawnObject();
        }
    }

public void SetShootingEnabled(bool enabled)
{
    if (this == null || !gameObject.activeInHierarchy)
    {
        Debug.LogWarning($"{gameObject.name}: Attempted to set shooting state, but object is inactive or destroyed.");
        return;
    }

    canShoot = enabled;
    Debug.Log($"{gameObject.name}: Shooting set to {enabled} (canShoot = {canShoot})");
}


}
