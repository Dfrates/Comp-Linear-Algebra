/* Name: Daniel Frates
 * Class: CS 2300
 * Date: 03/17/22
 * Assignment 3 part 2
 * Summary: Read in a 2d matrix and compute the eigen values and the eigen vectors
 * of the matrix. then find the eigen decomposition and determine whether
 * it is the same as input matrix.
 */
using System;
using System.IO;

namespace DFrates_part2
{
    class Program
    {

        static double[,] inputMatrix = new double[2, 3];
        static double[,] matrix = new double[2, 2];
        static double[,] vector = new double[2, 1];


        static void Main(string[] args)
        {

            string[] inputs = LoadInputFiles();


            Control(inputs[2]);

        }

        /// <summary>
        /// Controls what file is being read
        /// </summary>
        /// <param name="inputFile"></param>
        static void Control(string inputFile)
        {
            //input
            ReadFile(inputFile);
            LoadVector();
            LoadMatrix();
            Console.WriteLine("===================");
            Console.WriteLine("Input 1");
            Console.WriteLine("===================");
            Display(inputMatrix);
            

            Console.WriteLine();

            //output 1
            Console.WriteLine("===================");
            Console.WriteLine("Output 1");
            Console.WriteLine("===================");
            double[,] diagMatrix = CreateEigenValueMatrix();
            Display(diagMatrix);

            Console.WriteLine();

            //output 2
            Console.WriteLine("===================");
            Console.WriteLine("Output 2");
            Console.WriteLine("===================");
            double[,] vectorMatrix = CreateEigenVectorMatrix(diagMatrix);
            double[,] r = Normalize(vectorMatrix);
            Display(r);

            Console.WriteLine();

            //output 3
            Console.WriteLine("===================");
            Console.WriteLine("Output 3");
            Console.WriteLine("===================");
            double[,] vectorMatrix1 = CreateEigenVectorMatrix(diagMatrix);
            double[,] eigenDecomp = EigenDecomp(vectorMatrix, diagMatrix);
            Display(eigenDecomp);

            Console.WriteLine();

            //output 4
            Console.WriteLine("===================");
            Console.WriteLine("Output 4");
            Console.WriteLine("===================");
            Console.WriteLine(Compare(matrix, eigenDecomp));

        }

        /// <summary>
        /// Compares matrices values and determines if they are equal
        /// </summary>
        /// <param name="mat1"></param>
        /// <param name="mat2"></param>
        /// <returns></returns>
        static int Compare(double[,] mat1, double[,] mat2)
        {
            if (mat1.GetLength(0) == mat2.GetLength(0) && mat1.GetLength(1) == mat2.GetLength(1))
            {
                for (int row = 0; row < mat1.GetLength(0); row++)
                {
                    for (int col = 0; col < mat1.GetLength(1); col++)
                    {
                        if(mat1[row, col] != mat2[row,col])
                        {
                            return 0;
                        }
                    }
                }
            } 

            return 1;
        }

        /// <summary>
        /// computes eigen decompostion
        /// </summary>
        /// <param name="eigenVectorMatrix"></param>
        /// <param name="diagMatrix"></param>
        /// <returns></returns>
        static double[,] EigenDecomp(double[,] eigenVectorMatrix, double[,] diagMatrix)
        {
            double[,] norm = Normalize(eigenVectorMatrix);
            double[,] transpose = Transpose(norm);
            double[,] firstDot = MatrixDot(norm, diagMatrix);
            double[,] secondDot = MatrixDot(firstDot, transpose);
            
            return secondDot;
        }

