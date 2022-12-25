//*****************************************************
// Name: Daniel Frates
// Date: 3 May 2022
// Assignment: Page Ranking and Linear Binary
//             classification.
//
// Description: Part A is a simple page rank algorithm
//              that uses the power method to find the
//              rank of the pages. Part B implements
//              the perceptron algorithm to find the
//              correct weights and to determine if a
//              set of features are in the group.
//*****************************************************
using System;
using System.Collections.Generic;
using System.IO;

namespace DFrates_P5
{
    class Program
    {
        static List<string> input = new List<string>();

        static double[,] matrixPA;
        static double[,] matrixPB;
        static double[,] trainMat;
        static Random rand = new Random();

        


        static void Main(string[] args)
        {
            string inputA = "test_input_A.txt";
            string inputB = "test_input_B.txt";
            string trainingInputB = "training_input_B.txt";

            LoadInput(inputA);
            LoadInput(inputB);
            LoadInput(trainingInputB);

            Control();
        }

        static void Control()
        {
            
            

            Console.WriteLine("************************ Input ************************");

            matrixPA = ReadFile(input[0]);
            Display(matrixPA);

            Console.WriteLine("\n************************ Part A ************************");
            PartA();

            Console.WriteLine("\n************************ Part B ************************");
            PartB();


        }

        //Part A PowerMethod for page ranking
        static void PartA()
        {
            if (CheckMatrix(matrixPA))
            {
                
                double[,] rank = PowerMethod(matrixPA);
                Console.WriteLine("R: (");
                Display(rank);
                Console.WriteLine(")");

                
            }
        }

        //Part B Perceptron aligorithm to classify data
        static void PartB()
        {
            matrixPB = ReadFile(input[1]);
            trainMat = ReadFile(input[2]);
            Display(matrixPB);

            double[,] W = RandomValues(matrixPB.GetLength(1), 1);


            //loops over 1000 iterations
            for (int j = 0; j < 1000; j++)
            {
                //loops over each set and updates the weights
                for (int i = 0; i < matrixPB.GetLength(0); i++)
                {
                    double[,] X = GetVector(trainMat, i, 1);

                    W = Perceptron(W, X, trainMat[i, 0]);
                   
                }
                

            }

            WriteToFile("updated_weights.txt", W);

            InGroup(matrixPB, W);

        }


        //Determines if the feature sets are in the group based
        //on the final calculated weights.
        static void InGroup(double[,] features, double[,] W)
        {
         
            for (int i = 0; i < W.GetLength(0); i++)
            {
                double[,] F = GetVector(features, i, 0);
                

                double value = VectorDot(F, W);
                if(value >= 0)
                {
                    Console.WriteLine("Feature sets are in the group");
                } else
                {
                    Console.WriteLine("Feature sets are NOT in the group");
                }

            }
            

        }


        //Performs the perceptron alogrith to find the weights
        static double[,] Perceptron(double[,] W, double[,] X, double Y)
        {
            double e;

            double state = VectorDot(W, X);
            

            if(state >= 0)
            {
                state = 1;

            } else
            {
                state = 0;
            }

            e = Y - state;

            for (int i = 0; i < W.GetLength(0); i++)
            {
                W[i, 0] = W[i, 0] + e * X[i, 0];
            }

            return W;

        }

        //creates a 2d array of random values based on row and col
        static double[,] RandomValues(int rowSize, int colSize)
        {
            double[,] result = new double[rowSize, colSize];


            for (int row = 0; row < rowSize; row++)
            {
                for (int col = 0; col < colSize; col++)
                {
                    result[row,col] = rand.Next(1, 20);
                }
            }


            return result;
        }

        //Gets the row vector for a given matrix
        static double[,] GetVector(double[,] v, int row, int startCol)
        {
            double[,] result = new double[v.GetLength(1) - startCol, 1];


            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i , 0] = v[row, startCol];
                startCol++;
            }

