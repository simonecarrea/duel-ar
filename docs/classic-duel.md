# Primo duello: Mago Nero contro Drago Bianco

Demo locale di allenamento su un telefono; non sincronizza due giocatori.

## Prova rapida

Apri Duel AR e inquadra l’immagine del Drago Bianco già configurata. Tocca **Evoca mago di prova**: un mago geometrico originale compare a 25 cm dal drago. Inquadra entrambi e premi **Attacca con Drago Bianco**. Un raggio azzurro raggiunge il mago; i suoi LP passano da 4.000 a 3.500. Il turno successivo mostra **Attacca con Mago Nero**, con raggio viola.

Entrambi sono considerati in posizione di attacco: 3.000 contro 2.500 ATK. Il mago perde 500 LP anche quando attacca il drago. Per consentire prove ripetute, i mostri restano sul tavolo invece di essere distrutti. A zero LP gli attacchi si bloccano. **Ricomincia** ripristina LP e turno. La perdita del tracking durante il raggio annulla l’attacco senza applicare danni.

## Seconda immagine indipendente

Apri o stampa `Assets/DuelAR/Art/MageMarker.png`, immagine originale di prova a rettangoli colorati. La library usa il nome `demo-dark-magician`, larghezza 59 mm e altezza 86 mm: visualizzala a queste dimensioni per una scala coerente. Quando viene riconosciuta, il mago segue questa immagine e sostituisce quello della modalità prova. La vera carta Mago Nero non è ancora registrata.

## Setup ripetibile

Apri la scena esistente `Assets/DuelAR/Generated/DuelAR.unity`, attendi la compilazione, poi esegui **Duel AR > Prepare Classic Duel**. Il comando conserva gli asset esistenti, aggiunge la definizione del mago al catalogo e richiede due immagini mobili al provider. Esegue controlli sulle regole: danni, turni, fine duello, attacco dopo fine e reset. Il supporto effettivo al tracking simultaneo va provato sul dispositivo.

Il modello dettagliato del Drago Bianco rimane una risorsa locale esclusa da Git. Il mago è un segnaposto creato con primitive; può essere sostituito successivamente nel prefab della definizione della carta.
