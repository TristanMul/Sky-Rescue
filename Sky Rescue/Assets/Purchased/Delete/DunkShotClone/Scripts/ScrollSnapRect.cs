using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(ScrollRect))]
public class ScrollSnapRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject[] Arrows;

    [Tooltip("Set starting page index - starting from 0")]
    public static int startingPage;
    [Tooltip("Threshold time for fast swipe in seconds")]
    public float fastSwipeThresholdTime = 0.3f;
    [Tooltip("Threshold time for fast swipe in (unscaled) pixels")]
    public int fastSwipeThresholdDistance = 100;
    [Tooltip("How fast will page lerp to target position")]
    public float decelerationRate = 10f;

    // fast swipes should be fast and short. If too long, then it is not fast swipe
    private float _fastSwipeThresholdMaxLimit;

    public ScrollRect ScrollRectComponent;
    public Button Buy_Button;

    private RectTransform _scrollRectRect;
    private RectTransform _container;

    // number of pages in container
    private int _pageCount;
   
    public int CurrentPage;

    // whether lerping is in progress and target lerp position
    private bool _horizontal;
    private bool _lerp;
    private Vector2 _lerpTo;

    // target position of every page
    private List<Vector2> _pagePositions = new List<Vector2>();

    // in draggging, when dragging started and where it started
    private bool _dragging;
    private float _timeStamp;
    private Vector2 _startPosition;

    // for showing small page icons
    private bool _showPageSelection;

    private int _previousPageSelectionIndex;

    private float wightOffset;

    private void OnEnable()
    {
        Invoke("Init", 0.2f);
    }

    private void Init()
    {
        GetScreenSize();

        //_scrollRectComponent = GetComponent<ScrollRect>();
        _scrollRectRect = GetComponent<RectTransform>();
        _container = ScrollRectComponent.content;
        _pageCount = _container.childCount;

        // is it horizontal or vertical scrollrect
        if (ScrollRectComponent.horizontal && !ScrollRectComponent.vertical)
        {
            _horizontal = true;
        }
        else if (!ScrollRectComponent.horizontal && ScrollRectComponent.vertical)
        {
            _horizontal = false;
        }
        else
        {
            Debug.LogWarning("Confusing setting of horizontal/vertical direction. Default set to horizontal.");
            _horizontal = true;
        }

        _lerp = false;

        // init
        SetPagePositions();

        SetPage(PlayerPrefs.GetInt("BallId"));
        BuyButton.UpdateTextAction();

        //Activate Deactivate Arrow
        Arrows[0].SetActive((CurrentPage == 0) ? false : true);
        Arrows[1].SetActive((CurrentPage == _pageCount - 1) ? false : true);
    }

    //------------------------------------------------------------------------
    void Update()
    {
        // if moving to target position
        if (_lerp)
        {
            // prevent overshooting with values greater than 1
            float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
            _container.anchoredPosition = Vector2.Lerp(_container.anchoredPosition, _lerpTo, decelerate);
            // time to stop lerping?
            if (Vector2.SqrMagnitude(_container.anchoredPosition - _lerpTo) < 0.25f)
            {
                // snap to target and stop lerping
                _container.anchoredPosition = _lerpTo;
                _lerp = false;
                // clear also any scrollrect move that may interfere with our lerping
                ScrollRectComponent.velocity = Vector2.zero;
            }
        }
    }

    //------------------------------------------------------------------------
    private void SetPagePositions()
    {
        float width = 0;
        float height = 0;
        float offsetX = 0;
        float offsetY = 0;
        float containerWidth = 0;
        float containerHeight = 0;

        if (_horizontal)
        {
            // screen width in pixels of scrollrect window
            width = _scrollRectRect.rect.width / wightOffset;
            // center position of all pages
            offsetX = width / 2 + 100f;
            // total width
            containerWidth = width * _pageCount + 280f;
            // limit fast swipe length - beyond this length it is fast swipe no more
            _fastSwipeThresholdMaxLimit = width;
        }
        else
        {
            height = _scrollRectRect.rect.height;
            offsetY = height / 2;
            containerHeight = height * _pageCount;
            _fastSwipeThresholdMaxLimit = height;
        }

        // set width of container
        Vector2 newSize = new Vector2(containerWidth, containerHeight);
        _container.sizeDelta = newSize;
        Vector2 newPosition = new Vector2(containerWidth / 2, containerHeight / 2);
        _container.anchoredPosition = newPosition;

        // delete any previous settings
        _pagePositions.Clear();

        // iterate through all container childern and set their positions
        for (int i = 0; i < _pageCount; i++)
        {
            RectTransform child = _container.GetChild(i).GetComponent<RectTransform>();
            Vector2 childPosition;
            if (_horizontal)
            {
                childPosition = new Vector2(i * width - containerWidth / 2 + offsetX, 0f);
            }
            else
            {
                childPosition = new Vector2(0f, -(i * height - containerHeight / 2 + offsetY));
            }
            child.anchoredPosition = childPosition;
            _pagePositions.Add(-childPosition);
        }
    }

    //------------------------------------------------------------------------
    private void SetPage(int aPageIndex)
    {
        aPageIndex = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
        _container.anchoredPosition = _pagePositions[aPageIndex];
        CurrentPage = aPageIndex;
    }

    //------------------------------------------------------------------------
    private void LerpToPage(int aPageIndex)
    {
        aPageIndex = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
        _lerpTo = _pagePositions[aPageIndex];
        _lerp = true;
        CurrentPage = aPageIndex;

        BuyButton.UpdateTextAction();

        //Activate Deactivate Arrow
        Arrows[0].SetActive((CurrentPage == 0) ? false : true);
        Arrows[1].SetActive((CurrentPage == _pageCount - 1) ? false : true);
    }
    //------------------------------------------------------------------------

    public void NextScreen()
    {
        if (CurrentPage + 1 == _pageCount)
        {

        }
        else
        {
            LerpToPage(CurrentPage + 1);
        }
    }
    //------------------------------------------------------------------------
    public void PreviousScreen()
    {

        if (CurrentPage - 1 < 0)
        {

        }
        else
        {
            LerpToPage(CurrentPage - 1);
        }
    }
    //------------------------------------------------------------------------
    private int GetNearestPage()
    {
        // based on distance from current position, find nearest page
        Vector2 currentPosition = _container.anchoredPosition;

        float distance = float.MaxValue;
        int nearestPage = CurrentPage;

        for (int i = 0; i < _pagePositions.Count; i++)
        {
            float testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);
            if (testDist < distance)
            {
                distance = testDist;
                nearestPage = i;
            }
        }
        return nearestPage;
    }

    //------------------------------------------------------------------------
    public void OnBeginDrag(PointerEventData aEventData)
    {
        // if currently lerping, then stop it as user is draging
        _lerp = false;
        // not dragging yet
        _dragging = false;
    }

    //------------------------------------------------------------------------
    public void OnEndDrag(PointerEventData aEventData)
    {
        // how much was container's content dragged
        float difference;
        if (_horizontal)
        {
            difference = _startPosition.x - _container.anchoredPosition.x;
        }
        else
        {
            difference = -(_startPosition.y - _container.anchoredPosition.y);
        }

        // test for fast swipe - swipe that moves only +/-1 item
        if (Time.unscaledTime - _timeStamp < fastSwipeThresholdTime &&
        Mathf.Abs(difference) > fastSwipeThresholdDistance &&
        Mathf.Abs(difference) < _fastSwipeThresholdMaxLimit)
        {

            if (difference > 0)
            {
                NextScreen();
            }
            else
            {
                PreviousScreen();
            }
        }
        else
        {
            // if not fast time, look to which page we got to
            LerpToPage(GetNearestPage());
        }
        _dragging = false;
        Buy_Button.interactable = true;
    }

    //------------------------------------------------------------------------
    public void OnDrag(PointerEventData aEventData)
    {
        if (CurrentPage != 0 && aEventData.delta.x > 0 || CurrentPage != _pageCount - 1 && aEventData.delta.x < 0)
        {
            if (!_dragging)
            {
                // dragging started
                _dragging = true;
                // save time - unscaled so pausing with Time.scale should not affect it
                _timeStamp = Time.unscaledTime;
                // save current position of cointainer
                _startPosition = _container.anchoredPosition;
            }
        }
        Buy_Button.interactable = false;
    }

    private void GetScreenSize()
    {
        switch (Screen.width)
        {
            case 1080:
                wightOffset = 1.25f;
                break;

            case 1440:
                wightOffset = 1.19f;
                break;

            default:
                wightOffset = 1.3f;
                break;
        }
    }
}
