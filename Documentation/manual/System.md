# System

![Screenshot](https://raw.githubusercontent.com/EMM-Gruppe8/Morphy/gh-pages/resources/SystembildColored.png)

Die Komponenten der App lassen sich in drei Kategorien einteilen.

## Hardwarekomponenten (Grün):
Die erste Kategorie, im Systembild grün dargestellt, sind die die verwendeten Hardwarekomponenten. Die Schnittstellen zu diesen Komponenten werden von den Betriebssystemen Android oder iOS zur Verfügung gestellt und durch Unity angesprochen. 

### Display:
Der Display dient der Anzeige des Spiels. 

### Touch-Input:
Der Touch Input wird für die Menü- und Aktionssteuerung benötigt. Während des Spiel läuft, dient die rechte Hälfte der Aktion zum „Morphen“, wenn sich ein entsprechendes Artefakt in der Nähe befindet. Die über die linke Hälfte kann der Spieler den nächstgelegenen Gegner angreifen.

### Acceleration Sensor / Beschleunigungssensor:
Wie im Kaptiel „Verwendete Technologien“ erwähnt, wird der Beschleunigungssensor für die Bewegungssteuerung genutzt. Die X-Achse dient  der horizontalen Bewegung. Je nach Neigung läuft die Spielfigur unterschiedlich schnell. Die Y-Achse wird für das Springen genutzt, dabei muss der Spieler das Smartphone schnell zu sich ran ziehen oder wegdrücken, um die Spielfigur springen zu lassen. Die Z-Achse wird ausgelesen, um zu erkennen, wenn das Smartphone auf umgedreht wird, z.B. um den Schleim auf die andere Seite fallen zu lassen.

## System- und Umgebungskomponenten (Blau)
Die im Systembild blau dargestellten Komponenten sind alle Teil des Unity-Spiels und sind für alles zuständig, was nicht direkt mit den Spielfiguren zu tun hat.

### Game Controller:
Als zentrale Verwaltung steht der Game Controller in der Mitte. Dieser erstellt Instanzen von den anderen verbunden Komponenten und bietet einen zentralen Zugriff auf diese. Der Game Controller ist somit auch die Verbindung zwischen den Blauen und den roten Komponenten. Die Instanzen werden als Singleton erstellt.

### Artefakt Spawner:
Der Artefakt Spawner beinhaltet alle Orbs bzw. Artefakte, die ein Gegner im Todesfall hinterlässt.
Ist ein Gegener besiegt werden, anhängig vom Charakter Typ, neue Instanzen eines Prefab erzeugt. Nach dem ein Artefakt genutzt wurde, deaktiviert die Komponente dieses. Die Artefakte haben die Primärfarbe des dazugehörigen Charakters.

### Highscore Controller:
Diese Komponente misst die Zeit. Bei jedem Start eines Level, bzw Re-Spawn des Spielers, wird die Messung gestartet und in der Zielzone beendet. Ist die Zeit besser als die vorherige wird diese in den User Prefs gespeichert. 

### Virtual Camera:
Die Virtuelle Camera ist eine „CinemachineVirtualCamera“ aus dem Cinemachine Paket. Die Kamera ist an den Spieler gebunden, damit diese immer den Spieler zentriert. Cinemachine wird genutzt um zu erreichen, dass die Kamera sich niemals aus dem Spielfeld beweg und ggf. die Zentrierung löst, wenn der Spieler z.B. in einen Abgrund fällt. Die Komponente spricht den Physikalischen Bildschirm an.

### Level Controller
Der Level Controller verwaltet alle drei Level. Die Komponente ermöglich das Laden von Levels, sichert dabei aber ab, dass nur bereits freigeschaltete Level geladen werden können. Hat ein Spieler ein Level erfolgreich abgeschlossen, schaltet diese Komponente das nächste Level frei. Die Daten werden in den User Prefs gespeichert.

### UI / Benutzeroberfläche
Die UI-Komponente beinhaltet alle Bestandteile der Benutzeroberfläche. Dazu gehört das Hauptmenü, aber auch das Pausenmenü, welches über einen Pause-Button auf dem Bildschirm aufgerufen werden kann. Die Komponente wird über den Touch-Input gesteuert und greift zum Laden der Level auf den Level Controller zu. Auch die Button für die linke und rechte Seite des Bildschirms sind hier enthalten.

## Spieler- und Gegnerkomponenten (Rot)
Die rot dargestellten Komponenten betreffen den Spieler oder die Gegner. Es gibt zwei Hauptkomponenten, den Spieler und die Gegner. Beide teilen sich dann gemeinsame Komponenten, für Eigenschaften, die bei beiden gleich sind.

### Player Controller:
Der Player Controller steuert und verwaltet den Spieler. Es ist die zentrale Komponente, in der auch die Steuerung implementiert ist. Für jeden Charakter Typ sind Werte für die verfügbaren Fähigkeiten, wie z.B. Springen oder Sprinten, definiert. Auch die Methoden zum Morphen, also dem Wechseln des Charakter Typen sind enthalten. Der Controller bleibt also immer der gleiche und reagiert nur unterschiedlich anhand des Charakter Typen.

### AI Enemy:
Die Komponente für die Gegner KI beinhaltet die A* Wegfindung und Logiken um dem Spieler zu lokalisieren. Ebenso wird sichergestellt, dass die Gegner nicht einfach über Klippen laufen und in den Tod stürzen. Auch hier sind alle Fähigkeiten für die einzelnen Charaktertypen implementiert, sodass alle Gegenderten den gleichen Controller nutzen können.  

### Health:
Die Health Komponente ist eine Klasse, welche dem Spieler und auch den Gegnern eine maximale Gesundheit gibt. Methoden zum Reduzieren oder Erhöhen der Gesundheit sowie zum Auffüllen der gesamten Gesundheit sind vorhanden. Fällt die Gesundheit auf 0, wird ein Event ausgelöst.

### Animation
Die Komponente zur Animation ist eine Vielzahl an verschiedenen Animation Controllern für jeden einzelnen Charaktertypen. Sie beinhaltet Animationen für den Stillstand, laufen, Sprinten und springen, sofern der jeweilige Charakter die Fähigkeit besitzt. Die Animationen werden von dem Spieler und auch von den Gegnern genutzt. 

### Attackable / Atackker
Diese Komponente sorgt dafür, dass die Spieler und Gegner überhaupt angreifen können und auch angegriffen werden können. Sie bietet Methoden um zu berechnen, welcher der nächstgelegene Feind ist und kann diese angreifen. Timeouts nach jedem Angriff erhöhen dabei die Komplexität des Kampfes. Die Unterscheidung zwischen Spieler und Gegner funktioniert über die Tags der Objekte und es wird zwischen der Spezialfähigkeit und einem normalen Kampf unterschieden.

### Kinematic Object
Das Kinematic Object ist die Physik des Spielers für den Rigidbody. Es enthält grundlegende Methoden zur Bewegung des Objektes und invertiert die gesamte Steuerung, wenn der Spieler sich auf der Decke bewegt. Da die Bewegung der Gegner anders realisiert wurde für A*, ist diese Klasse nur Teil des Spielers.

## Event-Manager (Lila)
Der Event-Manager ist Lila eingefärbt, da er eigentlich als eine blaue Systemkomponente zu sehen ist, aber stark mit den Spieler und den Gegnern interagiert. Der Manager nutzt das Event Queue Pattern und baut Eventwarteschlangen auf, die sich beliebig verketten lassen. Die eigentlichen Events können dabei auf alle Komponenten zugreifen. Ein Beispiel für ein Event ist, wenn der Spieler kein Leben mehr hat, wird der Tod ausgelöst, was wiederum die Steuerung kurzfristig deaktiviert und anschließend das Level neu startet. Aber auch das Erreichen eines Ziel oder das Drehen des Bildschirmes sind Events, welche über den Event-Manager ausgeführt werden.
