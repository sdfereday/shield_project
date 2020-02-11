using UnityEngine;

public class RelativeCam : MonoBehaviour
{
    public Transform camTransform;

    private Vector3 relativePosition;
    private Vector2 inputVector;
   
    private void Update()
    {
        // TODO: I would think about using a helper component for this sort of stuff since it's being used elsewhere too.
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        Vector3 camF = camTransform.forward;
        Vector3 camR = camTransform.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        // TODO: Consider NOT using rotation, as I think the old FF games may have just tracked along axis, not rotated?
        relativePosition = (camF * inputVector.y + camR * inputVector.x);
    }

    public Vector3 GetRelativePosition() => relativePosition;
}
