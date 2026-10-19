using UnityEngine;

namespace DuelAR.Cards
{
    [CreateAssetMenu(menuName = "Duel AR/Card Definition")]
    public sealed class CardDefinition : ScriptableObject
    {
        [SerializeField] private string referenceImageName;
        [SerializeField] private GameObject modelPrefab;
        public string ReferenceImageName => referenceImageName;
        public GameObject ModelPrefab => modelPrefab;
    }
}
