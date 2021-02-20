using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float loyality, zoomInMax, zoomOutMax, shiftY;
    public GameObject target;
    public bool isFightPhase;

    private Camera vcam;

    private void Awake()
    {
        vcam = GetComponent<Camera>();
    }

    public void FixedUpdate()
    {

        // Smooth camera zoom //
        if (target != null)
        {
            if (isFightPhase)
            {
                vcam.orthographicSize = Mathf.Lerp(vcam.orthographicSize, zoomInMax, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y + shiftY, transform.position.z), loyality);
            }
            else
            {
                vcam.orthographicSize = Mathf.Lerp(vcam.orthographicSize, zoomOutMax, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y + shiftY, transform.position.z), loyality);
            }
        }

        if (target == null)
        {

            // Mobile Version //
            if (!isFightPhase && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                Vector2 direction = touch.position;
                transform.position = new Vector2(Mathf.Lerp(transform.position.x, direction.x, Time.deltaTime), Mathf.Lerp(transform.position.y, direction.y, Time.deltaTime));
            }

            // PC Version //
            if (!isFightPhase && Input.GetMouseButtonDown(0))
            {
                Vector2 direction = Input.mousePosition;
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, direction.x, Time.deltaTime), Mathf.Lerp(transform.position.y, direction.y, Time.deltaTime), transform.position.z);

            }
        }
    }
}
