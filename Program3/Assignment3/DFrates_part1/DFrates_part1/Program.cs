/* Name: Daniel Frates
 * Class: CS 2300
 * Date: 03/17/22
 * Assignment 3 part 1
 * Summary: reads a 2x3 matrix and creates a vector and a 2x2 matrix. then compute
 * x where it satisifies Ax = b
 */
using System;
using System.IO;

namespace DFrates_part1
{
    class Program
    {

        static double[,] inputMatrix = new double[2,3];
        
        static double[,] vector = new double[2, 1];


        static void Main(string[] args)
        {

            string[] inputs = LoadInputFiles();
           

            Control(inputs[0]);

        }

        /// <summary>
        /// controls input files
        /// </summary>
        /// <param name="inputFile"></param>
        static void Control(string inputFile)
        {
            ReadFile(inputFile);
            LoadVector();

            Console.Write("Input\n--------\n");
            double[,] matrix = CreateMatrix();
            Display(matrix);

            Console.Write("\nOutput\n--------\n");
            double[,] xvector = CreateMatrixInverseVector(matrix);
            Display(xvector);

            

           
        }

        /// <summary>
        /// creates x vector from the inverse matrix and input vector
        /// </summary>
        /// <param name="inverse"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        static double[,] CreateXVector(double[,] inverse, double[,] vector)
        {
            double[,] result = new double[2, 1];

            for(int row = 0; row < result.GetLength(0); row++)
            {
                for(int col = 0; col < result.GetLength(1); col++)
                {
                    result[row, col] = (inverse[row, col]*vector[0,0]) + (inverse[row, col + 1]*vector[1,0]);

                }
            }

            return result;
        }

        /// <summary>
        /// finds the determinant of a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static double Determinant(double[,] matrix)
        {
            double result = 0;
            double ad = 0;
            double bc = 0;
            int rowLength = matrix.GetLength(0);
            int colLength = matrix.GetLength(1);

            if(rowLength == colLength)
            {

                ad = matrix[0, 0] * matrix[rowLength - 1, colLength - 1];
                bc = matrix[0, colLength - 1] * matrix[rowLength - 1, 0];

                result = ad - bc;
                return result;

            }
            else
            {
                Console.WriteLine("Matrix is not square, cannot find determinant!");
            }



            return result;
        }

        /// <summary>
        /// takes matrix and computes the inverse vector
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static double[,] CreateMatrixInverseVector(double[,] matrix)
        {

            int rowLength = matrix.GetLength(0);
            int colLength = matrix.GetLength(1);

            double[,] resultInverse = new double[rowLength, colLength];
            double[,] resultVector = new double[2, 1];
            double[,] flipped = new double[rowLength, colLength];
            double[,] aZero = new double[2, 1];

            double determinant = Determinant(matrix);

            if (determinant != 0)
            {
                double inverse = 1/(determinant);
                double a = matrix[0, 0];
                double d = matrix[rowLength - 1, colLength - 1];
                double b = matrix[0, colLength - 1];
                double c = matrix[rowLength - 1, 0];
                flipped[0, 0] = d;
                flipped[rowLength - 1, colLength - 1] = a;
                flipped[0, colLength - 1] = b * -1;
                flipped[rowLength - 1, 0] = c * -1;

                for (int row = 0; row < rowLength; row++)
                {
                    for (int col = 0; col < colLength; col++)
                    {
                        resultInverse[row, col] = flipped[row, col] * inverse;
                    }
                    
                }

                resultVector = CreateXVector(resultInverse,vector);


            } else
            {
                aZero = DeterminantIsZero(resultInverse);
                return aZero;
            }
            return resultVector;
        }

        /// <summary>
        /// determines if determinant is zero and continues
        /// to computes vector x
        /// </summary>
        /// <param name="inverse"></param>
        /// <returns></returns>
        static double[,] DeterminantIsZero(double[,] inverse)
        {
            double[,] resultVectorX = CreateXVector(inverse,vector);
            double[,] resultVectorB = CreateXVector(inputMatrix, resultVectorX);
        
            double a = inputMatrix[0, 0];
            double b = inputMatrix[0, 1];
            double c = inputMatrix[1, 0];
            double d = inputMatrix[1, 1];

           

            if (b != 0)
            {
                resultVectorX[0, 0] = 1;
                resultVectorX[1, 0] = (resultVectorB[0, 0] - a) / b;
                
            } else if (b == 0 && a !=0)
            {
                resultVectorX[1, 0] = 1;
                resultVectorX[0, 0] = resultVectorB[0, 0] / a;
            } else if (b == 0 && a == 0 && resultVectorB[0,0] != 0)
            {
                Console.WriteLine("Inconsistent");
            } else if (a == 0 && b == 0 && resultVectorB[0,0] == 0)
            {
                Console.WriteLine("Underdetermined");
            } else
            {
                Console.WriteLine("Error in case");
            }

            double result = c * resultVectorX[0, 0] + d * resultVectorX[1, 0];

            if (result != resultVectorB[1, 0])
            {
                Console.WriteLine("Inconsistent");
            } else
            {
                Console.WriteLine("Underdetermined");
            }

            return resultVectorX;

        }

        /// <summary>
        /// Creates matrix from input matrix
        /// </summary>
        /// <returns></returns>
        static double[,] CreateMatrix()
        {
            int removeCol = inputMatrix.GetLength(1) - 1;
            double[,] result = new double[2, removeCol];
            for(int row = 0; row < inputMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < removeCol; col++)
                {
                    result[row, col] = inputMatrix[row, col];
                }
            }
            return result;
        }

        /// <summary>
        /// loads input files and stores them in an array of strings
        /// </summary>
        /// <returns></returns>
        static string[] LoadInputFiles()
        {
            string inputFile1 = "test_input_1.txt";
            string inputFile2 = "test_input_2.txt";
            string inputFile3 = "test_input_3.txt";
            string inputFile4 = "test_input_4.txt";
            
            string[] inputFiles = { inputFile1, inputFile2, inputFile3, inputFile4 };
            return inputFiles;
        }

        /// <summary>
        /// reads file and loads values into input matrix
        /// </summary>
        /// <param name="file"></param>
        static void ReadFile(string file)
        {
            try
            {
                StreamReader reader = new StreamReader(file);
                double value = 0;

                for (int i = 0; i < inputMatrix.GetLength(0); i++)
                {
                    //reads first line of matrix
                    var line = reader.ReadLine();
                    //splits the line into multiple pieces and removes spaces
                    //data becomes an array of strings
                    var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < inputMatrix.GetLength(1); j++)
                    {
                        //changes data to double and asigns that to matrix
                        value = double.Parse(data[j]);
                        inputMatrix[i, j] = value;

                    }
                }
                reader.Close();
            } catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found!");

            }
            
        }

        /// <summary>
        /// takes vector from input matrix
        /// </summary>
        static void LoadVector()
        { 
            int col = inputMatrix.GetLength(1) - 1;
            for(int row = 0; row < inputMatrix.GetLength(0); row++)
            {
                vector[row, 0] = inputMatrix[row, col];
            }
        }

        /// <summary>
        /// displays matrix
        /// </summary>
        /// <param name="arr"></param>
        static void Display(double[,] arr)
        {
           
            for (int row = 0; row < arr.GetLength(0); row++)
            {
                for (int col = 0; col < arr.GetLength(1); col++)
                {
                    Console.Write("{0,-5:0.####}", arr[row,col]);
                }
                Console.WriteLine();
            }
        }
     }
}
