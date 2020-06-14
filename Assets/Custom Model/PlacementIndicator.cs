using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;
using System.Linq;

public class PlacementIndicator : MonoBehaviour
{

    public Camera firstPersonCamera;
    //prefabs
    public GameObject placementIndicator;
    public GameObject placementIndicatorParticle;
    public GameObject model;

    //objects
    private GameObject placementIndicatorVar;
    private GameObject placementIndicatorFixed;
    private GameObject placedModel;

    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool exist;
    private TrackableHit hit;
    private const float k_PrefabRotation = 180.0f;


    // Start is called before the first frame update
    void Start()
    {
        Button button = GameObject.Find("ResetButton").GetComponent<Button>();
        button.onClick.AddListener(ResetAll);
        init();
    }
    private void init()
    {
        Debug.Log("Script Started");
        placementIndicatorVar = Instantiate(placementIndicator);
    }


    // Update is called once per frame
    void Update()
    {
        if(exist) { return; }
        if (placementIndicatorVar != null)
        {
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        } 
        else if (placementIndicatorFixed != null)
        {
            PlacementSetup();
        }

        
    }

    private void PlacementSetup()
    {
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }


        placedModel = Instantiate(model, placementIndicatorFixed.transform.position, placementIndicatorFixed.transform.rotation);

        var anchor = hit.Trackable.CreateAnchor(hit.Pose);
        //placedModel.transform.Rotate(0, k_PrefabRotation, 0, Space.Self);
        placedModel.transform.parent = anchor.transform;

        exist = true;
    }

    private void UpdatePlacementIndicator()
    {
        if(placementPoseIsValid)
        {
            Debug.Log("indicator placement valid");
            placementIndicatorVar.SetActive(true);
            Debug.Log(placementPose.position);
            placementIndicatorVar.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }


            Destroy(placementIndicatorVar);
            //var baseObject = Instantiate(placementIndicator, placementIndicator.transform.position, placementIndicator.transform.rotation);
            placementIndicatorFixed = Instantiate(placementIndicatorParticle, placementPose.position, placementPose.rotation);

            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
            placementIndicatorFixed.transform.Rotate(0, k_PrefabRotation, 0, Space.Self);
            placementIndicatorFixed.transform.parent = anchor.transform;

        }
        else
        {
            placementIndicatorVar.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = firstPersonCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
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


    private void ResetAll()
    {
        exist = false;

        //slow
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if (o == placementIndicatorFixed ||
                o == placementIndicatorVar ||
                o == placedModel)
            {
                Destroy(o);
            }

        }

        init();
    }

}
