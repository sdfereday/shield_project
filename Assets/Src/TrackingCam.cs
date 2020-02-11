using UnityEngine;

public class TrackingCam : MonoBehaviour
{
    public Transform trackingTarget;
    private Camera cam;

    private void Start() => cam = GetComponent<Camera>();

    private void Update() => FollowPlayer(trackingTarget);

    private void FollowPlayer(Transform followTarget) {
        Quaternion previousRotation = transform.rotation; // remember original rotation
        transform.LookAt(followTarget, Vector3.up); // look at player
                                                    // if looking at player caused the camera to turn outside its bounds, restore previous rotation
        if (IsCameraOutOfBounds())
        {
            transform.rotation = previousRotation;
        }
    }

    private bool IsCameraOutOfBounds() {
        Ray[] edgeRays = GetCameraEdgeRays(); // get camera extreme rays
        int layerMask = 1 << LayerMask.NameToLayer("SceneBounds"); // raycast layer should only find the "AreaLayer" collider - the collider that depicts the game area

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        var isInBounds = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

        if (isInBounds)
        {
            print("I'm looking at " + hit.transform.name);
        }
        else
        {
            print("I'm looking at nothing!");
        }

        return !isInBounds;

        /*
        foreach (Ray ray in edgeRays)
        {
            //Debug.Log(Physics.Raycast(ray, Mathf.Infinity, layerMask));
            Debug.DrawRay(ray.origin, transform.TransformDirection(Vector3.forward) * 100f, Color.red);
            // if raycast doesn't hit the area layer collider, camera is out of bounds
            if (!Physics.Raycast(ray, Mathf.Infinity, layerMask)) {
                return true;
            }
        }*/
    }

    // get 4 rays, one for each corner of the camera's view
    private Ray[] GetCameraEdgeRays() {
        Ray[] rays = new Ray[4];
        /*
        rays[0] = cam.ScreenPointToRay(new Vector3(0, 0, 0));
        rays[1] = cam.ScreenPointToRay(new Vector3(Screen.width, 0, 0));
        rays[2] = cam.ScreenPointToRay(new Vector3(Screen.width, Screen.height));
        rays[3] = cam.ScreenPointToRay(new Vector3(0, Screen.height, 0));
        */

        rays[0] = Camera.main.ViewportPointToRay(new Vector3(0, 0, 0));
        rays[1] = Camera.main.ViewportPointToRay(new Vector3(1, 0, 0));
        rays[2] = Camera.main.ViewportPointToRay(new Vector3(1, 1, 0));
        rays[3] = Camera.main.ViewportPointToRay(new Vector3(0, 1, 0));
        
        return rays;
    }

    public void SetTrackingTarget(Transform targ) => trackingTarget = targ;

    /*
    void SetCameraPosition(Transform camera, Transform player, float distanceFromPlayer) {
        Quaternion rotate60Upwards = Quaternion.AngleAxis(60, Vector3.right);
        Vector3 fromPlayerToCamera = (rotate60Upwards * Vector3.back) * distanceFromPlayer;
        camera.position = player.position + fromPlayerToCamera;
        camera.rotation = Quaternion.AngleAxis(60, Vector3.right);
    }*/
}
