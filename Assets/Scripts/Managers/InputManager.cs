using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[DefaultExecutionOrder(-1)]

public class InputManager : SingletonPersistent<InputManager>
{
    #region EVENT DELEGATE
    public delegate void TouchPositionDelegate(Vector2 position);
    public event TouchPositionDelegate OnTouch;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //Cek apakah player menyentuh layar dan menjalankan event
        if(Touch.activeFingers.Count == 1)
        {
            if(Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began)
                OnTouch?.Invoke(Touch.activeTouches[0].screenPosition);
        }
    }
}
