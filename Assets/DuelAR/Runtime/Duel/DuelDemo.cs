using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DuelAR.Duel
{
    // Local training encounter. This is deliberately separate from a full card rules engine.
    public sealed class DuelDemo : MonoBehaviour
    {
        [SerializeField] private ARTrackedImageManager manager;
        [SerializeField] private GameObject trainingMagePrefab;
        [SerializeField] private Material attackMaterial;
        private GameObject trainingMage;
        private Transform dragon, mage;
        private readonly TrainingBattle battle = new TrainingBattle();
        private bool busy, training;
        private string message = "Inquadra il Drago Bianco e il segnaposto del mago.";
        private const string DragonImage = "poc-card-001", MageImage = "demo-dark-magician";
        private Coroutine attack;
        private LineRenderer beam;
        private Material beamMaterial;

        private void Update()
        {
            dragon = FindModel(DragonImage);
            var trackedMage = FindModel(MageImage);
            if (trackedMage != null)
            {
                if (trainingMage != null) trainingMage.SetActive(false);
                mage = trackedMage;
            }
            else
            {
                if (training && trainingMage == null && trainingMagePrefab != null)
                    trainingMage = Instantiate(trainingMagePrefab);
                if (trainingMage != null)
                {
                    trainingMage.SetActive(training && dragon != null);
                    if (dragon != null)
                    {
                        trainingMage.transform.position = dragon.position + dragon.forward * .30f;
                        trainingMage.transform.rotation = Quaternion.LookRotation(-dragon.forward, dragon.up);
                    }
                }
                mage = trainingMage != null && trainingMage.activeSelf ? trainingMage.transform : null;
            }
        }

        private Transform FindModel(string imageName)
        {
            foreach (var image in manager.trackables)
            {
                if (image.referenceImage.name != imageName || image.trackingState != TrackingState.Tracking)
                    continue;
                for (int i = 0; i < image.transform.childCount; i++)
                {
                    var child = image.transform.GetChild(i);
                    if (child.gameObject.activeInHierarchy) return child;
                }
            }
            return null;
        }

        private void OnGUI()
        {
            float scale = Screen.width / 720f;
            var previous = GUI.matrix;
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1));
            float height = Screen.height / scale;
            var style = new GUIStyle(GUI.skin.label) { fontSize = 24, wordWrap = true };
            var button = new GUIStyle(GUI.skin.button) { fontSize = 25 };
            float top = height - 390;
            GUI.Box(new Rect(12, top, 696, 378), "");
            GUI.Label(new Rect(28, top + 12, 665, 40), "DUEL AR · ALLENAMENTO LOCALE", style);
            GUI.Label(new Rect(28, top + 56, 665, 62),
                $"Drago Bianco  LP {battle.DragonLife} · ATK 3000\nMago Nero     LP {battle.MageLife} · ATK 2500", style);
            GUI.Label(new Rect(28, top + 125, 665, 70), message, style);
            GUI.enabled = !busy && dragon != null && mage != null && battle.DragonLife > 0 && battle.MageLife > 0;
            if (GUI.Button(new Rect(28, top + 205, 665, 60), battle.DragonTurn ? "Attacca con Drago Bianco" : "Attacca con Mago Nero", button))
                attack = StartCoroutine(Attack());
            GUI.enabled = !busy;
            if (GUI.Button(new Rect(28, top + 280, 430, 65), training ? "Disattiva mago di prova" : "Evoca mago di prova", button))
                training = !training;
            if (GUI.Button(new Rect(472, top + 280, 221, 65), "Ricomincia", button)) ResetDuel();
            GUI.enabled = true;
            GUI.matrix = previous;
        }

        private IEnumerator Attack()
        {
            busy = true;
            bool fromDragon = battle.DragonTurn;
            Transform source = fromDragon ? dragon : mage, target = fromDragon ? mage : dragon;
            message = fromDragon ? "Drago Bianco: attacco!" : "Mago Nero: magia oscura!";
            var effect = new GameObject("Attack beam");
            beam = effect.AddComponent<LineRenderer>();
            beamMaterial = new Material(attackMaterial);
            beamMaterial.color = fromDragon ? Color.cyan : new Color(.8f, .15f, 1);
            beam.sharedMaterial = beamMaterial;
            beam.positionCount = 2;
            beam.startWidth = .009f; beam.endWidth = .002f;
            float elapsed = 0;
            while (elapsed < 1.2f)
            {
                // Tracking loss cancels rather than applying invisible damage.
                if (source == null || target == null || !source.gameObject.activeInHierarchy ||
                    !target.gameObject.activeInHierarchy || dragon == null || mage == null)
                {
                    message = "Attacco annullato: inquadra entrambi i mostri.";
                    FinishEffect(); busy = false; attack = null; yield break;
                }
                elapsed += Time.deltaTime;
                var start = source.position + Vector3.up * .09f;
                var end = target.position + Vector3.up * .09f;
                beam.SetPosition(0, start);
                beam.SetPosition(1, Vector3.Lerp(start, end, Mathf.Clamp01(elapsed / .65f)));
                yield return null;
            }
            // Both in attack position: lower ATK loses 500 LP, even when it initiates.
            // Training keeps monsters on the table instead of destroying them.
            int damage = battle.ResolveAttack();
            message = battle.DragonLife == 0 || battle.MageLife == 0
                ? (battle.MageLife == 0 ? "Vince Drago Bianco!" : "Vince Mago Nero!")
                : $"Mago Nero perde {damage} LP. Tocca a {(battle.DragonTurn ? "Drago Bianco" : "Mago Nero")}.";
            FinishEffect(); busy = false; attack = null;
        }

        private void FinishEffect()
        {
            if (beam != null) Destroy(beam.gameObject);
            if (beamMaterial != null) Destroy(beamMaterial);
        }
        private void ResetDuel()
        {
            battle.Reset();
            message = "Nuovo allenamento. Inquadra i mostri e attacca.";
        }
        private void OnDisable()
        {
            if (attack != null) StopCoroutine(attack);
            FinishEffect(); busy = false; attack = null;
            if (trainingMage != null) Destroy(trainingMage);
        }
    }
}
