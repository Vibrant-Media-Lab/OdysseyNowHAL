using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : PointerInputModule {
    [Header("Cursor Controls")] [Tooltip("The camera in your scene being used as the Main Camera.")]
    public Camera cameraMain;

    [Tooltip("The axis name in the Input Manager")]
    public string horizontalAxis = "Horizontal";

    [Tooltip("The axis name in the Input Manager")]
    public string verticalAxis = "Vertical";

    [Tooltip("The cursor x-Axis movement speed according to Input Manager sensitivity")]
    public float horizontalSpeed = 45.0f;

    float tempHspeed = 0.0f;

    [Tooltip("The cursor y-Axis movement speed according to Input Manager sensitivity")]
    public float verticalSpeed = 45.0f;

    float tempVspeed = 0.0f;

    [Tooltip("The speed of the cursor when hovering over a button")] [Range(0.1f, 1.0f)]
    public float hoverMultiplier = 0.33f;

    [Tooltip("The rect UI elements functioning as the CURSOR")]
    public RectTransform cursorRect;

    [Tooltip("The Rect Transform whose boundaries will be used to calculate edge of screen")]
    public RectTransform boundaries;

    [Tooltip("Enables and Disables movement control of the cursor")]
    public bool canMoveCursor = true;

    [Header("Realtime Settings")] public bool usingDesktopCursor = false;
    bool hasSwitchedToVirtualMouse = false;
    bool hasSwitchedToController = false;

    [Tooltip("The current array index that matches the selected cursor style. 0-44")]
    public int cursorIndex;

    public GameObject mouseInputModule;

    [Header("Offsets for Cursor Edge Bounds")] [Tooltip("Left Boundary Offset")]
    public float xminOffset = 0.0f;

    [Tooltip("Right Boundary Offset")] public float xmaxOffset = 0.0f;
    [Tooltip("Bottom Boundary Offset")] public float yminOffset = 0.0f;
    [Tooltip("Top Boundary Offset")] public float ymaxOffset = 0.0f;
    private float xMin = 0.0f;
    private float xMax = 0.0f;
    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private float xMovement, yMovement;

    public GameObject cursorObject = null;

    [Tooltip("All the possible cursor objects available in the scene")]
    public GameObject[] cursors;

    private Vector2 screenVec;
    private PointerEventData pointer;

    [Header("Input Controllers")]
    [Tooltip("If you want to switch between Mouse Input and Hand-Held Controller Input, add a game object with a Standalone Input Module attached and call it through the function.")]
    public EventSystem mouseEventSystem;

    [Header("Cursor Animator Controls")]
    [Tooltip("The name of the bool that will be called when the cursor is transitioning from DEFAULT to HOVER and vice versa.")]
    public string fadeBool = "";

    Animator fade;

    [Header("TOOLTIPS")] [Tooltip("The Rect Transform holding the position of the tool tips.")]
    public RectTransform tooltipRect;

    [Tooltip("The 'Width' of the Tooltip game object. This is how Console Cursors determines the size boundaries at runtime.")]
    public float toolTipWidth = 770;

    [Tooltip("The 'Height' of the Tooltip game object. This is how Console Cursors determines the size boundaries at runtime.")]
    public float toolTipHeight = 245;

    [Tooltip("The Pos X of the game object 'container' that is used to re-position the tooltip so it always stays visible on the screen. Adjusting this value will adjust how far left or right the tooltip positions itself when re-aligning.")]
    public float toolTipOffsetX = 468;

    [Tooltip("The Pos Y of the game object 'container' that is used to re-position the tooltip so it always stays visible on the screen. Adjusting this value will adjust how far left or right the tooltip positions itself when re-aligning.")]
    public float toolTipOffsetY = -206;

    private bool tooFarRight = false;
    private bool tooFarLeft = false;

    [Header("Demo Stuff")] public bool cursorButton = true; // if the button is a cursor or a normal style template

    public void Start() {
        xMovement = 0.5f;
        yMovement = 0.5f;

        // Set edge of screen bounds
        xMin = (boundaries.rect.width / 2 * -1) + xminOffset;
        xMax = (boundaries.rect.width / 2) + xmaxOffset;
        yMin = (boundaries.rect.height / 2 * -1) + yminOffset;
        yMax = (boundaries.rect.height / 2) + ymaxOffset;

        base.Start();
        pointer = new PointerEventData(eventSystem);

        // Fade for Cursor Animations set to currently selected cursor
        fade = cursorObject.GetComponent<Animator>();

        ChangeCursor(cursorIndex);

        // Remember the original cursor speed for when we need to alter it
        tempHspeed = horizontalSpeed;
        tempVspeed = verticalSpeed;
    }

    // Change cursor to the index corresponding to the GameObject array
    public void ChangeCursor(int num) {
        for (int i = 0; i < cursors.Length; i++) // disable cursors first
        {
            cursors[i].SetActive(false);
        }

        cursorIndex = num;
        cursors[num].SetActive(true);
        cursorObject = cursors[num];
        fade = cursorObject.GetComponent<Animator>();
        Cursor.visible = false;
    }

    public void SwitchToMouse() {
        hasSwitchedToVirtualMouse = true;
        hasSwitchedToController = false;
        mouseInputModule.SetActive(true);
        GetComponent<EventSystem>().enabled = false;
        for (int i = 0; i < cursors.Length; i++) // disable cursors first
        {
            cursors[i].SetActive(false);
        }
    }

    public void SwitchToController() {
        hasSwitchedToVirtualMouse = false;
        hasSwitchedToController = true;
        mouseInputModule.SetActive(false);
        GetComponent<EventSystem>().enabled = true;
        cursors[cursorIndex].SetActive(true);
        cursorObject = cursors[cursorIndex];
        fade = cursorObject.GetComponent<Animator>();
        cursorRect.anchoredPosition = new Vector3(0, 0, 0);
        //Debug.Log("Switching to virtual controller");
    }

    void Update() {
        // Cursor speed 
        xMovement = Time.deltaTime * (horizontalSpeed * 100) * Input.GetAxis(horizontalAxis);
        yMovement = Time.deltaTime * (verticalSpeed * 100) * Input.GetAxis(verticalAxis);

        if (mouseInputModule) {
            if (usingDesktopCursor) {
                Cursor.visible = true;
                if (!hasSwitchedToVirtualMouse) {
                    SwitchToMouse();
                }
            }
            else if (!usingDesktopCursor && !hasSwitchedToController) {
                Cursor.visible = false;
                if (!hasSwitchedToController) {
                    SwitchToController();
                }
            }
        }
        else if (!mouseInputModule) {
            print("There is no Mouse Input game object in the scene! Please add one to allow switching between cursor settings.");
        }

        if (canMoveCursor) {
            if (!usingDesktopCursor) {
                // Cursor Bounds
                if (cursorRect.anchoredPosition.x >= xMin) {
                    cursorRect.anchoredPosition += new Vector2(xMovement, 0);
                }

                if (cursorRect.anchoredPosition.x <= xMin && cursorRect.anchoredPosition.x <= xMax) {
                    cursorRect.anchoredPosition -= new Vector2(xMovement, 0);
                }

                if (cursorRect.anchoredPosition.x >= xMax && cursorRect.anchoredPosition.x >= xMin) {
                    cursorRect.anchoredPosition += new Vector2(-xMovement, 0);
                }

                if (cursorRect.anchoredPosition.y >= yMin) {
                    cursorRect.anchoredPosition += new Vector2(0, yMovement);
                }

                if (cursorRect.anchoredPosition.y <= yMin && cursorRect.anchoredPosition.y <= yMax) {
                    cursorRect.anchoredPosition -= new Vector2(0, yMovement);
                }

                if (cursorRect.anchoredPosition.y >= yMax && cursorRect.anchoredPosition.y >= yMin) {
                    cursorRect.anchoredPosition += new Vector2(0, -yMovement);
                }
            }
            else if (usingDesktopCursor) {
                cursorRect.position = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            }


            // Tool Tip boundaries
            if (cursorRect.anchoredPosition.x <= xMin + (toolTipWidth + 70)) {
                // Too Far left
                tooFarLeft = true;
                if (cursorRect.anchoredPosition.y <= yMin + (toolTipHeight + 55)) {
                    tooltipRect.anchoredPosition = new Vector2(toolTipOffsetX, toolTipOffsetY);
                }
                else {
                    tooltipRect.anchoredPosition = new Vector2(toolTipOffsetX, -toolTipOffsetY);
                }
            }
            else {
                tooFarLeft = false;
            }

            if (cursorRect.anchoredPosition.x >= xMax - (toolTipWidth + 70)) {
                // Too Far Right
                tooFarRight = true;
                if (cursorRect.anchoredPosition.y <= yMin + (toolTipHeight + 55)) {
                    tooltipRect.anchoredPosition = new Vector2(-toolTipOffsetX, toolTipOffsetY);
                }
                else {
                    tooltipRect.anchoredPosition = new Vector2(-toolTipOffsetX, -toolTipOffsetY);
                }
            }
            else {
                tooFarRight = false;
            }

            if (cursorRect.anchoredPosition.y <= yMin + (toolTipHeight + 55)) {
                if (!tooFarRight)
                    tooltipRect.anchoredPosition = new Vector2(toolTipOffsetX, toolTipOffsetY);
            }
            else {
                if (!tooFarRight)
                    tooltipRect.anchoredPosition = new Vector2(toolTipOffsetX, -toolTipOffsetY);
            }
        }
    }

    public void ShowTooltip() {
        tooltipRect.GetComponent<Animator>().SetBool("Show", true);
    }

    public void HideTooltip() {
        tooltipRect.GetComponent<Animator>().SetBool("Show", false);
    }

    public override void Process() {
        Vector3 screenPos = cameraMain.WorldToScreenPoint(cursorObject.transform.position);

        screenVec.x = screenPos.x;
        screenVec.y = screenPos.y;

        // Raycasting
        pointer.position = screenVec;
        eventSystem.RaycastAll(pointer, this.m_RaycastResultCache);
        RaycastResult raycastResult = FindFirstRaycast(this.m_RaycastResultCache);
        pointer.pointerCurrentRaycast = raycastResult;
        this.ProcessMove(pointer);

        pointer.clickCount = 0;
        if (Input.GetButtonDown("Submit")) {
            pointer.pressPosition = screenVec;
            pointer.clickTime = Time.unscaledTime;
            pointer.pointerPressRaycast = raycastResult;

            pointer.clickCount = 1;
            pointer.eligibleForClick = true;

            if (this.m_RaycastResultCache.Count > 0) {
                pointer.selectedObject = raycastResult.gameObject;
                pointer.pointerPress =
                    ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointer, ExecuteEvents.submitHandler);
                pointer.rawPointerPress = raycastResult.gameObject;

                // As long as it's not a button style demo, reset the current button
                if (cursorButton) {
                    // DEMO STUFF - RESET BUTTON
                    pointer.rawPointerPress.transform.parent.gameObject.SetActive(false); // temporarily disable button
                    NormalSpeed(); // reset cursor speed
                    StartCoroutine(resetButton(pointer.rawPointerPress));
                }
            }
            else {
                pointer.rawPointerPress = null;
            }
        }
        else {
            pointer.clickCount = 0;
            pointer.eligibleForClick = false;
            pointer.pointerPress = null;
            pointer.rawPointerPress = null;
        }
    }

    // Changes cursorButton status to NO which stops the button from resetting once pressed
    public void NoCursorButton() {
        cursorButton = false;
    }

    // Changes cursorButton status to YES which enables the button to be reset once pressed
    public void YesCursorButton() {
        cursorButton = true;
    }

    // JUST FOR DEMO - RESETTING BUTTON ON PRESS
    IEnumerator resetButton(GameObject point) {
        yield return new WaitForSeconds(0);
        point.transform.parent.gameObject.SetActive(true);
    }

    // FUNCTIONS USED BY USER /////////////////////////////////////

    // Called by trigger when a button is hovered over
    public void HoverSpeed() {
        // When hovering over something, slow down
        horizontalSpeed = horizontalSpeed * hoverMultiplier;
        verticalSpeed = verticalSpeed * hoverMultiplier;
    }

    // Called by trigger when the cursor exits the button hover
    public void NormalSpeed() {
        // When not hvoering anymore, back to original start speed
        horizontalSpeed = tempHspeed;
        verticalSpeed = tempVspeed;
    }

    // Called when the cursor hovers over a button (transitions to highlight effect)
    public void FadeIn() {
        fade.SetBool(fadeBool, true);
    }

    // Called when the cursor exits a button hover (transitions to default effect)
    public void FadeOut() {
        fade.SetBool(fadeBool, false);
    }

    // Disables Console Cursor Control and enables the Mouse Cursor
    public void SwitchingToConsoleInput() {
        GetComponent<EventSystem>().enabled = true;
        GetComponent<CanvasGroup>().alpha = 1;
        if (mouseEventSystem != null) {
            mouseEventSystem.enabled = false;
        }
    }

    // Disables Mouse Control and enables Cursor Control
    public void SwitchingToMouseInput() {
        GetComponent<EventSystem>().enabled = false;
        GetComponent<CanvasGroup>().alpha = 0;
        if (mouseEventSystem != null) {
            mouseEventSystem.enabled = true;
        }
    }

    // FOR DEMO /////////////////////////////////////////////

    public void MouseButton() {
        usingDesktopCursor = true;
    }

    public void ControllerButton() {
        usingDesktopCursor = false;
    }

    public void LoadOnlineDocumentation() {
        Application.OpenURL("https://www.slimui.com/console-cursor-documentation/");
    }
}
