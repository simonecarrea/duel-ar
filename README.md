# Duel AR — Sprint 0

Proof of concept Android: inquadrare una carta Yu-Gi-Oh! originale e mostrare un modello 3D associato, usando Unity, AR Foundation e ARCore. Sviluppo condiviso fra Mac e Windows; nessun backend.

**Stato:** starter sorgente. Unity e uno smartphone non erano disponibili durante la preparazione: importazione pacchetti, compilazione, build Android e tracking non sono verificati. La scena e gli asset di configurazione si generano nell'Editor; non sono già presenti. Nessuna immagine di carte o modello protetto è incluso.

## Avvio

1. Clona il repository su entrambi i computer. Installa da Unity Hub **Unity 6000.0.65f1**, con Android Build Support, SDK & NDK Tools e OpenJDK.
2. In Hub scegli **Add project from disk** e seleziona la radice del repository, dove si trovano `Assets`, `Packages` e `ProjectSettings`. Non creare un altro progetto sopra questi file.
3. Attendi la risoluzione dei pacchetti e controlla la Console.
4. Segui [setup completo](docs/setup.md): inizializza XR Plug-in Management, esegui **Duel AR > Configure Android**, poi **Duel AR > Create Sprint 0 Scene**.
5. Completa la library immagini e il catalogo prima della prova su Android. Senza immagini la scena non può riconoscere carte.

## Versioni fissate

| Componente | Versione |
| --- | --- |
| Unity 6.0 LTS | 6000.0.65f1 |
| AR Foundation / ARCore | 6.0.8 / 6.0.8 |
| Universal RP | 17.0.4 |
| XR Plug-in Management | 4.5.0 |
| XR Legacy Input Helpers | 2.1.11 |

Baseline esplicita, non indicazione della release più recente. Stessa versione su Mac e Windows; aggiornamenti in PR dedicata. `Packages/packages-lock.json` sarà generato dal primo import Unity riuscito e va committato insieme agli asset generati e alle impostazioni. La risoluzione delle dipendenze transitive non è ancora verificata.

## Struttura

- `Assets/DuelAR/Runtime/Cards`: definizioni e catalogo carte.
- `Assets/DuelAR/Runtime/AR`: associazione immagini riconosciute e modelli.
- `Assets/DuelAR/Runtime/UI`: log dello stato della sessione AR.
- `Assets/DuelAR/Editor`: generazione scena URP e configurazione Android.
- `docs`: [architettura](docs/architecture.md), [requisiti](docs/requirements.md), [roadmap](docs/roadmap.md), [validazione](docs/validation.md).
- `prompts`: [prima attività Codex](prompts/sprint-0.md) e [Sprint 1](prompts/sprint-1.md).

## Lavorare su due computer

Prima di cambiare computer: salva Unity, controlla il diff, commit e push. Sull'altro computer chiudi Unity prima del pull, poi riapri con la stessa release. Versiona sempre `.meta`, scene, prefab, `ProjectSettings`, manifest e lockfile. Non versionare `Library`, cache, SDK, build o chiavi di firma. Imposta **Visible Meta Files** e **Force Text** (lo fa anche Configure Android). Evita modifiche simultanee alla stessa scena; non rigenerare GUID per risolvere conflitti.

## Controllo statico

Esegui `python3 scripts/check_repository.py` (su Windows anche `py -3 scripts/check_repository.py`) e `git diff --check`. Il controllo verifica JSON, GUID/meta e link locali; non compila C# e non sostituisce Unity.

## Materiali

Usare carte fisiche originali per l'esperimento non fornisce automaticamente una licenza per distribuire illustrazioni o personaggi. Il progetto non è affiliato a Konami. Per lo starter usa modelli geometrici creati da te; conserva eventuali foto sperimentali locali in `Assets/PrivateCardReferences/` (ignorata da Git). La library può contenere riferimenti locali mancanti su un altro computer: ricreali localmente, oppure condividi solo immagini per cui hai il permesso. Una build potrebbe incorporare dati derivati dalle immagini: non pubblicarla senza verificare i materiali inclusi.

## Fonti tecniche

- [Release Unity scelta](https://unity.com/releases/editor/whats-new/6000.0.65f1)
- [AR Foundation per Unity 6.0](https://docs.unity.com/en-us/engine/6000.0/manual/packages-list/packages-all/pack-safe/com-unity-xr-arfoundation)
- [Configurazione ARCore](https://docs.unity3d.com/Packages/com.unity.xr.arcore@6.0/manual/project-configuration-arcore.html)
- [API tracking AR Foundation 6](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.0/api/UnityEngine.XR.ARFoundation.ARTrackedImageManager.html)
