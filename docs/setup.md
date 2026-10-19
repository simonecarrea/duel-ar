# Configurazione Mac e Windows

## Prerequisiti

Git e Unity Hub; Editor 6000.0.65f1 e moduli Android Build Support, SDK & NDK Tools, OpenJDK forniti da Hub. Usa i tool integrati di Unity, senza percorsi SDK del computer nel repository. Serve uno smartphone Android compatibile ARCore, Google Play Services for AR aggiornato, fotocamera autorizzata, debug USB e collegamento dati. Controlla il modello nell'[elenco Google](https://developers.google.com/ar/devices). Windows potrebbe richiedere il driver USB del produttore.

## Primo import

1. Apri la radice da Hub, lascia scaricare le dipendenze, attendi la compilazione e annota eventuali errori. Non modificare le versioni casualmente per eliminarli.
2. Project Settings > XR Plug-in Management: crea/inizializza le impostazioni se richiesto, seleziona la scheda Android. Esegui **Duel AR > Configure Android**. Se il comando segnala impostazioni mancanti, abilita ARCore manualmente e ripeti.
3. Il comando imposta IL2CPP, ARM64, OpenGLES3, API minima 24, identificativo `com.duelar.poc`, inizializzazione XR all'avvio, Force Text e Visible Meta Files. Seleziona Android in File > Build Profiles e **Switch Platform**. Il target SDK automatico usa il toolchain installato: non equivale a una verifica dei requisiti Play Store.
4. Controlla ARCore abilitato, ARCore **Required**, e risolvi gli errori in XR Plug-in Management > Project Validation. Conferma il consenso fotocamera sul dispositivo; rifiuto e dispositivo incompatibile sono casi da collaudare nello Sprint 1.
5. Esegui **Duel AR > Create Sprint 0 Scene**. Crea `Assets/DuelAR/Generated/`: pipeline mobile URP, renderer con AR Background, library vuota, catalogo vuoto e scena con AR Session, XR Origin, camera tracciata e presenter. Il comando rifiuta di sovrascrivere la scena già esistente.
6. In Graphics e nel livello Quality usato su Android controlla la pipeline generata. Il renderer deve avere **AR Background Renderer Feature**. AR Foundation 6.0.8 include il supporto Render Graph per URP 17; non serve disabilitarlo per questo starter. Controlla la camera a schermo intero e nessun override di pipeline su altri livelli Quality Android.
7. Salva e committa `ProjectSettings`, `.meta`, `Assets/DuelAR/Generated` e `Packages/packages-lock.json` dopo l'import riuscito. Se il processo fallisce a metà, non cancellare asset già modificati: controlla la cartella Generated e completa manualmente le impostazioni.

## Una carta e un modello

1. Seleziona `Generated/CardImages.asset`: aggiungi una reference image autorizzata con nome stabile, ad esempio `poc-card-001`. Usa la foto frontale della specifica stampa fisica. Misura la larghezza della carta in metri e impostala nella library; non inventare una misura per immagini ritagliate.
2. Usa luce uniforme, niente riflessi o custodie lucide. La libreria è un insieme chiuso: non riconosce automaticamente tutte le carte Yu-Gi-Oh! e non fa OCR.
3. Crea una capsula o altro placeholder tuo con altezza circa 0,1 m, pivot alla base e materiale URP; salvalo come prefab. Il prefab deve essere centrato sulla carta e avere scala in metri. Puoi rimuovere il collider per questa PoC.
4. Create > Duel AR > Card Definition: nome immagine esattamente `poc-card-001`, Model Prefab il placeholder. Aggiungi questa definizione nell'array Cards di `Generated/CardCatalog.asset`.
5. Verifica che library e catalogo siano assegnati rispettivamente ad ARTrackedImageManager e TrackedCardPresenter sullo XR Origin. Il manager deve avere Requested Max Number Of Moving Images = 1; è una richiesta al provider, non una garanzia del dispositivo.
6. La scena generata è registrata nelle scene di build. In Build Profiles controlla che un eventuale elenco scene del profilo non la escluda.

## Build e prova

In Build Profiles scegli Android e il dispositivo collegato, abilita Development Build per diagnosi, poi Build And Run in `Builds/Android/`. Registra editor, versioni risolte, modello smartphone, Android, commit e risultato in docs/validation.md. Nell'Editor non aspettarti il tracking reale senza un ambiente di simulazione configurato.

Per ripetere su Windows: pull a Unity chiuso, apri con la stessa release, lascia rigenerare Library. Non rieseguire Create Sprint 0 Scene: apri quella committata. Le immagini private escluse da Git vanno fornite localmente prima della build. Esegui il collaudo della checklist; nessun esito è implicito nel solo completamento del setup.
