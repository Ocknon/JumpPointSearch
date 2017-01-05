# JumpPointSearch
### Made in Unity3D 5.3.1
![alt text](https://raw.githubusercontent.com/ocknon/JumpPointSearch/master/Pictures/JumpPoint.png)

#### How this project came to be:
In class we were introduced to a simple form of A* pathfinding through waypoints. For my midterm I worked to make
a better pathfinding system for projects in the future. For a few days I worked on implementing A* and found it to be terribly
slow. I scoured the internet looking for better solutions and found Jump Point, however I found few documentations on it and no tutorials.
I worked on it for the next two weeks and eventually ended up with this in time to turn in!

#### Basic overview:
Jump point works almost exactly the same as A* and the structure is the same. Instead of grabbing every neighbour around the parent node
and doing all the calculations to every single node, you instead move along a direction recursively until you find a point of importance
or hit an obstacle/off the map. A node of importance is one where there is a possible path around an object, for example a corner where 
the top node is unwalkable, but the diagonal node in the direction of movement is clear. It cuts all the node calculations down 
monumentally, prunes symmetrical paths, and eliminates a lot of overhead memory.

#### How good is it?
![alt text](https://raw.githubusercontent.com/ocknon/JumpPointSearch/master/Pictures/AStarTime.png)		
A* ran through that maze in 40ms. That doesn't seem so bad until you realize that's just for one unit. When adding multiple units with
their own unique destination, you can get crazy amounts of computing time for pathing.	
![alt text](https://raw.githubusercontent.com/ocknon/JumpPointSearch/master/Pictures/JumpPointTime.png)		
Jump Point cuts down the time dramatically, cutting the computing time to a fraction of A*. Through many different tests this holds true
in every situation, especially cases where the target is unreachable. While jump point excels in open environments, it is still extremely
fast in confined spaces with hard to reach places.

#### Problems that came up:
One of the hardest things to understand was just getting the algorithm to make sense. After a couple attempted tries it still didn't make
any sense. After understanding it, there were still many problems with each iteration. Debugging became a massive hassle and I learned an
enormous amount about debugging and how important it is to set up custom tools. I restarted the project around 10 times, even with the 
2 week deadline.

#### This isn't the end:
This is not the most optimal iteration. There's a lot more than can be added to this to make it even faster. I had plans to change the
structure and have the grid store integers so that I could bit-shift rapidly instead of recursively calling a function to find 
neighbours.

Sources:	
[A* algorithm explained](https://www.youtube.com/watch?v=-L-WgKMFuhE)	
[Jump Point algorithm](http://users.cecs.anu.edu.au/~dharabor/data/papers/harabor-grastien-aaai11.pdf)	
[Jump Point explained](https://harablog.wordpress.com/2011/09/07/jump-point-search/)	
[Another Jump Point explination](https://gamedevelopment.tutsplus.com/tutorials/how-to-speed-up-a-pathfinding-with-the-jump-point-search-algorithm--gamedev-5818)
