using System;
using System.IO;
using DuelAR.Cards;
using DuelAR.Duel;
using UnityEditor;
using UnityEditor.XR.ARSubsystems;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DuelAR.Editor
{
    public static class DuelDemoSetup
    {
        private const string Folder = "Assets/DuelAR/Generated";
        [MenuItem("Duel AR/Prepare Classic Duel")]
        public static void Prepare()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != Folder + "/DuelAR.unity")
                throw new InvalidOperationException("Apri la scena DuelAR esistente prima di preparare il duello.");
            var manager = UnityEngine.Object.FindFirstObjectByType<ARTrackedImageManager>();
            if (manager == null) throw new InvalidOperationException("ARTrackedImageManager mancante.");
            string prefabPath = Folder + "/TrainingMage.prefab";
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                var root = new GameObject("Mago Nero - segnaposto originale");
                var purple = Material("MagePurple", new Color(.35f,.06f,.65f));
                var dark = Material("MageDark", new Color(.12f,.03f,.22f));
                var skin = Material("MageFace", new Color(.9f,.72f,.55f));
                var gold = Material("MageStaff", new Color(.25f,.9f,.6f));
                Part(root, "Veste", PrimitiveType.Capsule, new Vector3(0,.055f,0), new Vector3(.045f,.05f,.04f), purple);
                Part(root, "Volto", PrimitiveType.Sphere, new Vector3(0,.12f,0), Vector3.one*.031f, skin);
                Part(root, "Cappello", PrimitiveType.Capsule, new Vector3(0,.15f,0), new Vector3(.034f,.025f,.034f), dark);
                Part(root, "Braccio", PrimitiveType.Capsule, new Vector3(.03f,.085f,0), new Vector3(.015f,.025f,.015f), purple);
                Part(root, "Bastone", PrimitiveType.Cylinder, new Vector3(.05f,.08f,0), new Vector3(.006f,.075f,.006f), gold);
                Part(root, "Gemma", PrimitiveType.Sphere, new Vector3(.05f,.16f,0), Vector3.one*.025f, gold);
                prefab = PrefabUtility.SaveAsPrefabAsset(root,prefabPath);
                UnityEngine.Object.DestroyImmediate(root);
            }
            var definition = AssetDatabase.LoadAssetAtPath<CardDefinition>(Folder + "/DarkMagicianCard.asset");
            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<CardDefinition>();
                AssetDatabase.CreateAsset(definition, Folder + "/DarkMagicianCard.asset");
            }
            var cardData = new SerializedObject(definition);
            cardData.FindProperty("referenceImageName").stringValue = "demo-dark-magician";
            cardData.FindProperty("modelPrefab").objectReferenceValue = prefab;
            cardData.ApplyModifiedPropertiesWithoutUndo();
            var catalog = AssetDatabase.LoadAssetAtPath<CardCatalog>(Folder + "/CardCatalog.asset");
            var catalogData = new SerializedObject(catalog);
            var cards = catalogData.FindProperty("cards");
            bool found = false;
            for (int i=0;i<cards.arraySize;i++) if (cards.GetArrayElementAtIndex(i).objectReferenceValue==definition) found=true;
            if (!found) { cards.InsertArrayElementAtIndex(cards.arraySize); cards.GetArrayElementAtIndex(cards.arraySize-1).objectReferenceValue=definition; }
            catalogData.ApplyModifiedPropertiesWithoutUndo();
            var library = AssetDatabase.LoadAssetAtPath<XRReferenceImageLibrary>(Folder+"/CardImages.asset");
            bool imageExists=false;
            for(int i=0;i<library.count;i++) if(library[i].name=="demo-dark-magician") imageExists=true;
            if(!imageExists)
            {
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/DuelAR/Art/MageMarker.png");
                if(texture==null) throw new InvalidOperationException("MageMarker.png mancante.");
                library.Add(); int index=library.count-1;
                library.SetTexture(index,texture,false); library.SetName(index,"demo-dark-magician");
                library.SetSpecifySize(index,true); library.SetSize(index,new Vector2(.059f,.086f));
                EditorUtility.SetDirty(library);
            }
            manager.requestedMaxNumberOfMovingImages=2;
            EditorUtility.SetDirty(manager);
            var demo=manager.GetComponent<DuelDemo>();
            if(demo==null) demo=manager.gameObject.AddComponent<DuelDemo>();
            var data=new SerializedObject(demo);
            data.FindProperty("manager").objectReferenceValue=manager;
            data.FindProperty("trainingMagePrefab").objectReferenceValue=prefab;
            var beamMaterial = AssetDatabase.LoadAssetAtPath<Material>(Folder+"/AttackBeam.mat");
            if(beamMaterial==null)
            {
                beamMaterial=new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                AssetDatabase.CreateAsset(beamMaterial,Folder+"/AttackBeam.mat");
            }
            data.FindProperty("attackMaterial").objectReferenceValue=beamMaterial;
            data.ApplyModifiedPropertiesWithoutUndo();
            EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            var battle = new TrainingBattle();
            if (battle.ResolveAttack()!=500 || battle.MageLife!=3500 || battle.DragonTurn)
                throw new InvalidOperationException("Test danno o turno del drago fallito.");
            battle.ResolveAttack();
            if (battle.MageLife!=3000 || battle.DragonLife!=4000 || !battle.DragonTurn)
                throw new InvalidOperationException("Test attacco del mago fallito.");
            for(int i=0;i<6;i++) battle.ResolveAttack();
            if(!battle.Finished || battle.MageLife!=0) throw new InvalidOperationException("Test fine duello fallito.");
            bool rejected=false;
            try { battle.ResolveAttack(); } catch(InvalidOperationException) { rejected=true; }
            if(!rejected) throw new InvalidOperationException("Attacco consentito dopo fine duello.");
            battle.Reset();
            if(battle.Finished || battle.MageLife!=4000 || !battle.DragonTurn) throw new InvalidOperationException("Test reset fallito.");
            Debug.Log("TrainingBattle PASS: danni, alternanza turni, fine duello, blocco attacchi e reset.");
            Debug.Log("Duello classico pronto: 2 immagini, mago provvisorio, allenamento locale e attacchi.");
        }
        private static Material Material(string name, Color color)
        {
            string path=Folder+"/"+name+".mat";
            var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(mat==null) { mat=new Material(Shader.Find("Universal Render Pipeline/Simple Lit")); mat.color=color; AssetDatabase.CreateAsset(mat,path); }
            return mat;
        }
        private static void Part(GameObject root,string name,PrimitiveType type,Vector3 position,Vector3 scale,Material material)
        {
            var part=GameObject.CreatePrimitive(type); part.name=name;
            part.transform.SetParent(root.transform,false); part.transform.localPosition=position; part.transform.localScale=scale;
            part.GetComponent<Renderer>().sharedMaterial=material;
            UnityEngine.Object.DestroyImmediate(part.GetComponent<Collider>());
        }
    }
}
