/* Name: Daniel Frates
 * Class: CS 2300
 * Date: 03/17/22
 * Assignment 3 part 3
 * Summary: Read in either a 2d array or a 3d array. if
 * input is a 2d array then find triange area and form a line, then 
 * find the distance from the third point to the line. if 3d input
 * then find area of triange and form a plane that bisects 2 points,
 * then find the distance from the third point to the plane.
 */

using System;
using System.Collections;
using System.IO;

namespace DFrates_part3
{
    class Program
    {
        //Input structure
        static ArrayList inputMat = new ArrayList();

        //2d array and 3d array
        static double[,] twoDMatrix = new double[2, 3];
        static double[,] threeDMatrix = new double[3, 3];
        

        static void Main(string[] args)
        {

            string[] inputs = LoadInputFiles();

            Control(inputs[5]);
            

        }

        /// <summary>
        /// Controls whether to call 3d operations or 2d operations
        /// based on input structure size
        /// </summary>
        /// <param name="input"></param>
        static void Control(string input)
        {
            ReadFile(input);
          
            if(inputMat.Count == 9)
            {

                ThreeD();

            } else
            {
                TwoD();
            }
           
           
        }

        //runs 3D operations
        static void ThreeD()
        {
            int size = 3;
            Load3DMatrix();
            Console.WriteLine("====================");
            Console.WriteLine("\tInput");
            Console.WriteLine("====================");
            Display(threeDMatrix);

            Console.WriteLine();

            Console.WriteLine("====================");
            Console.WriteLine("\tOutput");
            Console.WriteLine("====================");
            double triangle3DArea = AreaOfTriangle(size);
            Console.WriteLine("Triangle Area: {0:0.000}", triangle3DArea);

            Console.WriteLine();

            double[,] pnt1 = GetPoint(0, size);
            double[,] pnt2 = GetPoint(1, size);
            double[,] pnt3 = GetPoint(2, size);
            double[] plane = ConstructPlane(pnt1, pnt2);
            double distance = DistanceToPlane(plane, pnt3);
            Console.WriteLine("Distance to plane: {0:0.000}", distance);
        }

        //runs 2d operations
        static void TwoD()
        {
            int size = 2;
            Load2DMatrix();
            Console.WriteLine("====================");
            Console.WriteLine("\tInput");
            Console.WriteLine("====================");
            Display(twoDMatrix);

            Console.WriteLine();

            Console.WriteLine("====================");
            Console.WriteLine("\tOutput");
            Console.WriteLine("====================");
            double triangle2DArea = AreaOfTriangle(size);
            Console.WriteLine("Triangle Area: {0:0.000}", triangle2DArea);

            Console.WriteLine();

            double[,] pnt1 = GetPoint(0, size);
            double[,] pnt2 = GetPoint(1, size);
            double[,] pnt3 = GetPoint(2, size);
            double[,] vector = GetVector(pnt2, pnt1, size);
            double[,] line = FormLine(pnt1, vector);
            double distance = DistanceToLine(line, pnt3);
            Console.WriteLine("Distance to line: {0:0.000}", distance);
        }

        /// <summary>
        /// Computes the distance from point to plane
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="pnt"></param>
        /// <returns>"distance"</returns>
        static double DistanceToPlane(double[] plane, double[,] pnt)
        {
            double distance = 0;

            distance = plane[0] * pnt[0, 0] + plane[1] * pnt[1, 0] + plane[2] * pnt[2, 0] - plane[3];

            return distance;
        }

        /// <summary>
        /// Creates a plane that bisects 2 points and stores it in
        /// an array.
        /// </summary>
        /// <param name="pnt1"></param>
        /// <param name="pnt2"></param>
        /// <returns>  plane  </returns>
        static double[] ConstructPlane(double[,] pnt1, double[,] pnt2)
        {
            double[] plane = new double[4];
            double[,] n = UnitLengthVector(pnt1, pnt2);
            double[,] m = Midpoint(pnt1, pnt2);

            double c = VectorDot(n, m);

            plane[0] = n[0, 0];
            plane[1] = n[1, 0];
            plane[2] = n[2, 0];
            plane[3] = c;

            return plane;
        }

