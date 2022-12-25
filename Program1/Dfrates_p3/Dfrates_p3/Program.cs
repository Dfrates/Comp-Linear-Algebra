using System;
using System.IO;

namespace Dfrates_p3
{
    class Program
    {
        static void Main(string[] args)
        {
            RunP3();

        }

        private static void RunP3()
        {
            //Initializes all matrices by filling them from file
            try
            {
                int[,] mat1 = FillMat<int>(String.Format(@"{0}/Program1/Matrices/dfrates_{1}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 1));

                int[,] mat2 = FillMat<int>(String.Format(@"{0}/Program1/Matrices/dfrates_{1}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 2));

                double[,] mat3 = FillMat<double>(String.Format(@"{0}/Program1/Matrices/dfrates_{1}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 3));

                int[,] mat4 = FillMat<int>(String.Format(@"{0}/Program1/Matrices/dfrates_{1}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 4));

                int[,] mat5 = FillMat<int>(String.Format(@"{0}/Program1/Matrices/dfrates_{1}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), 5));

                //after creating matrices do the dot product on all combinations
                DotMats(mat1, mat2, mat3, mat4, mat5);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void IntDotMats(int[,] mat1, int[,] mat2, int id1, int id2)
        {
            //sets matrix 1 sizes
            int mat1Rows = mat1.GetLength(0);
            int mat1Cols = mat1.GetLength(1);

            //sets matrix 2 sizes 
            int mat2Rows = mat2.GetLength(0);
            int mat2Cols = mat2.GetLength(1);


            int[,] result = new int[mat1Rows, mat2Cols];

            //checks if columns of matrix 1 match rows of matrix 2
            if (ArithCheck(mat1, mat2))
            {
                //uses id1 and id2 to output the combination of the matrices
                string fileName = String.Format(@"{0}/Program1/Outputs/dfrates_p3_out{1}{2}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), id1, id2);

                Console.WriteLine("Created File: {0}", fileName);

                StreamWriter writer = new StreamWriter(fileName);
                //clears buffer, does not write anything to file if removed
                writer.AutoFlush = true;
                
                //iterates over matrix 1 rows
                for (int row = 0; row < mat1Rows; row++)
                {
                    //iterates over matrix 2 columns
                    for (int col2 = 0; col2 < mat2Cols; col2++)
                    {
                        //iterates over matrix1 columns, then adds the product of matrix 1
                        //at given location with matrix 2 at given location.
                        //it then adds that to results giving us the dot product of the
                        //two matrice and then outputs that to a file
                        for(int col = 0; col < mat1Cols; col++)
                        {
                            result[row, col2] += mat1[row, col] * mat2[col, col2];
                            
                           
                        }
                        writer.Write((String.Format("{0,-10}", result[row, col2])));

                    }
                    writer.WriteLine();

                }

            }
            
            


        }

        //exactly like IntDotMats except double to account for fomrmatting when writing to file
        private static double[,] DoubleDotMats(double[,] mat1, double[,] mat2, int id1, int id2)
        {
            int mat1Rows = mat1.GetLength(0);
            int mat1Cols = mat1.GetLength(1);
            int mat2Rows = mat2.GetLength(0);
            int mat2Cols = mat2.GetLength(1);

            double[,] result = new double[mat1Rows, mat2Cols];

            if (ArithCheck(mat1, mat2))
            {
                string fileName = String.Format(@"{0}/Program1/Outputs/dfrates_p3_out{1}{2}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), id1, id2);

                Console.WriteLine("Created File: {0}", fileName);

                StreamWriter writer = new StreamWriter(fileName);
                writer.AutoFlush = true;

                for (int row = 0; row < mat1Rows; row++)
                {
                    for (int col = 0; col < mat2Cols; col++)
                    {

                        for (int k = 0; k < mat1Cols; k++)
                        {
                            result[row, col] += mat1[row, k] * mat2[k, col];


                        }
                        writer.Write((String.Format("{0,-10:0.00}", result[row, col])));

                    }
                    writer.WriteLine();
                }

            }
            return result;
        }

        //calls every combination of the dot product of the matrices
        private static void DotMats(int[,] mat1, int[,] mat2, double[,] mat3, int[,] mat4, int[,] mat5)
        {
            IntDotMats(mat1, mat1, 1, 1);
            IntDotMats(mat1, mat2, 1, 2);
            DoubleDotMats(ConvertToDouble(mat1), mat3, 1, 3);
            IntDotMats(mat1, mat4, 1, 4);
            IntDotMats(mat1, mat5, 1, 5);

            IntDotMats(mat2, mat1, 2, 1);
            IntDotMats(mat2, mat2, 2, 2);
            DoubleDotMats(ConvertToDouble(mat2), mat3, 2, 3);
            IntDotMats(mat2, mat4, 2, 4);
            IntDotMats(mat2, mat5, 2, 5);

            DoubleDotMats(mat3, ConvertToDouble(mat1), 3, 1);
            DoubleDotMats(mat3, ConvertToDouble(mat2), 3, 2);
            DoubleDotMats(mat3, mat3, 3, 3);
            DoubleDotMats(mat3, ConvertToDouble(mat4), 3, 4);
            DoubleDotMats(mat3, ConvertToDouble(mat4), 3, 5);

            IntDotMats(mat4, mat1, 4, 1);
            IntDotMats(mat4, mat2, 4, 2);
            DoubleDotMats(ConvertToDouble(mat4), mat3, 4, 3);
            IntDotMats(mat4, mat4, 4, 4);
            IntDotMats(mat4, mat5, 4, 5);

            IntDotMats(mat5, mat1, 5, 1);
            IntDotMats(mat5, mat2, 5, 2);
            DoubleDotMats(ConvertToDouble(mat5), mat3, 5, 3);
            IntDotMats(mat5, mat4, 5, 4);
            IntDotMats(mat5, mat5, 5, 5);
            


        }

        //converts every element of matrix to type double
        private static double[,] ConvertToDouble(int[,] mat)
        {
            double[,] arr = new double[mat.GetLength(0), mat.GetLength(1)];
            for (int row = 0; row < mat.GetLength(0); row++)
            {
                for (int col = 0; col < mat.GetLength(1); col++)
                {
                    arr[row, col] = mat[row, col];
                }
            }
            return arr;
        }


        //fills matrix using data from input file
        private static T[,] FillMat<T>(string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader(fileName);

                //gets row size, column size and type from first
                //three lines of input file
                int rowSize = GetRowSize(fileName);
                int colSize = GetColSize(fileName);
                char type = GetType(fileName);


                int[,] intMat = new int[rowSize, colSize];
                double[,] doubleMat = new double[rowSize, colSize];
                int valueInt;
                double valueDouble;

                //Used to ensure reader starts at the correct line
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();

                switch (type)
                {
                    case 'i':

                        //reads line, then splits line into multiple strings
                        //then parses each string to correct data type
                        while (!reader.EndOfStream)
                        {

                            for (int i = 0; i < rowSize; i++)
                            {
                                var line = reader.ReadLine();
                                var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                                for (int j = 0; j < colSize; j++)
                                {
                                    valueInt = int.Parse(data[j]);
                                    intMat[i, j] = valueInt;

                                }

                            }

                        }
                        reader.Close();
                        return intMat as T[,];

                    case 'd':
                        while (!reader.EndOfStream)
                        {

                            for (int i = 0; i < rowSize; i++)
                            {
                                var line = reader.ReadLine();
                                var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                                for (int j = 0; j < colSize; j++)
                                {
                                    valueDouble = double.Parse(data[j]);
                                    doubleMat[i, j] = valueDouble;

                                }

                            }

                        }
                        reader.Close();
                        return doubleMat as T[,];

                    default:
                        Console.WriteLine("Error in case");
                        return null;

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        private static int GetRowSize(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            var line = reader.ReadLine();
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            reader.Close();
            return int.Parse(data[0]);
        }

        private static int GetColSize(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            reader.ReadLine();
            var line = reader.ReadLine();
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(data[0]);
        }

        private static char GetType(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            reader.ReadLine();
            reader.ReadLine();
            var line = reader.ReadLine();
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return char.Parse(data[0].ToLower());
        }

        //Checks if columns of first matrix are equal to rows of the second matrix
        //if not then they cannot be computed
        private static bool ArithCheck<T>(T[,] mat1, T[,] mat2)
        {
            if (mat1.GetLength(1) == mat2.GetLength(0))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Cannot compute! Rows and/or columns do not match");

                return false;
            }
        }


    }
}
