using UnityEngine;

public class MobileInputs : MonoBehaviour
{
    // Pinch Variables
    float pinchLastDistance;

    // Swipe Variables
    Vector2 swipeStartPos;
    float swipeMinDistance = 50f;
    bool isSwiping = false;
    bool rotating = false;
    Quaternion targetRotation;

    [SerializeField]
    public GameObject cube;
    public Material material;


    void Update()
    {
        SingleTap();
        Pinch();
        Swipe();
    }
    void SingleTap()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0); // first finger

            if (t.phase == TouchPhase.Began)
            {
                isSwiping = false;
            }

            if (t.phase == TouchPhase.Ended)
            {
                if (!isSwiping)
                {
                    Debug.Log("Single Tap Detected");
                    material.color = Random.ColorHSV();
                }
            }
        }
    }
    void Pinch()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0); // first finger
            Touch t1 = Input.GetTouch(1); // second finger

            float currentDistance = Vector2.Distance(t0.position, t1.position);

            if (t1.phase == TouchPhase.Began)
            {
                pinchLastDistance = currentDistance;
                return;
            }
            float difference = currentDistance - pinchLastDistance;

            if (Mathf.Abs(difference) > 5f)
            {
                Vector3 scale = cube.transform.localScale;

                if (difference > 0)
                {
                    Debug.Log("Zoom In");
                    scale += Vector3.one * 0.1f;
                }
                else
                {
                    Debug.Log("Zoom Out");
                    scale -= Vector3.one * 0.1f;
                }
                scale.x = Mathf.Clamp(scale.x, 0.1f, 3f);
                scale.y = Mathf.Clamp(scale.y, 0.1f, 3f);
                scale.z = Mathf.Clamp(scale.z, 0.1f, 3f);

                cube.transform.localScale = scale;
            }
            pinchLastDistance = currentDistance;
        }
    }
    void Swipe()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                swipeStartPos = t.position;
                isSwiping = false;
            }

            if (t.phase == TouchPhase.Moved)
            {
                isSwiping = true;

                Vector2 swipe = t.position - swipeStartPos;

                float rotateSpeed = 0.1f;

                cube.transform.Rotate(
                    Vector3.right * swipe.y * rotateSpeed,
                    Space.World
                    );
                cube.transform.Rotate(
                    Vector3.up * -swipe.x * rotateSpeed,
                    Space.World
                    );

                swipeStartPos = t.position;
            }
        }
    }
}