        /// <summary>
        /// Finds the midpoint of 2 points
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns> mid </returns>
        static double[,] Midpoint(double[,] pt1, double[,] pt2)
        {
            double[,] mid = new double[3, 1];

            for (int row = 0; row < mid.GetLength(0); row++)
            {
                mid[row, 0] = (pt2[row, 0] + pt1[row, 0]) / 2;
            }

            return mid;
        }

        /// <summary>
        /// Finds the unit length vector normal from 2 points
        /// </summary>
        /// <param name="pnt1"></param>
        /// <param name="pnt2"></param>
        /// <returns> unit vector </returns>
        static double[,] UnitLengthVector(double[,] pnt1, double[,] pnt2)
        {
            double[,] unitVector = new double[3, 1];
            double[,] result = new double[3, 1];

            double[,] v = GetVector(pnt2, pnt1, 3);

            double magA = Math.Abs(Math.Sqrt(Math.Pow(v[0, 0], 2) + Math.Pow(v[1, 0], 2) + Math.Pow(v[2,0],2)));
            for (int row = 0; row < result.GetLength(0); row++)
            {
                unitVector[row,0] = v[row, 0] = v[row,0] / magA;
            }
  
            return unitVector;
        }

        /// <summary>
        /// Takes col and rowsize and returns the point
        /// associated with the col and checks if 2d or 3d
        /// </summary>
        /// <param name="col"></param>
        /// <param name="rowSize"></param>
        /// <returns> point </returns>
        static double[,] GetPoint(int col, int rowSize) 
        {
            double[,] result = new double[rowSize, 1];
            switch(rowSize)
            {
                case 2:
                    for (int row = 0; row < rowSize; row++)
                    {
                        result[row, 0] = twoDMatrix[row, col];

                    }
                    break;
                case 3:
                    for (int row = 0; row < rowSize; row++)
                    {
                        result[row, 0] = threeDMatrix[row, col];
                    }
                    break;
                default:
                    break;
            }
            return result;
            
        }

        /// <summary>
        /// Forms a line from given point and vector
        /// </summary>
        /// <param name="point"></param>
        /// <param name="vector"></param>
        /// <returns> line </returns>
        static double[,] FormLine(double[,] point, double[,] vector)
        {
            double[,] line = new double[2, 2];
            for (int row = 0; row < line.GetLength(0); row++)
            {
                line[row, 0] = point[row, 0];
                line[row, 1] = vector[row, 0];
            }



            return line;
        }

        /// <summary>
        /// Finds the distance to the line from a point
        /// </summary>
        /// <param name="line"></param>
        /// <param name="pnt"></param>
        /// <returns> distance </returns>
        static double DistanceToLine(double[,] line, double[,] pnt)
        {
            double distance = 0;
            double[,] p1 = GetPoint(0, 2);
            
            double[,] w = GetVector(pnt, p1, 2);
            double[,] v = { {line[1,0] }, {line[1,1]} };
            double vDotW = VectorDot(v, w);
            double magV = Math.Abs(Math.Sqrt(Math.Pow(v[0,0], 2) + Math.Pow(v[1,0], 2)));
            double magW = Math.Abs(Math.Sqrt(Math.Pow(w[0, 0], 2) + Math.Pow(w[1, 0], 2)));
            double denom = magV * magW;
            if(denom == 0)
            {
                Console.WriteLine("Division by zero!!");
                return 0;
            }
            double cos = vDotW / denom;
            double sin = Math.Sqrt(1 - Math.Pow(cos, 2));
            distance = magW * sin;


            return distance;
        }

        /// <summary>
        /// Computes the area of a triange in either 2d or 3d
        /// </summary>
        /// <param name="rowSize"></param>
        /// <returns> area </returns>
        static double AreaOfTriangle(int rowSize)
        {
            double area = 0;
            double value = 0;
            double[,] p1 = GetPoint(0, rowSize);
            double[,] p2 = GetPoint(1, rowSize);
            double[,] p3 = GetPoint(2, rowSize);

            double[,] a1 = GetVector(p2, p1, rowSize);
            double[,] a2 = GetVector(p3, p1, rowSize);
            if(rowSize == 3)
            {
                double[,] cross = Cross(a1, a2, rowSize);

                value = Math.Abs(Math.Sqrt(Math.Pow(cross[0, 0], 2) + Math.Pow(cross[1, 0], 2) + Math.Pow(cross[2, 0], 2)));

                area = Math.Abs(Math.Sqrt(value));
            } else
            {
                
                area = Math.Abs(.5*(a1[0,0] * a2[1,0] - a1[1,0] * a2[0,0]));
            }
           



            return area;
        }
        
