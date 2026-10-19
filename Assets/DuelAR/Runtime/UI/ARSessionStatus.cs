using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace DuelAR.UI
{
    public sealed class ARSessionStatus : MonoBehaviour
    {
        private void OnEnable() => ARSession.stateChanged += OnStateChanged;
        private void OnDisable() => ARSession.stateChanged -= OnStateChanged;
        private static void OnStateChanged(ARSessionStateChangedEventArgs args)
            => Debug.Log($"Duel AR session: {args.state}");
    }
}
