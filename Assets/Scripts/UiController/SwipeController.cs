using System;
using System.Collections;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [SerializeField]
    private float _minTimeToSwipe;

    [SerializeField]
    private float _minSwipeDistance;

    private bool _swipeRoutineStarted;

    public Action OnSwiped;

#if UNITY_EDITOR
    [SerializeField] 
    private Camera _viewCamera;
#endif

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount == 1 && !_swipeRoutineStarted)
        {
            Debug.DrawLine(Input.GetTouch(0).position, Input.GetTouch(0).position + Vector2.up * _minSwipeDistance, Color.cyan, 5f);
            StartCoroutine(SwipeRoutine());
        }
#endif

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && !_swipeRoutineStarted)
        {
            Vector3 worldPosition = _viewCamera.ScreenToWorldPoint(Input.mousePosition);

            worldPosition = new Vector3(worldPosition.x, worldPosition.y, -2);

            Debug.DrawLine(worldPosition, worldPosition + Vector3.up * _minSwipeDistance, Color.cyan, 5f);
            StartCoroutine(SwipeRoutine());
        }
#endif
    }

    private IEnumerator SwipeRoutine()
    {
        Vector3 startPosition = Input.mousePosition;
        _swipeRoutineStarted = true;
        yield return new WaitForSeconds(_minTimeToSwipe);

//#if UNITY_ANDROID && !UNITY_EDITOR
//        if (Input.touchCount != 1)
//        {
//            Debug.Log("Swipe canceled");
//            yield return null;
//        }

//        while (Input.GetTouch(0).phase != TouchPhase.Ended || Input.GetTouch(0).phase != TouchPhase.Canceled)
//        {
//            deltaMovePosition = Input.GetTouch(0).deltaPosition;
//            yield return new WaitForEndOfFrame();
//        }
//#endif

        if (!Input.GetMouseButton(0))
        {
            Debug.Log("Swipe canceled");
            _swipeRoutineStarted = false;
            yield break;
        }

        //Vector3 startPosition = Input.mousePosition;
        Vector3 endPosition = Vector3.zero;
        while (Input.GetMouseButton(0))
        {
            //deltaMovePosition = startPosition - Input.mousePosition;

            endPosition = Input.mousePosition;

            yield return new WaitForEndOfFrame();
        }

        Vector2 deltaMovePosition = startPosition - endPosition;

        Debug.Log("Swipe stopped: " + deltaMovePosition + "     " + startPosition + "       " + endPosition);

        if (deltaMovePosition.y * -1 > _minSwipeDistance)
        {
            Swiped();
        }

        _swipeRoutineStarted = false;
    }

    private void Swiped()
    {
        Debug.Log("Swiped!");
        OnSwiped?.Invoke();
    }
}
