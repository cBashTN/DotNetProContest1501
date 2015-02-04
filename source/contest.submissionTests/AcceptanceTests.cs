using System;
using System.Drawing;
using contest.submission;
using contest.submission.contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Point = contest.submission.contract.Point;

namespace contest.submissionTests
{
    [TestClass()]
    public class AcceptanceTests
    {

        [TestMethod()]
        public void OneStepTest()
        {
            // Arrange
            Point startpoint = new Point {x = 0, y = 0};
            Point endpoint = new Point {x = 1, y = 1};
            BoolArray ground = new BoolArray();

            Point[] testPath = new Point[2];
            testPath[0] = startpoint;
            testPath[1] = endpoint;

            // Act
            PathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Point[] myPath = pathFinder.FindAPath();

            //Assert
            Assert.AreEqual(testPath.Length, myPath.Length); // Is the number of steps the same?
            Assert.AreEqual(startpoint, myPath[0]);
            Assert.AreEqual(endpoint, myPath[1]);
        }

        [TestMethod()]
        public void OpenGroundTest()
        {
            // Arrange
            Point startpoint = new Point { x = 1023, y = 1023 };
            Point endpoint = new Point { x = 0, y = 0 };
            BoolArray ground = new BoolArray();

            const int groundDimensionLength = 1024;

            // Act
            PathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Point[] testeePath = pathFinder.FindAPath();

            //Assert
            Assert.AreEqual(startpoint, testeePath[0]);
            Assert.AreEqual(endpoint, testeePath[1023]);
            Assert.AreEqual(groundDimensionLength, testeePath.Length); // Is the number of steps the same?
        }


        [TestMethod()]
        public void DefaultObstaclesGroundTest()
        {
            // Arrange
            Point startpoint = new Point { x = 0, y = 0 };
            Point endpoint = new Point { x = 1023, y = 1023 };

            BoolArray ground = new BoolArray();
            ground.GenerateObstacle();

            const int expectedStepsCount = 1033; //without the first startpoint

            // Act
            PathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Point[] testeePath = pathFinder.FindAPath();

            //Assert
            Assert.AreEqual(startpoint, testeePath[0]);
            Assert.AreEqual(endpoint, testeePath[expectedStepsCount]);
            Assert.AreEqual(expectedStepsCount, testeePath.Length-1); // Is the number of steps the same?
        }


        [TestMethod()]
        public void ComplexObstaclesGroundTest()
        {
            // Arrange
            Point startpoint = new Point { x = 0, y = 0 };
            Point endpoint = new Point { x = 1023, y = 1023 };

            BoolArray ground = GenerateObstacleFromBitmap("Labyrinth1.bmp", new BoolArray(), 1); ;
            const int expectedStepsCount = 1623;

            // Act
            PathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Point[] testeePath = pathFinder.FindAPath();

            //Assert
            Assert.AreEqual(startpoint, testeePath[0]);
            Assert.AreEqual(endpoint, testeePath[expectedStepsCount]);
            Assert.AreEqual(expectedStepsCount, testeePath.Length-1); // Is the number of steps the same?
        }

        [TestMethod()]
        public void LabyrinthBeetzTest()
        {
            // Arrange
            Point startpoint = new Point { x = 511, y = 0 };
            Point endpoint = new Point { x = 511, y = 511 };


            BoolArray ground = GenerateObstacleFromBitmap("labyrinthBeetz.bmp", new BoolArray(), 20); ;
            const int expectedStepsCount = 20020;

            // Act
            PathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Point[] testeePath = pathFinder.FindAPath();

            //Assert
            Assert.AreEqual(startpoint, testeePath[0]);
            Assert.AreEqual(endpoint, testeePath[expectedStepsCount]);
            Assert.AreEqual(expectedStepsCount, testeePath.Length-1); // Is the number of steps the same?
        }

        [TestMethod()]
        public void TrickyLabyrinthGroundTest()
        {
            // Arrange
            Point startpoint = new Point { x = 0, y = 0 };
            Point endpoint = new Point { x = 1023, y = 1023 };


            BoolArray ground = GenerateObstacleFromBitmap("labyrinthTricky.bmp", new BoolArray(), 1); ;
            const int expectedStepsCount = 3501;

            // Act
            PathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Point[] testeePath = pathFinder.FindAPath();

            //Assert
            Assert.AreEqual(startpoint, testeePath[0]);
            Assert.AreEqual(endpoint, testeePath[expectedStepsCount]);
            Assert.AreEqual(expectedStepsCount, testeePath.Length - 1); // Is the number of steps the same?
        }

        [TestMethod()]
        public void Tricky2LabyrinthGroundTest()
        {
            // Arrange
            Point startpoint = new Point { x = 0, y = 0 };
            Point endpoint = new Point { x = 1023, y = 1023 };


            BoolArray ground = GenerateObstacleFromBitmap("labyrinthTricky2.bmp", new BoolArray(), 1); ;
            const int expectedStepsCount = 1896;

            // Act
            PathFinder pathFinder = new PathFinder(ground, startpoint, endpoint);
            Point[] testeePath = pathFinder.FindAPath();

            //Assert
            Assert.AreEqual(startpoint, testeePath[0]);
            Assert.AreEqual(endpoint, testeePath[expectedStepsCount]);
            Assert.AreEqual(expectedStepsCount, testeePath.Length - 1); // Is the number of steps the same?
        }

        public BoolArray GenerateObstacleFromBitmap(String bitmapCopiedContentName, BoolArray ground, int scale)
        {
            var b = new Bitmap(bitmapCopiedContentName, true);

            int shiftX = (1024 - b.Width * scale) / 2;
            int shiftY = (1024 - b.Height * scale) / 2;

            for (int i = 0; i < b.Width; i++)
            {
                for (int k = 0; k < b.Height; k++)
                {
                    var pixel = b.GetPixel(i, k);

                    if (pixel.R >= 128 || pixel.G >= 128 || pixel.B >= 128) continue;
                    for (int x = 0; x < scale; x++)
                    {
                        for (int y = 0; y < scale; y++)
                        {
                            ground.Data[shiftX + i * scale + x, shiftY + k * scale + y] = true;
                        }
                    }
                }
            }
            return ground;
        }
    }
}
