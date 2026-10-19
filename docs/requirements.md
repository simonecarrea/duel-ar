# Requisiti della PoC

## Funzionali

| ID | Requisito | Accettazione |
| --- | --- | --- |
| F01 | Fotocamera AR su Android compatibile | Camera visibile e sessione in tracking sul telefono |
| F02 | Rilevare una carta fisica originale nella library | Nome/posa della specifica reference image rilevati |
| F03 | Mostrare il modello associato | Un'istanza visibile sulla carta, scala fisica plausibile |
| F04 | Gestire perdita e ritorno tracking | Modello nascosto in Limited/None e riattivato senza duplicati |
| F05 | Gestire immagini sconosciute e rimozione | Nessun modello errato; istanze rimosse senza errori |
| F06 | Stato comprensibile all'utente | Istruzioni scansione, permesso negato e AR non supportato (Sprint 1) |

## Non funzionali

| ID | Requisito | Verifica |
| --- | --- | --- |
| N01 | Stesso progetto Mac/Windows | Import dalla stessa revisione con identico Editor e lockfile |
| N02 | Nessun backend per la PoC | Dopo provisioning ARCore, scansione senza rete |
| N03 | Reattività mobile | Target iniziale 30 fps sul telefono scelto, da misurare con Profiler |
| N04 | Repository riproducibile | Meta, manifest, lockfile dopo import, impostazioni e scena versionati |
| N05 | Risorse controllate | Nessun segreto o materiale protetto incluso nello starter |

Fuori ambito: catalogo completo Yu-Gi-Oh!, autenticazione, multiplayer, regole ufficiali, cloud recognition, iOS, pubblicazione sugli store. Il riconoscimento dipende da stampa, illuminazione, riflessi e dispositivo; non ha ancora metriche misurate.
