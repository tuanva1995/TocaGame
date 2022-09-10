using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.EventSystems;

namespace BitBenderGames
{
    [RequireComponent(typeof(TouchInputController))]
    [RequireComponent(typeof(MobileTouchCamera))]
    public class TouchInputHelper : Singleton<TouchInputHelper>
    {
        private List<GameObject> touchBlocker = new List<GameObject>();
        private TouchInputController touchInputController;
        private MobileTouchCamera mobileTouchCamera;

        private PointerEventData pointer;
        List<RaycastResult> hitsUI = new List<RaycastResult>();
        private RaycastHit2D[] hits;
        private static Vector3 mouseDownPos;
        private static float touchTimeStamp;
        /// <summary>
        ///  Call in if (Input.GetMouseButtonUp(0)) {} to recognize click action from hold action
        /// </summary>
        public static bool IsClick
        {
            get
            {
                return Time.time - touchTimeStamp < 0.2f && Vector3.Distance(mouseDownPos, Input.mousePosition) < 5f;
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchTimeStamp = Time.time;
                mouseDownPos = Input.mousePosition;
            }
        }

        protected override void OnRegistration()
        {
            touchInputController = GetComponent<TouchInputController>();
            mobileTouchCamera = GetComponent<MobileTouchCamera>();
        }

        /// <summary>
        /// If something is meant to block camera movement when being active, 
        /// call this function when activated and pass that gameObject as param
        /// </summary>
        /// <param name="go"></param>
        public void SubscribeTouchBlocker(GameObject go)
        {
            if (!touchBlocker.Contains(go))
            {
                touchBlocker.Add(go);
            }
            touchInputController.enabled = touchBlocker.Count == 0;
            mobileTouchCamera.enabled = touchBlocker.Count == 0;
        }
        /// <summary>
        /// If something is meant to block camera movement when being active,
        /// call this function when it's deactivated and pass that gameObject as param
        /// </summary>
        /// <param name="go"></param>
        public void UnsubscribeTouchBlocker(GameObject go)
        {
            touchBlocker.Remove(go);
            touchInputController.enabled = touchBlocker.Count == 0;
            mobileTouchCamera.enabled = touchBlocker.Count == 0;
        }

        //public RaycastHit2D[] GetAllRaycastHit()
        //{
        //    Vector3 worldTouchPos = GridBuildSystem.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    worldTouchPos.z = 0;

        //    hits = Physics2D.RaycastAll(worldTouchPos, Vector2.zero, float.PositiveInfinity);
        //    return hits;
        //}

        //public List<RaycastResult> GetRaycastUIHit()
        //{
        //    if (pointer == null)
        //    {
        //        pointer = new PointerEventData(EventSystem.current);
        //    }
        //    pointer.position = Input.mousePosition;
        //    hitsUI.Clear();
        //    EventSystem.current.RaycastAll(pointer, hitsUI);
        //    return hitsUI;
        //}
    }
}


