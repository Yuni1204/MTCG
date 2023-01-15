Design:
Das Programm Startet mit dem Server.cs, wo es mit der StartServer() Methode beginnt nach Clients zu hören.
Wenn ein Client sich verbinden möchte, startet der Server einen Thread, wo er die Request des Clients abarbeitet
und eine passende Response zurückschickt.

Lessons Learned:
Am Anfang des Semesters den Moodle Kurs möglichst genau schon im Vorfeld anschauen, damit ich 
möglichst früh schon weiß was gefragt ist. (zb wurde dieses Semester im Moodle Kurs gesagt,
man soll BattleLogic zuerst machen. Jedoch musste ich alles löschen, da es nicht mit der ServerStruktur
und dem Http Request Handling zusammengepasst hat. Auch wurde dann im Endeffekt Battle Logic ziemlich 
am Ende des Projekts erst implementiert.)

Natürlich das nächste mal früher beginnen das Programm zu schreiben, ist auch im vorherigen Punkt impliziert.

C# ist eine sehr angenehme Sprache.

Unit Test Design:
Die Unit Test des Programms prüfen die BattleLogic, um die Karte gegen Karte
Runden auf ihre

Unique Feature:
Mein Unique Feature ist, dass beim Battle, Spieler, die vor dem Kampf einen gewissen Elo Unterschied
überschreiten, die Möglichkeit haben Coins zu gewinnen.
Wenn der Spieler mit der deutlich niedrigeren Elo gegen den High-Elo Spieler gewinnen sollte, bekommt
der Spieler mit niedriger Elo 5 coins -> eine package.


Time Spent:
sehr viele Stunden, habe leider nicht mitgezählt wieviele.
wahrscheinlich um die 80 Stunden.


Link to Git:
im README.md ist der link auch zu finden:
https://github.com/Yuni1204/MTCG
