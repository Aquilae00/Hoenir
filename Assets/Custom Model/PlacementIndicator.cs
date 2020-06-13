using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlacementIndicator : MonoBehaviour
{

    public Camera firstPersonCamera;
    public GameObject placementIndicator;
    public GameObject model;


    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool exist;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Script Started");
        placementIndicator = Instantiate(placementIndicator);
    }


    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        //PlacementSetup();
    }

    private void PlacementSetup()
    {
        //TrackableHit hit;
        //TrackableHitFlags raycastFilter = TrackableHitFlags.Default;
        //if (Frame.Raycast())
        //{

        //}
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            Debug.Log("indicator placement valid");
            placementIndicator.SetActive(true);
            Debug.Log(placementPose.position);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = firstPersonCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinInfinity;
        if (Frame.Raycast(screenCenter.x, screenCenter.y, raycastFilter, out hit))
        {
            placementPoseIsValid = hit.Trackable is DetectedPlane;
            Debug.Log(placementPoseIsValid);
            if (placementPoseIsValid)
            {
                placementPose = hit.Pose;
                Debug.Log("pose updated");
            }
        }
    }
}
