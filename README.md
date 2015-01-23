# DotNetPro Contest 15/01
#####Describtion:
Solution for a fun coding contest to guide a robot through a labyrinth. This contest is part of the dotnetpro magazine issue 1/15.

#####Personal goal priority:
1) Solve the problem in a feasible time  
2) The solution should give an optimum result: Guaranteed shortest path  
3) As many others find an perfect solution, too, I would like to provide at least a robot path that is different than the ones of the others  
4) Performance optimisation is not part of the contest. Although, it doesn't hurt do have a maximum run time of 30sec for a labyrinth  

#####Used Algorithm:
I didn't reinvent the wheel, so I just used the next simple one I found: http://en.wikipedia.org/wiki/Pathfinding#Sample_algorithm

#####Results:
Contest results are going to be public in magazine issue 4/15

To goal #2) It seems I have the same step numbers as the others. (See http://www.dotnetpro.de/newsgroups/ThreadList.aspx?groupId=15)

To goal #3) M.Beetz shared a method to graphically display the path of the robot: http://www.beetz.eu/A1501ContestSource.html

M.Beetz path (20020 steps): ![alt text](http://www.beetz.eu/Gelaende3.png "M.Beetz robot's path")
This solution's path (20020 steps): ![alt text](http://i.imgur.com/DmAq6KG.png "My robot's path")

To goal #4) The worst case labyrinth performance-wise for the algorithm used would be the one from M.Beetz. My older notebook needs 62 seconds for this. As this labyrith's step count exceeds the official one and due to the low priority, I don't waste time for further optimizations. It is good enough. YAGNI.
