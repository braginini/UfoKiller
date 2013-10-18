using UnityEngine;
using System.Collections;

public class PerceptualTurret : Turret {

    private PerceptualDataSource perceptualDataSource;

    public GameObject perceptualObj;

    private PXCMGesture.GeoNode[][] handData;

    private PXCMGesture.Gesture[] gestureHandData;

    public override void Start()
    {
        base.Start();

        if (!perceptualObj)
        {
            Debug.LogError("Perceptual object not found");
        }
        else
        {
            perceptualDataSource = perceptualObj.GetComponent<PerceptualDataSource>();

            if (!perceptualDataSource)
            {
                Debug.LogError("Perceptual datasource script not found");
            } 
        }
    }

    public override void Update()
    {
        base.Update();

        if (perceptualDataSource && perceptualDataSource.IsReady())
        {
            handData = perceptualDataSource.HandData;
            gestureHandData = perceptualDataSource.GestureData;

            if (handData == null || gestureHandData == null)
                return;

            if (isTriggerPressed())
            {
                Fire();
                StartCoroutine(Reload());
            }
        }

    }

    //checks if we are ready to shoot - preconditions and both thumbs are up
    private bool isReadyToShoot()
    {

        return preconditions(handData[0], handData[1])
            && thumbsUp(gestureHandData[0], gestureHandData[1]);
    }

    //checks whether both triggers are pressed
    private bool isTriggerPressed()
    {
        return preconditions(handData[0], handData[1])
            && handsClosed(handData[0], handData[1]);
    }

    //check whether basic conditions are satisfied - hands recognized and hands are in a closed state
    private bool preconditions(PXCMGesture.GeoNode[] primaryHand, PXCMGesture.GeoNode[] secondaryHand)
    {

        bool recognized = handsRecognized(primaryHand, secondaryHand);
        bool closed = handsInClosedState(primaryHand, secondaryHand);

        return recognized && closed;
    }

    //checks whether both hands are recognized
    private bool handsRecognized(PXCMGesture.GeoNode[] primaryHand, PXCMGesture.GeoNode[] secondaryHand)
    {

        bool primaryRecognized = primaryHand[0].body > 0;
        bool secondaryRecognized = secondaryHand[0].body > 0;

        return (primaryRecognized && secondaryRecognized);
    }

    //check whether both hands are in a closed state
    private bool handsInClosedState(PXCMGesture.GeoNode[] primaryHand, PXCMGesture.GeoNode[] secondaryHand)
    {

        bool primaryClosed = primaryHand[0].opennessState.Equals(PXCMGesture.GeoNode.Openness.LABEL_CLOSE);
        bool secondaryClosed = secondaryHand[0].opennessState.Equals(PXCMGesture.GeoNode.Openness.LABEL_CLOSE);

        return (primaryClosed && secondaryClosed);
    }

    //check whether both hands are fully closed - in closed state and openness = 0
    private bool handsClosed(PXCMGesture.GeoNode[] primaryHand, PXCMGesture.GeoNode[] secondaryHand)
    {

        if (handsInClosedState(primaryHand, secondaryHand))
        {

            bool primaryClosed = primaryHand[0].openness == 0;
            bool secondaryClosed = secondaryHand[0].openness == 0;

            return primaryClosed && secondaryClosed;
        }

        return false;
    }

    //check whether both thumbs are recognized
    private bool thumbsRecognized(PXCMGesture.GeoNode[] primaryHand, PXCMGesture.GeoNode[] secondaryHand)
    {

        bool primaryThumb = primaryHand[(int)PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB].body > 0;
        bool secondaryThumb = secondaryHand[(int)PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB].body > 0;

        return (primaryThumb && secondaryThumb);
    }

    //check whether both thums are up
    private bool thumbsUp(PXCMGesture.Gesture primaryHand, PXCMGesture.Gesture secondaryHand)
    {

        bool primaryThumbUp = primaryHand.label.Equals(PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP);
        bool secondaryThumbUp = secondaryHand.label.Equals(PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP);

        return (primaryThumbUp && secondaryThumbUp);

    }
}