            return result;
        }

        //Checks matrix columns add to 1
        static bool CheckMatrix(double[,] mat)
        {
            double[,] result = new double[matrixPA.GetLength(0), mat.GetLength(1)];
            double value = 0;
            for (int col = 0; col < mat.GetLength(1); col++)
            {
                value = 0;
                for(int row = 0; row < mat.GetLength(0); row++)
                {
                    value += mat[row, col];
                    
                }
                if(value < 1 && value > 1.0000000000000005)
                {
                    Console.WriteLine("Column {0} does not add to 1!", col);
                    return false;
                }
            }


            return true;
        }

        //Performs the power method to find R
        static double[,] PowerMethod(double[,] mat)
        {
            Random rand = new Random();
            double[,] result = new double[mat.GetLength(0), 1];

            double[,] r = new double[mat.GetLength(0), 1];
            double maxValR = 0;
            double maxValY = 0;
            double lambda = 0;
            double lastLambda = 0;
            double tolerance = .0005;
            int maxIterations = 100;
            double[,] y = new double[r.GetLength(0), 1];

            //estimate eigenvector
            for(int i = 0; i < r.GetLength(0); i++)
            {
                r[i, 0] = rand.Next(1, 5);
            }

            //find j in r
            maxValR = r[0, 0];
            for (int i = 0; i < r.GetLength(0); i++)
            {
                
                if(maxValR < Math.Abs(r[i,0]))
                {
                    maxValR = Math.Abs(r[i, 0]);
                }
            }

            //divide r by j
            for (int i = 0; i < r.GetLength(0); i++)
            {
                r[i, 0] = r[i, 0] / maxValR;
            }

            for (int i = 0; i < maxIterations; i++)
            {
                y = MatrixDot(matrixPA, r);
            }


            for (int i = 1; i < maxIterations; i++)
            {

                //find j in y
                maxValY = Math.Abs(y[0, 0]);
                for (int j = 0; j < y.GetLength(0); j++)
                {

                    if (maxValY < Math.Abs(y[j, 0]))
                    {
                        maxValY = Math.Abs(y[j, 0]);
                    }
                }

                // restart if max value of y is 0
                if(maxValY == 0)
                {
                    i = 0;
                    //estimate new eigenvector
                    for (int k = 0; k < r.GetLength(0); k++)
                    {
                        r[k, 0] = rand.Next(1, 5);
                    }

                } else
                {
                    lambda = maxValY;

                    //divide r by j in y
                    for (int x = 0; x < r.GetLength(0); x++)
                    {
                        r[x, 0] = y[x, 0] / maxValY;
                    }

                    
                    if (Math.Abs(lambda - lastLambda) < tolerance)
                    {

                        return r;
                        
                    } else
                    {
                        lastLambda = lambda;
                    }
                    if(i == maxIterations)
                    {
                        Console.WriteLine("Max iterations exceeded");
                    }
                }
                
            }

           
            

            return result;
        }


        //Perfroms matrix dot product
        static double[,] MatrixDot(double[,] mat1, double[,] mat2)
        {
            double[,] result = new double[mat1.GetLength(0), mat2.GetLength(1)];

            for (int row = 0; row < mat1.GetLength(0); row++)
            {
                for (int col = 0; col < mat2.GetLength(1); col++)
                {

                    for (int k = 0; k < mat1.GetLength(1); k++)
                    {
                        result[row, col] += mat1[row, k] * mat2[k, col];
                    }
                }

            }


            return result;
        }


        //performs vector dot product
        static double VectorDot(double[,] v1, double[,] v2)
        {
            double result = -1;

            if(v1.GetLength(0) == v2.GetLength(0))
            {
                for (int i = 0; i < v1.GetLength(0); i++)
                {
                    result += v1[i, 0] * v2[i, 0];
                }
                return result;
            } else
            {
                Console.WriteLine("invalid format v1: {0} != v2: {1}", v1.GetLength(0), v2.GetLength(0));
            }
            
            

            return result;
        }

        //writes to a file
        static void WriteToFile(string fileName, double[,] mat)
        {
            StreamWriter writer = new StreamWriter(fileName);
            writer.AutoFlush = true;
            for (int row = 0; row < mat.GetLength(0); row++)
            {
                for (int col = 0; col < mat.GetLength(1); col++)
                {
                    writer.Write("{0,-7:0.###}", mat[row, col]);
                }
                writer.WriteLine();
            }
        }

        //loads input files into list
        static void LoadInput(string fileName)
        {
            input.Add(fileName);
        }

        //reads file and outputs a matrix of correct dimensions
        static double[,] ReadFile(string fileName)
        {
            
            try
            {
                StreamReader reader = new StreamReader(fileName);
                StreamReader sizeRead = new StreamReader(fileName);
                int row = 0;
                int colSize;
                

                var x = sizeRead.ReadLine();
                var y = x.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                colSize = y.Length;
                int rowSize = 1;

                while(!sizeRead.EndOfStream)
                {
                    sizeRead.ReadLine();
                    rowSize++;

                }
                

                double[,] matrix = new double[rowSize, colSize];
                

                while (!reader.EndOfStream && row < rowSize)
                {
                   
                    var line = reader.ReadLine();
                    var data = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);


                    for (int i = 0; i < data.Length; i++)
                    {
                        matrix[row, i] = double.Parse(data[i]);
                        
                    }
                    row++;
                    

                }
                
                return matrix;
            } catch(FileNotFoundException)
            {
                Console.WriteLine("File not found!");
                return null;
            }
            
            

        }


        //Displays a matrix
        static void Display(double[,] mat)
        {
            for(int row = 0; row < mat.GetLength(0); row++)
            {
                for (int col = 0; col < mat.GetLength(1); col++)
                {
                    Console.Write("{0,-7:0.###}", mat[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
