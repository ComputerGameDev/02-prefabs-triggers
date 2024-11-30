using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This component moves its object when the player clicks the arrow keys.
 * Added circular world effect on the X axis and bounded world on the Y axis.
 */
public class InputMover : MonoBehaviour
{
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] float speed = 10f;

    [SerializeField] InputAction move = new InputAction(
        type: InputActionType.Value, expectedControlType: nameof(Vector2));

    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;

    void OnEnable()  
    {
        move.Enable();
        
        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
    }

    void OnDisable()  
    {
        move.Disable();
    }

    void Update() 
    {
        Vector2 moveDirection = move.ReadValue<Vector2>();
        Vector3 movementVector = new Vector3(moveDirection.x, moveDirection.y, 0) * speed * Time.deltaTime;

        if (transform.position.x < screenLeft)
        {
            transform.position = new Vector3(screenRight, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > screenRight)
        {
            transform.position = new Vector3(screenLeft, transform.position.y, transform.position.z);
        }

        float newY = Mathf.Clamp(transform.position.y + movementVector.y, screenBottom, screenTop);
        
        transform.position = new Vector3(transform.position.x + movementVector.x, newY, transform.position.z);
    }
}
