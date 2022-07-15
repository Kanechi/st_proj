using UnityEngine;
using TouchScript.Gestures;
using System;

namespace TGS
{
    public class SfInputSystem : MonoBehaviour, IInputProxy
    {
        /// <summary>
        /// 押した際の処理
        /// </summary>
        private PressGesture m_press;
        public PressGesture Press => m_press != null ? m_press : m_press = GetComponent<PressGesture>();

        private ReleaseGesture m_release;
        public ReleaseGesture Release => m_release != null ? m_release : m_release = GetComponent<ReleaseGesture>();

        // true...押した瞬間
        private bool m_onPressFlag = false;

        // true...離した瞬間
        private bool m_onReleaseFlag = false;

        // true...押している
        private bool m_touchFlag = false;

        private bool m_isInit = false;

        public virtual void Init() {

            if (m_isInit == true)
                return;

            m_isInit = true;

            Press.Pressed += OnPressed;
            Release.Released += OnReleased;
        }

        private void OnPressed(object sender, EventArgs e)
        {
            m_onPressFlag = true;

            m_touchFlag = true;
        }

        private void OnReleased(object sender, EventArgs e)
        {
            m_onReleaseFlag = true;

            m_touchFlag = false;
        }

        public virtual Vector3 mousePosition { get { return Input.mousePosition; } }

        public virtual bool touchSupported { get { return Input.touchSupported; } }

        // マルチタッチは無いのでタッチ数は常に１
        public virtual int touchCount { get { return Input.touchCount; } }


        #region keyboard control

        public virtual float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName);
        }

        public virtual bool GetButtonDown(string buttonName)
        {
            return Input.GetButtonDown(buttonName);
        }

        public virtual bool GetButtonUp(string buttonName)
        {
            return Input.GetButtonUp(buttonName);
        }

        public virtual bool GetKey(string name)
        {
            return Input.GetKey(name);
        }

        public virtual bool GetKey(KeyCode keyCode)
        {
            return Input.GetKey(keyCode);
        }

        public virtual bool GetKeyDown(string name)
        {
            return Input.GetKeyDown(name);
        }

        public virtual bool GetKeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }

        public virtual bool GetKeyUp(string name)
        {
            return Input.GetKeyUp(name);
        }

        public virtual bool GetKeyUp(KeyCode keyCode)
        {
            return Input.GetKeyUp(keyCode);
        }

        #endregion

        public virtual bool GetMouseButton(int buttonIndex)
        {
            return Input.GetMouseButton(buttonIndex);
        }

        // 押した際
        public virtual bool GetMouseButtonDown(int buttonIndex)
        {
            bool flag = m_onPressFlag;

            m_onPressFlag = false;

            return flag;
        }

        // 離した際
        public virtual bool GetMouseButtonUp(int buttonIndex)
        {
            bool flag = m_onReleaseFlag;

            m_onReleaseFlag = false;

            return flag;
        }

        #region drag

        public virtual bool IsTouchStarting(int touchIndex)
        {
            return Input.GetTouch(touchIndex).phase == TouchPhase.Began;
        }

        public virtual int GetFingerIdFromTouch(int touchIndex)
        {
            return Input.GetTouch(touchIndex).fingerId;
        }

        #endregion

        public virtual bool IsPointerOverUI()
        {
            return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);
        }
    }
}