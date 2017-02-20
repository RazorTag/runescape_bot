To make a new bot program:
1) Add to the BotActions enum in Start.cs (right-click -> View Code)
2) Add the startup handling for your program to StartButton_Click in Start.cs
3) Create a class in the BotPrograms folder that derives from BotProgram
4) Implement the Run method in your class to tell your bot what to do.
5) Screenscraper.cs has useful tools for interacting with a RS client