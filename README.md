# Katz und Maus
Alexander Bleisinger
Das Projekt hat zum Ziel Unity ml-Agents in einer Multi-Agent Umgebung gegeneinander antreten zu lassen.
In diesem spezifischen Fall versucht ein Katzen-Agent einen Maus-Agent zu fangen, währenddessen der Maus-Agent versucht zu flüchten.
Dabei gibt es zwei unterschiedliche Schwiergikeitsgrade in der Umgebung
- eine einfache Umgebung mit unterschiedlich verteilten Wänden um Overfittung zu vermeiden
- einfache Labyrinthe

### Zu beachten
Das Projekt basiert auf [CatAndMousePPO](https://github.com/pegasus0112/CatAndMousePPO). Das Projekt stieß an seine Grenzen, als Team-Play hinzugefügt wurde und der Code deswegen zu kompliziert wurde. Dieses Projekt nutzt statt dem PPO-Algorithmus den MA-POCA-Algorithmus der auf Teamverhalten ausgelegt ist. Die Mäuse zeigen jedoch im alten Projekt ein etwas besseres verhalten.

Das Projekt enthält zudem extra Branche in denen Experimente durchgeführt wurden. Interessant dürfte der [camera-sensor-instead-of-rays-view](https://github.com/pegasus0112/CatAndMouse--Unity-ml-Agents/tree/camera-sensor-instead-of-rays-view)-Branch sein, da dieser etwas größer ist und ebenfalls interessante Demos enthält.

## Beispielvideo

BITTE HIER EINFÜGEN

## Besonderheiten
### Neue NN trainieren
Um das Projekt nutzen zu können, muss eine Python-Umgebung agelegt werden, welche sowohl pythroch, als auch die mlAgents enthält
Für eine genauere Erklärung bitte die [offizielle](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Installation.md) Anleitung lesen.
Zudem müssen die ml_agent Pakete im Unity Editor installiert sein (in der Unity Registry vorhanden). Hier Bitte die mindestens 2.3.0-exp.3 verwenden.
die genutzen Konfigurationen, befinden sich im *config*-Ordner.

Beispiel Ablauf:
1. cmd in *colfig*-Ordner
2. In *python-env* wechseln
3. Befehl für das Training ausführen z-B.: 
    ```sh
    mlagents-learn Joined.yaml --run-id=trainingName
    ```
Um ein neues neuronales Netzwerk zu trainieren, bitte den offiziellen Schritten aus der ml-Agent  [Dokumentation](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Getting-Started.md) folgen.
Hier ist der Bereich ***Training a new model with Reinforcement Learning*** relevant.
### Demo
Der *Scene*-Ordner enthält einen *Demo*-Ordner. Hier muss man einfach nur die Scene öffnen und abspielen. Die Demo-Szenen enthalten bereits trainierte NN.

### Herausforderungen während der Entwicklung
Während der Entwicklung gabe es zwei Probleme:
1. Wenn man eine neue KI erzeugt weiß man zu beginn nicht, ob diese funktioniert. erst nach vielen Trainigsschritten kann man fortschritte erkennen. Im Fall von diesem Projekt dauerte es ca. 1 Mio Steps bis man ein verhalten erkennen konnte und 2.5 Mio. Steps bis die Katze ein gutes verhalten zeigt, im Versuch die Maus zu fangen. Die Maus hingegen war etwas komplizerter:
   Die Maus erhält zeitlich basierte Belohungen für das Überleben. Ist die Katze noch untrainiert, muss die Maus nichts spezifisches tun, um zu überleben, da die Katze sie ja nicht wirklich findet. Ist die Katze trainiert, muss sich die Maus auch verbessern. Was aber bedeutet das die Maus erst mit einer gut trainierten Katze trainiert. Zudem müssen beide gleichezeitg trainieren, da sonst ein Overfittung-Verhalten entsteht.
2. Overfitting:
   Die Katze hat zu Beginn in einer simplen Umgebung traniert. indem nur die Katze und die Maus zufällig platziert werden (Position & Rotation).
   

https://user-images.githubusercontent.com/33843960/212026614-84eb72fd-37b2-4ed8-8e4b-300eac627640.mp4


Das obere Video zeigt den Begin des Trainings. Das Problem bei dieser Umgebung ist es jedoch, das die Katze sich theoretisch nur um sich selbst drehen muss, um die Maus zu finden und dann zur Maus zu laufen. Das ist ein typischer Fall von Overfittung! Die katze muss nicht aktiv nach der Maus suchen, d.h. um Gegenstände und Wände laufen um die Maus zu finden.
Auf diesem Prinzip hat da auch die Maus, mit einem fertig traninierten Katen-Agent trainiert. Dadurch wurde das Overfitting noch schlimmer:


https://user-images.githubusercontent.com/33843960/212104754-05f6479c-d924-459f-8b8c-d736733f879b.mp4


Die Maus hat gelernt, dass sie lediglich im Kreis rennen muss, um der ktze zu entkommen. Die Katze hingegen versteht nicht, das sie ihr verhalten ändern muss, um die Katze zu fangen.

Damit das Projekt erfolg haben würde, mussten die Agents also in einer sehr unterschiedlichen Umgebung trainieren, damit kein Overfitting entsteht.

In der neuen Umgebung ist das Overfittung noch deutlicher zu erkennen:


https://user-images.githubusercontent.com/33843960/212027361-0516d3cf-a125-4e4d-a27b-e939797c9e2e.mp4


Die Katze dreht sich um sich selbst bis sie die Maus findet und die Maus läuft einfach nur im Kreis. 
Wichtig ist als, nicht nur die neue Umgebung, sondern auch, das die Angents zeitgleich trainieren, damit beide auch voneinander lernen und besser werden. Auf dieser weiße trainierte Agents verhalten sich deutlich besser: 


https://user-images.githubusercontent.com/33843960/212028749-1b1c2e3a-dbbb-431e-867d-cdf8698438de.mp4


## Referenzen
Die Grund Idee stammt von einem *Hide and Seek* Video von OpenAi:


[![OpenAI Plays Hide & Seek](http://img.youtube.com/vi/Lu56xVlZ40M/0.jpg)](http://www.youtube.com/watch?v=Lu56xVlZ40M "OpenAI Plays Hide and Seek…and Breaks The Game! 🤖")

Die Katzen und Mäuse (Model & Animation) stammen von der [Quirky Series - Animals Mega Pack Vol.1](https://assetstore.unity.com/packages/3d/characters/animals/quirky-series-animals-mega-pack-vol-1-137259)

Alles andere (Code, Logik) basiert auf dem Wissen, welches ich aus der offiziellen [Dokumentation](https://github.com/Unity-Technologies/ml-agents/tree/main/docs) der ml-Agents habe. Also bis auf die Grund Idee ist alles selbst erarbeitet.

## 2 Minuten Video Training


https://user-images.githubusercontent.com/33843960/212070218-b6033593-3fec-4a42-be73-fb64d3f05f2d.mp4


