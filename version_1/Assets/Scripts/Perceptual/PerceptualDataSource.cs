using UnityEngine;
using System.Collections;

public class PerceptualDataSource : MonoBehaviour {

    private PXCUPipeline pp = null;

    private PXCMGesture.GeoNode[][] handData;

    private PXCMGesture.Gesture[] gestureHandData;
	
	private bool ready = false;

    void Start()
    {
		
		handData = new PXCMGesture.GeoNode[2][];
        handData[0] = new PXCMGesture.GeoNode[6];
        handData[1] = new PXCMGesture.GeoNode[6];
		
        PXCUPipeline.Mode mode = PXCUPipeline.Mode.GESTURE;

        pp = new PXCUPipeline();

        gestureHandData = new PXCMGesture.Gesture[2];		

        if (!pp.Init(mode))
        {
            Debug.LogError("Unable to initialize the PXCUPipeline. Running application without camera");
        }        
    }

    void Update()
    {

        if (pp == null) return;
        if (!pp.AcquireFrame(false)) return;

        pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, handData[0]);
        pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY, handData[1]);
        pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out gestureHandData[0]);
        pp.QueryGesture(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY, out gestureHandData[1]);

        pp.ReleaseFrame();

    }

    public PXCMGesture.GeoNode[][] HandData
    {
        get { return this.handData; }
        set { this.handData = value; }
    }

    public PXCMGesture.Gesture[] GestureData
    {
        get { return this.gestureHandData; }
        set { this.gestureHandData = value; }

    }

    public bool IsReady()
    {
        return pp != null;
    }
}
