using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.MagicLeap;
        [System.Serializable]
public class MyVectorEvent : UnityEvent<Vector3>
{
}

     [System.Serializable]
public class MyFloatEvent : UnityEvent<float>
{
}
namespace MagicLeap
{
    [RequireComponent(typeof(ControllerConnectionHandler))]
    public class BasicControllerBindings : MonoBehaviour
    {
        [SerializeField]
        private ControllerConnectionHandler _controllerConnectionHandler;
    
        [Header("Trigger")]
        public UnityEvent OnTriggerDown;
        public UnityEvent OnTriggerUp;
        public UnityEvent OnTriggerUpdate;

        public MyFloatEvent OnTriggerValue;

        [Header("Bumper")]
        public UnityEvent OnBumperDown;
        public UnityEvent OnBumperUp;

        [Header("Touchpad")]
        public UnityEvent OnTouchpadClick;
        public UnityEvent OnTouchpadDown;
        public UnityEvent OnTouchpadUp;
        public UnityEvent OnTouchpadUpdate;


        public MyVectorEvent OnTouchpadVector;


        [Header("Touchpad Directions")]
        public UnityEvent OnTouchpadRightClick;
        public UnityEvent OnTouchpadLeftClick;
        public UnityEvent OnTouchpadUpClick;
        public UnityEvent OnTouchpadDownClick;

        [Header("Home")]
        public UnityEvent OnHomeTapDown;
        public UnityEvent OnHomeTapUp;
        public UnityEvent OnHomeClickDown;
        public UnityEvent OnHomeClickUp;

        private int _lastLEDindex = -1;
        private float _lastTriggerValue = 0f;
        private Vector3 _touchPosition;
        private bool _isTouching;
        private bool _touchPadClicked;
        private const float TRIGGER_DOWN_MIN_VALUE = 0.2f;

        // UpdateLED - Constants
        private const float HALF_HOUR_IN_DEGREES = 15.0f;
        private const float DEGREES_PER_HOUR = 12.0f / 360.0f;

        private const int MIN_LED_INDEX = (int)(MLInputControllerFeedbackPatternLED.Clock12);
        private const int MAX_LED_INDEX = (int)(MLInputControllerFeedbackPatternLED.Clock6And12);
        private const int LED_INDEX_DELTA = MAX_LED_INDEX - MIN_LED_INDEX;

        void Start()
        {

            MLInput.OnControllerButtonUp += HandleOnButtonUp;
            MLInput.OnControllerButtonDown += HandleOnButtonDown;
            MLInput.OnTriggerDown += HandleOnTriggerDown;
            MLInput.OnTriggerUp += HandleOnTriggerUp;
        }

        void Update()
        {
            if (!_controllerConnectionHandler.IsControllerValid()) return;

            // TRIGGER
            float triggerValue = ActiveController.TriggerValue;
            if (triggerValue != _lastTriggerValue)
            {
                _lastTriggerValue = triggerValue;
                OnTriggerUpdate.Invoke();
                OnTriggerValue.Invoke(triggerValue);
            }


            // TOUCHPAD
            if (ActiveController.Touch1Active)
            {
                _touchPosition = ActiveController.Touch1PosAndForce;
                if (!_isTouching)
                {
                    OnTouchpadDown.Invoke();
                }
                _isTouching = true;

                OnTouchpadVector.Invoke( _touchPosition );
                // HARD CLICK
                if (!_touchPadClicked && _touchPosition.z > 0.75f)
                {
                    _touchPadClicked = true;
                    OnTouchpadClick.Invoke();
                    StartCoroutine(TouchClick());

                    // QUADRANTS

                    if (_touchPosition.x > 0.25f && _touchPosition.x > Mathf.Abs(_touchPosition.y))
                    {
                        // Right
                        OnTouchpadRightClick.Invoke();
                    }
                    else if (_touchPosition.x < -0.25f && _touchPosition.x < -Mathf.Abs(_touchPosition.y))
                    {
                        // Left
                        OnTouchpadLeftClick.Invoke();
                    }
                    else if (_touchPosition.y < -0.25f && _touchPosition.y < -Mathf.Abs(_touchPosition.x))
                    {
                        // Bottom
                        OnTouchpadDownClick.Invoke();
                    }
                    else if (_touchPosition.y > 0.25f && _touchPosition.y > -Mathf.Abs(_touchPosition.x))
                    {
                        // Up
                        OnTouchpadUpClick.Invoke();

                    }
                }
            }
            else
            {
                if (!_isTouching)
                {
                    OnTouchpadUp.Invoke();
                }
            }
        }

