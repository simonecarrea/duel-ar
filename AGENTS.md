# Duel AR — istruzioni per gli agenti

## Obiettivo e limiti

PoC Unity 6 LTS, Android ARCore, carta fisica originale → immagine di riferimento nota → modello AR. Nessun backend, multiplayer o motore completo di regole nello Sprint 0. Documentazione per l'utente in italiano; nomi C# in inglese.

## Prima di modificare

Leggi README, docs/setup.md, docs/architecture.md e docs/validation.md. Ispeziona branch, stato Git e file esistenti. Preserva il lavoro dell'utente. Usa branch e PR. Non dichiarare importazione, test, build o prova dispositivo riusciti senza averli eseguiti: riporta i comandi e le limitazioni.

## Unity

Mantieni la versione in ProjectSettings/ProjectVersion.txt e i pacchetti in Packages/manifest.json; niente aggiornamenti silenziosi. Mantieni i .meta con gli asset e preserva i GUID. Non scrivere Library, Temp, UserSettings o file di progetto IDE in Git. Dopo il primo import committa packages-lock.json, impostazioni e asset generati da Unity. Non inventare un lockfile o dichiarare completa la configurazione generata prima del primo import.

Codice runtime in Assets/DuelAR/Runtime, strumenti in Assets/DuelAR/Editor. Dipendenze: Cards non dipende da AR; AR usa Cards; UI mostra stato. Evita allocazioni inutili nel tracking. Gestisci Tracking/Limited/None, rimozione, disable/enable, più TrackableId e carte sconosciute. Test significativi per cambiamenti di comportamento quando Unity Test Framework è disponibile; niente test fittizi che sostituiscono la prova Android.

## Materiali e segreti

Non scaricare né pubblicare immagini di carte, modelli protetti o risorse estratte da giochi. Non includere token, credenziali, keystore o path locali. Usa placeholder originali; foto locali in Assets/PrivateCardReferences sono escluse da Git. Non aggiungere file LFS senza concordare una strategia per i binari grandi.

## Consegna

Controlla diff e whitespace; esegui verifiche disponibili. La PR deve indicare cosa cambia, prove realmente eseguite e passi ancora da fare nell'Editor/su Android. Non unire la PR senza richiesta.