        /// <summary>
        /// Takes 2 points and computes the vector
        /// </summary>
        /// <param name="p2"></param>
        /// <param name="p1"></param>
        /// <param name="rowSize"></param>
        /// <returns></returns>
        static double[,] GetVector(double[,] p2, double[,] p1, int rowSize)
        {
            double[,] result = new double[rowSize, 1];
            for (int row = 0; row < rowSize; row++)
            {
                result[row, 0] = p2[row, 0] - p1[row, 0];
            }


            return result;
        }

        /// <summary>
        /// Computes the cross product of 2 vectors in 3d
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="rowSize"></param>
        /// <returns></returns>
        static double[,] Cross(double[,] u, double[,] v, int rowSize)
        {
            double[,] result = new double[rowSize, 1];
            double u1 = u[0,0];
            double u2 = u[1,0];
            double u3 = u[2,0];
            double v1 = v[0,0];
            double v2 = v[1,0];
            double v3 = v[2,0];
            double x = 0;
            double y = 0;
            double z = 0;

            x = u2 * v3 - u3 * v2;
            y = u3 * v1 - u1 * v3;
            z = u1 * v2 - u2 * v1;

            result[0,0] = x;
            result[1, 0] = y;
            result[2, 0] = z;



            return result;
        }

        /// <summary>
        /// Computes the dot product of 2 vectors
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        static double VectorDot(double[,] vector1, double[,] vector2)
        {
            double result = 0;


            for (int row = 0; row < vector1.GetLength(0); row++)
            {
                result += vector1[row, 0] * vector2[row, 0];
                
            }
            return result;
        }

        /// <summary>
        /// Loads input files into a string array 
        /// </summary>
        /// <returns></returns>
        static string[] LoadInputFiles()
        {
            string inputFile1 = "test_input_1.txt";
            string inputFile2 = "test_input_2.txt";
            string inputFile3 = "test_input_3.txt";
            string inputFile4 = "test_input_4.txt";
            string inputFile5 = "3D_test_input_1.txt";
            string inputFile6 = "3D_test_input_2.txt";
            string[] inputFiles = { inputFile1, inputFile2, inputFile3, inputFile4, inputFile5, inputFile6 };
            return inputFiles;
        }

        /// <summary>
        /// Reads file and fills the input structure
        /// </summary>
        /// <param name="file"></param>
        static void ReadFile(string file)
        {
            try
            {
                StreamReader reader = new StreamReader(file);
                double value = 0;
                double[] arr = { 0, 0, 0 };
                
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();
                   
                    var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < data.Length; j++)
                    {

                        value = double.Parse(data[j]);
                        
                        inputMat.Add(value);
                       
                    }

                }

                reader.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found!");

            }
                

        }

        /// <summary>
        /// Loads 2d matrix from input structure
        /// </summary>
        static void Load2DMatrix()
        {
           
            int index = 0;
            for (int row = 0; row < twoDMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < twoDMatrix.GetLength(1); col++)
                {
                    twoDMatrix[row, col] = (double)inputMat[index];
                    index++;
                }
            }


        }

        /// <summary>
        /// Loads 3d matrix from input structure
        /// </summary>
        static void Load3DMatrix()
        {
            
            int index = 0;
            for (int row = 0; row < threeDMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < threeDMatrix.GetLength(1); col++)
                {

                    threeDMatrix[row, col] = (double)inputMat[index];
                    index++;
                }
            }


        }

        /// <summary>
        /// Displays matrix 
        /// </summary>
        /// <param name="arr"></param>
        static void Display(double[,] arr)
        {

            for (int row = 0; row < arr.GetLength(0); row++)
            {
                for (int col = 0; col < arr.GetLength(1); col++)
                {
                    Console.Write("{0,-5:0.####}\t", arr[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