        /// <summary>
        /// Creates eigen vector from a given eigen value
        /// </summary>
        /// <param name="eigenValue"></param>
        /// <returns></returns>
        static double[,] CreateEigenVector(double eigenValue)
        {
            double[,] eigenVector = new double[2, 1];
            double[,] result = new double[2,2];
            double x1 = 0;
            double x2 = 0;
            double[,] tempMat = CopyMatrix(matrix);
            double[] resultx = new double[2];
            double[] resulty = new double[2];
            double[,] gauss = new double[2, 2];


            
            tempMat[0, 0] -= eigenValue;
            tempMat[tempMat.GetLength(0) - 1, tempMat.GetLength(1) - 1] -= eigenValue;

            for (int col = 0; col < tempMat.GetLength(1); col++)
            {
                resultx[col] = tempMat[0, col];
                resulty[col] = tempMat[1, col];

            }

            resultx[0] -= eigenValue;
            resulty[1] -= eigenValue;
            double a = resultx[0];
            double b = tempMat[0, 1];
            double c = tempMat[1, 0];
            

        

            if (a == 0 && c != 0)
            {

                tempMat = RowPivot(tempMat);

                tempMat[1, 1] = ((-1 * b * c) / a) + tempMat[1, 1];
                x2 = 1;
                x1 = -1 * (b / a);
                result[0,0] = x1;
                result[1,0] = x2;


            }
            else if (a == 0 && c == 0 && b != 0)
            {

                tempMat = ColumnPivot(tempMat);
                tempMat[1, 1] = ((-1 * b * c) / a) + tempMat[1, 1];
                x2 = 1;
                x1 = -1 * (b / a);
                result[0,0] = x1;
                result[1,0] = x2;



            }
            else if (a == 0 && b == 0 && c == 0)
            {


                tempMat = RowPivot(tempMat);
                tempMat = ColumnPivot(tempMat);
                tempMat[1, 1] = ((-1 * b * c) / a) + tempMat[1, 1];
                x1 = 1;
                x2 = -1 * (b / a);
                result[0,0] = x2;
                result[1,0] = x1;
            } else
            {
                tempMat = GaussianElim(tempMat);
                tempMat[1, 1] = ((-1 * b * c) / a) + tempMat[1, 1];

                
                x2 = 1;
                x1 = -1 * (b / a);
                result[0,0] = x1;
                result[1,0] = x2;


            }

            result[0, 1] = a;

            result[1, 1] = b;



            return result;
            
        }

        /// <summary>
        /// Creates a matrix of the eigen vectors
        /// </summary>
        /// <param name="eigenDiag"></param>
        /// <returns></returns>
        static double[,] CreateEigenVectorMatrix(double[,] eigenDiag)
        {
            double[,] vector1 = CreateEigenVector(eigenDiag[0, 0]);
            double[,] vector2 = CreateEigenVector(eigenDiag[1, 1]);

            double[,] result = { {vector1[0,0], vector1[1,0] },{vector2[0,0], vector2[1,0] } };
            return result;
        } 

        /// <summary>
        /// performs a row pivot on a matrix
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        static double[,] RowPivot(double[,] mat)
        {
            double[,] result = new double[2, 2];
            double next = 0;
            double last = 0;
            double rowLength = mat.GetLength(0);
            double colLength = mat.GetLength(1);


            for (int col = 0; col < colLength; col++)
            {
                for (int row = 0; row < rowLength; row++)
                {
                    last = mat[row, col];
                    if (col == 0)
                    {
                        if (row < rowLength -1)
                        {
                            next = mat[row + 1, col];
                            result[row + 1, col] = last;
                        } else
                        {
                            next = mat[row, col + 1];
                            result[row, col + 1] = last;
                        }
                    } else
                    {
                        if (row == 0)
                        {
                            result[row, col - 1] = last;
                        } else
                        {
                            result[row - 1, col] = next;
                        }
                    }
                }
            }

            return result;

        }

        /// <summary>
        /// performs a column pivot on matrix
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        static double[,] ColumnPivot(double[,] mat)
        {
            double rowLength = mat.GetLength(0);
            double colLength = mat.GetLength(1);
            double[,] result = new double[2,2];
            for (int row = 0; row < rowLength; row++)
            {
                for (int col = 0; col < colLength; col++)
                {

                   if (row == 0)
                   {
                        if (col > 0)
                        {
                            result[row, col - 1] = mat[row, col];
                        } else
                        {
                            result[row + 1, col] = mat[row, col];
                        }
                    } else
                    {
                        if(col > 0)
                        {
                            result[row - 1, col] = mat[row, col];
                        }
                    }
                    


                }
            }
                    return result;
        }
        
        /// <summary>
        /// perfroms gaussian elimination to get 0 in place of c
        /// </summary>
        /// <param name="mat1"></param>
        /// <returns></returns>
        static double[,] GaussianElim(double[,] mat1)
        {
            double gauss = (mat1[1, 0] / mat1[0, 0]) * -1;
            double[,] elim = new double[2, 2] { { 1, 0 },
                           { gauss, 1 } };
            
            return MatrixDot(elim, mat1);


        }

