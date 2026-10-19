# Registro di validazione

## Preparazione starter

Ambiente: Mac con Git e Python; Unity Editor e smartphone non disponibili. Eseguiti `python3 scripts/check_repository.py` (PASS: JSON, 19 GUID univoci, coppie asset/meta e link Markdown locali), `git diff --check` e `git check-ignore` sui percorsi cache/build/risorse private. Nessuna compilazione C#, import pacchetti, test Unity, APK o prova AR eseguiti. Le API sono state confrontate con documentazione/sorgenti dei pacchetti, ma questo non sostituisce l'import e la build.

## Checklist da eseguire

- [ ] Import pacchetti e compilazione C# senza errori.
- [ ] Configure Android abilita ARCore e persiste le impostazioni.
- [ ] Creazione scena riuscita, secondo tentativo non sovrascrive.
- [ ] Camera background URP visibile, posa camera segue il telefono.
- [ ] Build IL2CPP ARM64 e installazione Android riuscite.
- [ ] Prima carta nota: un modello, nessuna duplicazione negli aggiornamenti.
- [ ] Carta ruotata/spostata: posa e scala coerenti.
- [ ] Tracking perso o Limited: modello nascosto; ritorno: riattivato.
- [ ] Rimozione e disable/enable presenter: nessuna istanza residua.
- [ ] Carta non in library o nome senza voce catalogo: nessun modello.
- [ ] Permesso camera negato/revocato e dispositivo incompatibile: annotare comportamento (UI da implementare).
- [ ] Background/resume dell'app: annotare recupero sessione.
- [ ] Prova senza rete dopo installazione/provisioning.
- [ ] Import stesso commit su Windows e Mac.

## Scheda per ogni esecuzione

Data; commit; host; Editor; versioni packages-lock; telefono; Android/ARCore; carta/reference image; condizioni di luce; procedura; atteso; osservato; esito; errori Console/logcat; fps misurati se disponibili. Allegare solo evidenze condivisibili. Non segnare come passato un controllo non effettuato.
