using System;
using UnityEngine;

namespace DuelAR.Cards
{
    [CreateAssetMenu(menuName = "Duel AR/Card Catalog")]
    public sealed class CardCatalog : ScriptableObject
    {
        [SerializeField] private CardDefinition[] cards = Array.Empty<CardDefinition>();

        public bool TryGet(string imageName, out CardDefinition definition)
        {
            foreach (var card in cards)
            {
                if (card != null && string.Equals(card.ReferenceImageName, imageName, StringComparison.Ordinal))
                {
                    definition = card;
                    return true;
                }
            }
            definition = null;
            return false;
        }

        private void OnValidate()
        {
            var names = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            foreach (var card in cards)
            {
                if (card == null || string.IsNullOrWhiteSpace(card.ReferenceImageName) || card.ModelPrefab == null)
                    Debug.LogWarning("Catalogo: carta, nome immagine o prefab mancante.", this);
                else if (!names.Add(card.ReferenceImageName))
                    Debug.LogWarning($"Nome immagine duplicato: {card.ReferenceImageName}", this);
            }
        }
    }
}
