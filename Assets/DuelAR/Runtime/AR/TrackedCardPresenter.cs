using System.Collections.Generic;
using DuelAR.Cards;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DuelAR.AR
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    public sealed class TrackedCardPresenter : MonoBehaviour
    {
        [SerializeField] private CardCatalog catalog;
        private ARTrackedImageManager manager;
        private readonly Dictionary<TrackableId, GameObject> instances = new();

        private void Awake() => manager = GetComponent<ARTrackedImageManager>();

        private void OnEnable()
        {
            manager.trackablesChanged.AddListener(OnChanged);
            foreach (var image in manager.trackables) Present(image);
        }

        private void OnDisable()
        {
            manager.trackablesChanged.RemoveListener(OnChanged);
            foreach (var instance in instances.Values)
                if (instance != null) Destroy(instance);
            instances.Clear();
        }

        private void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> changes)
        {
            foreach (var image in changes.added) Present(image);
            foreach (var image in changes.updated) Present(image);
            foreach (var removed in changes.removed)
            {
                if (instances.Remove(removed.Key, out var instance) && instance != null)
                    Destroy(instance);
            }
        }

        private void Present(ARTrackedImage image)
        {
            if (!instances.TryGetValue(image.trackableId, out var instance))
            {
                if (image.trackingState != TrackingState.Tracking || catalog == null ||
                    !catalog.TryGet(image.referenceImage.name, out var card) || card.ModelPrefab == null)
                    return;
                // Each physical tracked image owns its own model, even for the same card type.
                instance = Instantiate(card.ModelPrefab, image.transform, false);
                instances.Add(image.trackableId, instance);
            }
            if (instance != null)
                instance.SetActive(image.trackingState == TrackingState.Tracking);
        }
    }
}
