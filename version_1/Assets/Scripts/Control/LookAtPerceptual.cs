using System;
using UnityEngine;
using System.Collections;

public class LookAtPerceptual : LookAtBase {

    private int[] imageSize; //Creative camera image size

    private PXCMGesture.GeoNode[][] handData;

    private PerceptualDataSource perceptualDataSource;

    public GameObject perceptualObj;

    void Start()
    {

        base.Start();

        if (!perceptualObj)
        {
            Debug.LogError("Perceptual object not found");
        }

        perceptualDataSource = perceptualObj.GetComponent<PerceptualDataSource>();

        if (!perceptualDataSource)
        {
            Debug.LogError("Perceptual datasource script not found");
        }

        imageSize = new int[2];
        imageSize[0] = 320;
        imageSize[1] = 240;

        handData = new PXCMGesture.GeoNode[2][];
        handData[0] = new PXCMGesture.GeoNode[6];
        handData[1] = new PXCMGesture.GeoNode[6];	

    }

    private Vector2 getMidPoint(Vector2 a, Vector2 b)
    {
        return new Vector2((a.x + b.x) / 2f, (a.y + b.y) / 2f);
    }

    public Vector2 MapCoordinates(PXCMPoint3DF32 imagePos)
    {
        return new Vector2((float)(imageSize[0] - 1 - imagePos.x) / imageSize[0], (float)(imageSize[1] - 1 - imagePos.y) / imageSize[1]);
    }

    public override void UpdateControlRay()
    {
        if (perceptualDataSource && perceptualDataSource.IsReady())
        {
            PXCMGesture.GeoNode[][] handData = perceptualDataSource.HandData;			

            if (handData[0][0].body > 0 && handData[1][0].body > 0)
            {


                Vector2 positionPrimary = MapCoordinates(handData[0][0].positionImage);
                Vector2 positionSecondary = MapCoordinates(handData[1][0].positionImage);

                Vector2 midPoint = getMidPoint(positionPrimary, positionSecondary);

                ray = camera.ViewportPointToRay(new Vector3(1 - midPoint.x, 1 - midPoint.y, 0));
			}
        }
    }
}
