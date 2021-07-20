# Verwendete Technologien:
## Unity
Das Spiel baut komplett auf Spiele-Engine Unity auf. Es wurden für die Grafiken dabei einige Assets aus dem Unity Asset Store verwendet. Die Entwicklung der Softwarekomponenten erfolgte in der Unity-Version 2020.3.3f1.
### A*
Die Wegfindung der Gegner wurde mithilfe des A+ Pathfinding Project realisiert. Link zum Projekt: https://arongranberg.com/astar/
## Android/iOS
Die Entwicklung selbst erfolgte weiterhin über ein herkömmliches Desktop-Betriebssystem, jedoch sind die Zielsysteme ausschließlich Smartphones mit Android oder iOS.  Während der Entwicklung konnte die Unity Remote App verwendet werden, damit die App nicht bei jeder kleinen Codeänderung die App kompiliert werden muss. Die Remote-App leitet dabei die Steuerung an den Entwicklungs-Computer weiter. Die Qualität und Performance entsprechen dabei aber nicht einer nativen App.
Die App steht als .apk Datei für Android zur Verfügung. Eine Datei für iOS können wir aufgrund von Einschränkungen von Apple nicht zur Verfügung stellen, diese muss für jedes Gerät und jedem Nutzer einzeln mit Xcode (nur macOS) und einem Apple Developer Account erstellt werden. Eine Veröffentlichung im AppStore ist aufgrund der Gebühren nicht geplant.
### Touch-Input
Die Aktionsteuerung in der App erfolgt über den Touchscreen der Smartphones. Dabei wird unabhängig von der Rotation die rechte hälfe für die Aktion zum Morphen genutzt und die linke hälfte zum Angriff, beides nur sofern entspreche Gegner oder Artefakte sich in der Reichweite befinden. Natürlich erfolgt auch die Navigation im Menü über den Touchscreen.
### Beschleunigungssensor
Die wichtigste Technologie für die gesamte Bewegungssteuerung ist der Beschleunigungssensor, welcher im Smartphone verbaut ist. Über die X-Achse kann man die Spielfigur laufen oder Sprinten lassen, über Ruckartige Bewegungen auf der Y-Achse springt die Spielfigur und die Z-Achse dient der Rotation. Die Intensität der Neigung oder Beschleunigung wird dabei auf die Spielfigur übertragen, bis zu einem vom Character abhängigen Schwellenwert.  
## GitHub
Als Versionsverwaltungssystem haben wir GitHub verwendet. Dabei haben wir eine „Organisation“ angelegt und alle Projektteilnehmer mit den höchsten Rechten ausgestattet. Features und Fehlerbehebungen wurden jeweils in einem eigenen Branch entwickelt und nach erfolgreicher Review in den Main-Branch zusammengeführt. GitHub haben wir zudem auch für die Aufgabenverteilung genutzt, indem Task und Issues dem jeweiligen Verantwortlichen zugeordnet werden konnten.
## DocFX
Für die Dokumentation haben wir uns für DocFX entschieden. Eigene Methoden im Code wurden vom jeweiligen Autor kommentiert. Beim Push in den Master wird die DocFX API-Dokumentation automatisch aus den Code generiert.  Die Schriftliche Projektdokumentation erfolgte manuell in nach Thema organisierten Markdown Dateien, die ebenfalls beim Push in den Main-Branch automatisch zu einer Dokumentation zusammengefasst werden.
