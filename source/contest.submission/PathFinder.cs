using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using contest.submission.contract;

namespace contest.submission
{
    public class PathFinder : IPathFinder
    {

        public PathFinder(BoolArray ground, Point startpoint, Point endpoint)
        {
           
 
        }

        /** Algorithm:
         * 
         * Its mor or less the same as described here: http://en.wikipedia.org/wiki/Pathfinding#Sample_algorithm
         *
         * - Create a list of the eight adjacent cells, with a counter variable of the current element's counter variable + 1
         * 
         * - Check all cells in each list for the following two conditions:
         *      * If the cell is a wall, remove it from the list
         *      * If there is an element in the main list with the same coordinate and an equal or higher counter
         * 
         * - Add all remaining cells in the list to the end of the main list
         * 
         * - Go to the next item in the list and repeat until start point is reached 
         * 
        **/
        public Point[] FindAPath()
    {
        Point[] choosenPath = new Point[3];

        //todo
        choosenPath[0] = new Point { x = 1, y = 1 };
        choosenPath[1] = new Point { x = 2, y = 2 };
        choosenPath[2] = new Point { x = 2, y = 3 };

        return choosenPath;
    }

    }
}
