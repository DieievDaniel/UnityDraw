using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed;

    #region MONO
    private void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
        #elif UNITY_IOS || UNITY_ANDROID
        HandleTouchInput();
        #endif
    }
    #endregion
    private void HandleMouseInput()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            RotateAroundTarget(horizontalInput, verticalInput);
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log($"Touch phase: {touch.phase}, position: {touch.position}");

            if (touch.phase == TouchPhase.Moved)
            {
                float horizontalInput = touch.deltaPosition.x;
                float verticalInput = touch.deltaPosition.y;

                RotateAroundTarget(horizontalInput, verticalInput);
            }
        }
    }


    private void RotateAroundTarget(float horizontalInput, float verticalInput)
    {
        float horizontalRotation = horizontalInput * rotationSpeed * Time.deltaTime;
        float verticalRotation = verticalInput * rotationSpeed * Time.deltaTime;

        transform.RotateAround(target.position, Vector3.up, horizontalRotation);
        transform.RotateAround(target.position, transform.right, -verticalRotation);
    }
}
