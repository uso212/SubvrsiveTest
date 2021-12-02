# SubvrsiveTest
10 Character brawl game, made as a test.

# How-To
In order to play the game just hit "Start game" in the main screen, it'll trigger the simulation. The characters are spawned in runtime so in order to modify the position of the characters or any of its attributes, please modify the matching Scriptable Object, you can find it at Assets > Data > Characters. The same for the Weapon, and the Bullets that each character has.

# Performance considerations.
In this project I've tried to follow the Unity best practices for performance. You can see that object pooling was used for the bullets, and even though we instantiate the characters in runtime instead of having them in the scene already, this gives us the flexibility of adding new characters by just creating 3 Scriptable Objects (one for the type of bullet, other for the weapon, and the character itself) and removing the need of manually adding code to create new characters.

# Use
Please feel free to take the code in this example as you may see fit. The classes, methods and complex parts of the code are properly commented so you should not have any problem with that.