        /// <summary>
        /// transposes a matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static double[,] Transpose(double[,] matrix)
        {
            double[,] result = new double[2, 2];

            result = CopyMatrix(matrix);
            result[1, 0] = matrix[0, 1];
            result[0, 1] = matrix[1, 0];



            return result;
            
        }

        /// <summary>
        /// Normalizes the eigen vector matrix
        /// </summary>
        /// <param name="eigenVectorMatrix"></param>
        /// <returns></returns>
        static double[,] Normalize(double[,] eigenVectorMatrix)
        {
           
            double norm1 = Math.Sqrt(Math.Pow(eigenVectorMatrix[0, 0], 2) + 1);
            double norm2 = Math.Sqrt(Math.Pow(eigenVectorMatrix[1, 0], 2) + 1);
            double[,] result = { {eigenVectorMatrix[0,0]/norm1, 1/norm1 }, {eigenVectorMatrix[1,0]/norm2, 1/ norm2 } };


            return result;

        }

        /// <summary>
        /// creates a matrix with eigen values in the diagonals
        /// </summary>
        /// <returns></returns>
        static double[,] CreateEigenValueMatrix()
        {
            double[] quad;


            double eigenValue1 = 0;
            double eigenValue2 = 0;

            double diagA = matrix[0, 0];
            double diagD = matrix[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];

            double a = 1;
            double b = (diagA + diagD) * -1;
            double c = Determinant(matrix);

            quad = Quadratic(a, b, c);

            eigenValue1 = quad[0];
            eigenValue2 = quad[1];

            return CreateDiagMatrix(eigenValue1, eigenValue2);


        }

        /// <summary>
        /// creates a diagonal matrix that contains eigen values
        /// </summary>
        /// <param name="eigen1"></param>
        /// <param name="eigen2"></param>
        /// <returns></returns>
        static double[,] CreateDiagMatrix(double eigen1, double eigen2)
        {
            double[,] diag = new double[2, 2];

            for (int row = 0; row < diag.GetLength(0); row++)
            {
                for (int col = 0; col < diag.GetLength(1); col++)
                {
                    diag[row, col] = 0;
                }
            }

            diag[0, 0] = eigen1;
            diag[diag.GetLength(0) - 1, diag.GetLength(1) - 1] = eigen2;

            return diag;
        }

        /// <summary>
        /// computes the quadratic formula on 3 values
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        static double[] Quadratic(double a, double b, double c)
        {

            double[] result = new double[2];

            double pos = (-1 * b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / 2 * a;
            double neg = (-1 * b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / 2 * a;
            result[0] = pos;
            result[1] = neg;

            return result;

        }

        /// <summary>
        /// find the determinant of a matrix
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

            if (rowLength == colLength)
            {

                ad = matrix[0, 0] * matrix[rowLength - 1, colLength - 1];
                bc = matrix[0, colLength - 1] * matrix[rowLength - 1, 0];

                result = ad - bc;
                return result;

            }
            else
            {
                Console.WriteLine("Matrix is not square, cannot find determinant!");
                return result;
            }

        }


        /// <summary>
        /// performs matrix dot product
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        static double[,] MatrixDot(double[,] matrix1, double[,] matrix2)
        {
            double[,] result = new double[2, 2];


            for (int row = 0; row < matrix1.GetLength(0); row++)
            {
                for (int col = 0; col < matrix2.GetLength(1); col++)
                {

                    for (int k = 0; k < matrix1.GetLength(1); k++)
                    {
                        result[row, col] += matrix1[row, k] * matrix2[k, col];


                    }
                    

                }
               
            }







            return result;
        }

        /// <summary>
        /// copies content of a given matrix to another
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static double[,] CopyMatrix(double[,] matrix)
        {
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    result[row, col] = matrix[row, col];
                }
            }
            return result;
        }

        /// <summary>
        /// loads input files into a array of strings
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
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found!");

            }

        }

        /// <summary>
        /// takes off vector of input matrix and leaves the 2x2 matrix
        /// </summary>
        static void LoadMatrix()
        {
            for (int row = 0; row < inputMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < inputMatrix.GetLength(1) - 1; col++)
                {
                    matrix[row, col] = inputMatrix[row, col];
                }
            }
        }

        /// <summary>
        /// takes the vector at the end of the 2x2 matrix
        /// </summary>
        static void LoadVector()
        {
            int col = inputMatrix.GetLength(1) - 1;
            for (int row = 0; row < inputMatrix.GetLength(0); row++)
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
                    Console.Write("{0,-5:0.####}\t", arr[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
