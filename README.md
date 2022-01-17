# Generating data

Use html data with winrates, with regex find/replace, find "[^\S\t]\n" and replace with "\t"

# Interesting Events

### <== Event_Join

### <== BotDraft_DraftStatus

Status of a started or ongoing bot draft. Pack card ids are in DraftPack. The players currently drafted cards are in PickedCards.

```
{ Payload: { PackNumber: int, PickNumber: int, DraftPack: ["(int card id)",...], PickedCards: ["(int card id)"] } }
```

### <== BotDraft_DraftPick

Server has sent a new pack after the player has made a pick. Pack card ids are in DraftPack. The players currently drafted cards are in PickedCards.

```
{ Payload: { PackNumber: int, PickNumber: int, DraftPack: ["(int card id)",...], PickedCards: ["(int card id)"] } }
```

### ==> BotDraft_DraftPick

Player is sending their pick to the server

```
{ request: { Payload: { PickInfo: { CardId: "(int card id)" }}} }
```