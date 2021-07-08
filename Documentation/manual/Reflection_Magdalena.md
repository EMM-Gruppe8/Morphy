# Selbsteinschätzung Magdalena

## Einleitung
Bei unserem ersten Treffen konnte sich jeder selbst aussuchen, woran er arbeiten möchte. Ich fand das sehr spannend und chillig, weil jeder an dem arbeiten konnte, was ihn interessiert. Ich habe an der Animation der Charaktere gearbeitet, herausgefunden, wie man die Terrain-Assets nutzen kann, als auch die Gegner AI recherchiert und Pathfinding implementiert.

## Animation 
Wir haben kostenlose Assets aus dem Unity-Store verwendet. Es gab bereits Sprites für die Animation, also habe ich im Animator Tab gearbeitet, wo ich die Samples auf 12 gesetzt habe. Sonst wird die Animation zu schnell dargestellt. Im Animator Tab ist es sehr wichtig, wie man die Pfeile setzt und was mit was verbunden ist. Was ich meine, ist, dass es wichtig ist, dass der erste Zustand /Idle/ mit dem Entry State verbunden ist. Von dort aus musste ich die Zustände Jump und Walk verbinden - besonders bei dem Bunny Charakter. Das Nashorn und der Slime Charakteren können nicht springen, also sie hatten nur Idle und Walk State.

## Terrainassets
Für das Terrain habe ich eine Tilemap verwendet. Zunächst wurden die Terrain Slices aus dem Asset zu klein dargestellt, sobald man auf der Tilemap zeichnete. Ich konnte es beheben, indem ich die Pixel pro Einheit auf 16 setzte.

## AI/Pathfinding
Ich habe das Projekt A* ausgewählt, weil ich finde, dass die Implementierung einfacher ist. Wichtig ist, dass im A*Objekt > Pathfinder > Gridgrpah > Diameter kleiner als 1 ist, in unserem Fall ist es 0,5, sonst war der Gridgraph, der den Charakteren sagt, wo sie laufen können und wo nicht, immer über dem eigentlichen Boden. Die Charaktere würden also dann "fliegen".

## Erkenntnisgewinne & Hürden
Für mich war es am schwierigsten, mich an die Arbeit in einem Team mit Leuten zu gewöhnen, die ich vorher nicht kenne. Aber das hat mir tatsächlich in dem Sinne geholfen, dass ich mich jetzt selbstbewusster fühle. Ich habe gelernt, dass es nichts Schlechtes ist, Fragen zu stellen, da dies ein Teil der Teamarbeit ist.

## Anteil
Ich habe das Gefühl, dass ich mehr für unser Team hätte tun können, aber ich bin wirklich glücklich, wie das Spiel am Ende ausgegangen ist.

