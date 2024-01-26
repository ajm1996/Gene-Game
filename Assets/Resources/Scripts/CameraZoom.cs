using System.Collections;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    private Camera cam;

    public float zoomEffectDuration;
    public float zoomStart;
    public float zoomEnd;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMoveCamera(Vector3 origin, Vector3 destination) {
        StartCoroutine(MoveCamera(origin, destination, zoomStart, zoomEnd));
    }

    IEnumerator MoveCamera(Vector3 origin, Vector3 destination, float zoomStart, float zoomEnd) {
        float totalMovementTime = zoomEffectDuration; //the amount of time you want the movement to take
        float currentMovementTime = 0f;//The amount of time that has passed
        while (Vector3.Distance(transform.localPosition, destination) > 0) {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(origin, destination, currentMovementTime / totalMovementTime);
            cam.orthographicSize = Mathf.Lerp(zoomStart, zoomEnd, currentMovementTime / (totalMovementTime / 2f));

            if(currentMovementTime / totalMovementTime > 0.5f) {
                cam.orthographicSize = Mathf.Lerp(zoomEnd, zoomStart, (currentMovementTime - (totalMovementTime / 2f)) / (totalMovementTime / 2f));
            }

            yield return null;
        }
    }

    //example of how to use from the game script
    //GetComponent<CameraZoom>().StartMoveCamera(transform.position, transform.position + new Vector3(0, 30, -10));
}
