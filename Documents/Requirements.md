# Beschreibung:
Der Spieler spielt einen Virus in einem 2D Jump ‘n Run, welcher die Fähigkeiten von
Gegnern übernehmen kann. Attackiert und besiegt der Spieler beispielsweise einen
fliegenden Gegner, so kann der Spieler nun selber fliegen.
Der Spieler kann hierbei immer nur die Fähigkeiten einer Spielfigur gleichzeitig besitzen:
Kann der Spieler zur Zeit fliegen und tötet einen Gegner, welcher an Wänden laufen kann,
so kann der Spieler nun an Wänden laufen, jedoch nicht mehr fliegen.
Der Spieler muss hierbei die Fähigkeiten verschiedener Gegner nutzen, um das Level
erfolgreich zu durchqueren, es kommt also auch auf das Geschick des Spielers an, das
Level mit den richtigen Fähigkeiten möglichst schnell zu meistern.

***Plattform: iOS + Android***

# Funktionale Anforderungen

## 1. Steuerung / Interaktionen
- Die Spielfigur wird über die Gyroskopsensor bzw. Beschleunigungssensor des
Smartphones bewegt.
- Die Geschwindigkeit der Spielfigur passt sich dabei der Neigung an, sodass diese
auch sprinten kann
- Charaktere verhalten sich unterschiedlich zur Schwerkraft, bzw Interaktion damit
- Die Angriffe werden über die Touch-Eingabe aktiviert.
- Der Wechsel zwischen Charakteren wird über Touch-Eingaben aktiviert.

## 2. Leveldesign
- Es soll in jedem Level Hindernisse geben, die es zu überwinden gilt. Es soll dabei
nicht möglich sein, das Level nur mit einer Fähigkeit durchzuspielen, sodass der
Spieler gezwungen wird sich andere Fähigkeiten zu besorgen.
  - höhere Hindernisse können durch Springen überwunden werden (Stein)
  - sehr Hohe Hindernisse können nur durch wechsel der Seite (oben/unten) überwunden werden
  - Gräben können nur durch Springen oder Seitenwechsel überwunden werden
  - weite Schluchten können nur mit Sprinten überwunden werden (Springen von Berg)
  - 
## 3. Levelfortschritt
- Es gibt drei unterschiedliche Level, die erst freigeschaltet werden, nachdem das
vorherige erfolgreich durchgespielt wurde.
- Die Level werden dabei schwieriger, um die Herausforderung für geübte Spieler zu
erhöhen

## 4. UI und Menüs
- Im Hauptmenü kann der Spieler direkt weiterspielen, freigeschaltete Level
auswählen und seine Zeiten einsehen.
- Im Pausenmenü kann der Spieler das Spiel pausieren, komplett neustarten und
zurück zum Hauptmenü gehen

## 5. Fähigkeit Seitenwechsel
- Charakter mit dieser Fähikeit sollte beim auf-den-Kopfstellen des Smartphones auf die gegenüberliegende Seite fallen und dort genau so laufen können, wie auf der anderen (Charakter: Slime)

## 6. Fähigkeit Hüpfen
- Durch Ranziehen des Handys kann der Charakter hüpfen (Charakter: Bunny)
- Je nach Stärke der Bewegung kann die Höhe des Sprungen moduliert werden

## 7. Fähigkeit übernehmen
- Die Fähigkeit eines besiegten Gegners kann übernommen werden
  - In der Nähe des Leichnahms kann ein Button gedrückt werden um die Fähigkeit zu übernehmen

## 8. Gegner AI
- Gegner mit verschiedenen Eigenschaften haben unterschiedliche Verhaltensweisen
beim Kampf und in der Fortbewegung und verwenden sie gegen den Spieler

## 9. Gesundheitssystem
- Der Spieler und die Gegner haben ein Gesundheitssystem basieren auf
Health-Points, welche im Fern- oder Nahkampf reduziert werden.

## 10. Kampfsystem
- Charaktere können durch charakterspezifische Aktionen Gegner sofort Ausschalten (Auf den Kopf springen: Hase, Rammen: Nashorn, Auf Gegner fallen: Slime)
- Charakter können durch Boxen geringen Schaden ausrichten (für alle Charaktere gleich). Dies wird durch einen virtuellen Button ausgelöst

## (Optional). Bestzeiten
- Die Zeit, die der Spieler für ein Level benötigt, wird gespeichert, das erhöht den
wiederspielwert, da der Spieler seine eigenen Bestzeiten einsehen kann

## (Optional) Feedback über Vibration des Smartphones
- Wird der Spieler verletzt, oder stirbt vibriert das Smartphone, um dem Spieler ein
besseres Feedback zu geben

## (Optional) Audio
- Aktionen und Bewegungen verursachen unterschiedliche Töne(Optional) Checkpoints
- Es können Checkpoints erreicht werden, bei denen der Spieler im Falle seines Todes
wieder starten kann.
- Der Spieler kann dabei jederzeit zum letzten Checkpoint zurückkehren

## (optional) Fähigkeit: Fliegen
- Charakter “Biene” kann fliegen und aus der Luft angreifen
- Nach dem Töten des Gegners erscheint an der Stelle der Tötung ein Orb
- Der Spieler besitzt ein Rand seines Bildschirms einen “Morph” Button, welcher aktiv
wird, wenn ein Orb in der Nähe ist
  - Ähnlich zu dem “Report” Button in Among Us
- Nach Drücken des Buttons holt der Spieler sich die Fähigkeit des Orbs
- Der Spieler kann sein Gerät nun neigen, um sich auch hoch unten runter zu
bewegen
## (optional) Fähigkeit: Grappling Hook
- Charakter “Chameleon” kann mit Hilfe der Zunge an Wände, Böden, Decke haften
und sich dort selber hinziehen - kann jedoch nicht normal laufen
- Morphing funktioniert wieder gleich wie unter 5. Fähigkeit: Fliegen
- Die Fähigkeit kann genutzt werden, in dem auf eine Wand/Decke/Boden getippt wird

# Nicht-Funktionale Anforderungen
- Benutzerfreundlichkeit in der Bedienung und in den Menüs
- Unterstützung verschiedener Auflösungen und Bildschirmverhältnisse bei den
Smartphones.

# Fähigkeiten, Aufteilung der Arbeiten
Ausgelagert in das Dokument EMM: Arbeitspakete

## Assets:
***“Pixel Adventure 1”***

https://assetstore.unity.com/packages/2d/characters/pixel-adventure-1-155360

***“Pixel Adventure 2”***

https://assetstore.unity.com/packages/2d/characters/pixel-adventure-2-155418#reviews
