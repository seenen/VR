using UnityEngine;
using System.Collections;

public class OctreeSample : MonoBehaviour
{

    public Transform MyContainer = null;

    BoundsOctree<GameObject> boundsTree = null;

    PointOctree<GameObject> pointTree = null;

    public GameObject myObject;

    public Bounds myBounds;

    public Vector3 myVector3;

    public Ray myRay;

    // Use this for initialization
    void Start () {
        // Initial size (metres), initial centre position, minimum node size (metres), looseness
        boundsTree = new BoundsOctree<GameObject>(15, MyContainer.position, 1, 1.25f);
        // Initial size (metres), initial centre position, minimum node size (metres)
        pointTree = new PointOctree<GameObject>(15, MyContainer.position, 1);

        boundsTree.Add(myObject, myBounds);
        //boundsTree.Remove(myObject);

        pointTree.Add(myObject, myVector3);
        //boundsTree.Remove(myObject);

        //bool result = boundsTree.IsColliding(bounds);

        //GameObject[] result2 = boundsTree.GetColliding(bounds);

        //pointTree.GetNearby(myRay, 4);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        if (boundsTree != null)
        {

            boundsTree.DrawAllBounds(); // Draw node boundaries
            boundsTree.DrawAllObjects(); // Draw object boundaries
            boundsTree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries
        }

        if (pointTree != null)
        {
            pointTree.DrawAllBounds(); // Draw node boundaries
            pointTree.DrawAllObjects(); // Mark object positions
        }
    }
}
