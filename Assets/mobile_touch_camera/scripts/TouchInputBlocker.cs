using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitBenderGames
{
    public class TouchInputBlocker : MonoBehaviour
    {
        private void OnEnable()
        {
            TouchInputHelper.Instance.SubscribeTouchBlocker(gameObject);
        }
        private void OnDisable()
        {
            TouchInputHelper.Instance.UnsubscribeTouchBlocker(gameObject);
        }
    }
}

