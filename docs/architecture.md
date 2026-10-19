# Architettura

## Flusso

Fotocamera Android → ARCore → AR Foundation / ARTrackedImageManager → TrackedCardPresenter → CardCatalog → CardDefinition → prefab figlio dell'immagine tracciata.

`Cards` contiene ScriptableObject per configurare l'associazione nome immagine/prefab, senza dipendenze dal tracking. `AR` interpreta gli eventi del provider e possiede il ciclo di vita delle istanze. `UI` registra per ora solo lo stato sessione; messaggi leggibili, autorizzazioni e gestione dispositivi incompatibili restano nello Sprint 1. `Editor` configura l'ambiente e genera asset tramite API Unity, evitando YAML di scena scritto a mano.

## Identità e ciclo di vita

Il nome esatto della reference image è la chiave di catalogo, con confronto case-sensitive. Mantienilo univoco. TrackableId è la chiave delle istanze: due rilevamenti della stessa carta non devono condividere il medesimo GameObject. Il provider decide quali duplicati e quante immagini mobili può effettivamente tracciare.

Con Tracking si crea una sola istanza per immagine nota; gli aggiornamenti riusano la stessa istanza. La posa segue il transform ARTrackedImage perché il modello ne è figlio. Limited/None nascondono il modello; il ritorno a Tracking lo riattiva. Removed distrugge l'istanza. Disable scollega gli eventi e distrugge i modelli; Enable ricostruisce dai trackable correnti. Carta sconosciuta, catalogo mancante e prefab mancante non generano un modello.

## Decisioni Sprint 0

- Android soltanto; Mac e Windows sono host di sviluppo.
- URP mobile, OpenGLES3, un'immagine mobile richiesta, nessuna occlusione né plane detection.
- XR Legacy Input Helpers fornisce il TrackedPoseDriver della camera; nessuna interazione utente richiede ancora Input System.
- Nessun server, account, database, rete o salvataggio foto.
- Nessun riconoscimento universale: library finita per poche carte concordate.
- Nessuna regola di duello o animazione di personaggi protetti.

## Estensione

Aggiungere una carta richiede una reference image e una definizione in catalogo, senza if per ID nel presenter. Un futuro riconoscitore OCR/ML dovrà adattarsi a un contratto indipendente dal provider; introdurlo solo se i test della library mostrano un limite concreto. Assembly definition e test automatici verranno introdotti insieme ai primi test Unity per evitare dipendenze inutili nello starter.