        IEnumerator TouchClick()
        {
            ActiveController.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.ForceDown, MLInputControllerFeedbackIntensity.Medium);
            yield return new WaitForSeconds(0.25f);
            ActiveController.StopFeedbackPatternVibe();
            _touchPadClicked = false;

        }

        void OnDestroy()
        {
            if (MLInput.IsStarted)
            {
                MLInput.OnTriggerDown -= HandleOnTriggerDown;
                MLInput.OnControllerButtonDown -= HandleOnButtonDown;
                MLInput.OnControllerButtonUp -= HandleOnButtonUp;
            }
        }

        private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
        {
            if (IsController(controllerId))
            {
                if (button == MLInputControllerButton.Bumper)
                {
                    // Demonstrate haptics using callbacks.
                    ActiveController.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.ForceDown, MLInputControllerFeedbackIntensity.Medium);
                    // // Toggle UseCFUIDTransforms
                    // controller.UseCFUIDTransforms = !controller.UseCFUIDTransforms;

                    OnBumperDown.Invoke();

                }
            }
            else if (button == MLInputControllerButton.HomeTap)
            {
                // Demonstrate haptics using callbacks.
                ActiveController.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.ForceDown, MLInputControllerFeedbackIntensity.Medium);
                // // Toggle UseCFUIDTransforms
                // controller.UseCFUIDTransforms = !controller.UseCFUIDTransforms;

                OnHomeTapDown.Invoke();

            }
        }

        private void HandleOnButtonUp(byte controllerId, MLInputControllerButton button)
        {
            if (IsController(controllerId))
            {
                ActiveController.StopFeedbackPatternVibe();

                if (button == MLInputControllerButton.Bumper)
                {
                    OnBumperUp.Invoke();
                }
                else if (button == MLInputControllerButton.HomeTap)
                {
                    OnHomeTapUp.Invoke();
                }
            }
        }

        private void HandleOnTriggerDown(byte controllerId, float value)
        {
            if (IsController(controllerId))
            {
                MLInputControllerFeedbackIntensity intensity = (MLInputControllerFeedbackIntensity)((int)(value * 2.0f));
                ActiveController.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Buzz, intensity);

                OnTriggerDown.Invoke();

            }
        }

        private void HandleOnTriggerUp(byte controllerId, float value)
        {
            if (IsController(controllerId))
            {
                ActiveController.StopFeedbackPatternVibe();
                OnTriggerUp.Invoke();
            }

        }

        bool IsController(byte controllerId)
        {
            MLInputController controller = ActiveController;
            if (controller != null && controller.Id == controllerId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        MLInputController ActiveController
        {
            get { return _controllerConnectionHandler.ConnectedController; }
        }

        public float CurrentTriggerValue
        {
            get { return _lastTriggerValue; }
        }

        /*
        private void UpdateLED()
        {
            if (!_controllerConnectionHandler.IsControllerValid())
            {
                return;
            }
            MLInputController controller = _controllerConnectionHandler.ConnectedController;
            if (controller.Touch1Active)
            {
                // Get angle of touchpad position.
                float angle = -Vector2.SignedAngle(Vector2.up, controller.Touch1PosAndForce);
                if (angle < 0.0f)
                {
                    angle += 360.0f;
                }
                // Get the correct hour and map it to [0,6]
                int index = (int)((angle + HALF_HOUR_IN_DEGREES) * DEGREES_PER_HOUR) % LED_INDEX_DELTA;
                // Pass from hour to MLInputControllerFeedbackPatternLED index  [0,6] -> [MAX_LED_INDEX, MIN_LED_INDEX + 1, ..., MAX_LED_INDEX - 1]
                index = (MAX_LED_INDEX + index > MAX_LED_INDEX) ? MIN_LED_INDEX + index : MAX_LED_INDEX;
                if (_lastLEDindex != index)
                {
                    // a duration of 0 means leave it on indefinitely
                    controller.StartFeedbackPatternLED((MLInputControllerFeedbackPatternLED)index, MLInputControllerFeedbackColorLED.BrightCosmicPurple, 0);
                    _lastLEDindex = index;
                }
            }
            else if (_lastLEDindex != -1)
            {
                controller.StopFeedbackPatternLED();
                _lastLEDindex = -1;
            }
        }
    */
    }
}