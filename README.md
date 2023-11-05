# Content TODO List
- Mining ausbauen
- Mehr adventure commands
- Mehr Crafting Rezepte
- Drugs
- Random locations für Mane command, am besten eine suspekte Location und irgendeine droge dazu

# Public TODO List Systen
- BuyPrices & SellPrices balancen
- Wahrscheinlichkeitssystem
- Weekly & Monthly
- Give every command an attribute

# Markus TODO List
<!-- - Timer Attribute -->
<!-- - Command Attributes für syntax -->
- Weight
- Tool class
- Loottable class
- Battler
- Abilities
- Playerdata Get methods standardizen
- Überlegungen bezüglich Timestamps für command cooldowns wegen overloads (welcher syntax ist gut dafür)
- snippets fixen
- 

# Adding new GameElements
- Add them to the switch statement of CommandHandler.EvaluateCommand()


# Web porting
- Extract Discord from litterally everywhere
- Add pages and www root and all of that
- let mane design
- write a razor page core application
- write razor logic

# Request handling for different ports
- Command class with all the different methods, each with attributes
- For Discord;
  - Discord.NET sends requests to server directly
  - Requests are being formatted into Request class
- For Web:
  - Requests are sent from website to server via http
  - Requests are being sent to server in Request format already

# Logging
- Admin Log:
  - Does not have anything to do with the rest of the command log, is just a plain txt file per session 
- Command Log
  - Log class
  - Can be loaded and unloaded easily


# For the bot to work
- Extract the discord.net from the evaluate command method and write overloads, refractor, then add a event handler for the commandreceived and add the tasks