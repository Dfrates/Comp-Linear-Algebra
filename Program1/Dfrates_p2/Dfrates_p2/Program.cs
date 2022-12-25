using System;
using System.IO;

namespace Dfrates_p2
{
    class Program
    {


        static void Main(string[] args)
        {
            RunP2();

        }

        private static void RunP2()
        {
            //Inititializes matrices using Fillmat which reads from file and fills matrix
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

                AddMats(mat1, mat2, mat3, mat4, mat5);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //Adds integer matrices
        private static void IntAddMats(int[,] mat1, int[,] mat2, int id1, int id2)
        {
            int rowLength = mat1.GetLength(0);
            int colLength = mat2.GetLength(1);

            //Checks if matrices have same dimensions
            if (ArithCheck(mat1, mat2))
            {
                //creates file using id1 and id2 to show combination of matrices that were added
                string fileName = String.Format(@"{0}/Program1/Outputs/dfrates_p2_out{1}{2}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), id1, id2);

                Console.WriteLine("Created File: {0}", fileName);

                StreamWriter writer = new StreamWriter(fileName);
                //Clears streamwriter buffer
                //if removed, it will not work
                writer.AutoFlush = true;

                int[,] result = new int[rowLength, colLength];

                for (int row = 0; row < mat1.GetLength(0); row++)
                {
                    for (int col = 0; col < mat1.GetLength(1); col++)
                    {
                 
                        result[row, col] = mat1[row, col] + mat2[row, col];
                        writer.Write((String.Format("{0,-10}", result[row, col])));

                    }

                    writer.WriteLine();
                }

            }
            
        }

        //similar to method above but takes double matrix paramaters instead of integer matrix
        //Generics would not work for some reason, so I had to be slightly redundant with
        //writing this method
        private static void DoubleAddMats(double[,] mat1, double[,] mat2, int id1, int id2)
        {
            int rowLength = mat1.GetLength(0);
            int colLength = mat2.GetLength(1);

            if (ArithCheck(mat1, mat2))
            {
                string fileName = String.Format(@"{0}/Program1/Outputs/dfrates_p2_out{1}{2}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), id1, id2);

                Console.WriteLine("Created File at: {0}", fileName);

                StreamWriter writer = new StreamWriter(fileName);
                writer.AutoFlush = true;

                double[,] result = new double[rowLength, colLength];

                for (int row = 0; row < mat1.GetLength(0); row++)
                {
                    for (int col = 0; col < mat1.GetLength(1); col++)
                    {
                        result[row, col] = mat1[row, col] + mat2[row, col];

                        writer.Write((String.Format("{0,-10:0.00}", result[row, col])));
                    }
                    writer.WriteLine();
                }

            }
            
        }

        //calls every combination of matrix 
        private static void AddMats(int[,] mat1, int[,] mat2, double[,] mat3, int[,] mat4, int[,] mat5)
        {
            IntAddMats(mat1, mat1, 1, 1);
            IntAddMats(mat1, mat2, 1, 2);
            DoubleAddMats(ConvertToDouble(mat1), mat3, 1, 3);
            IntAddMats(mat1, mat4, 1, 4);
            IntAddMats(mat1, mat5, 1, 5);

            IntAddMats(mat2, mat2, 2, 2);
            DoubleAddMats(ConvertToDouble(mat2), mat3, 2, 3);
            IntAddMats(mat2, mat4, 2, 4);
            IntAddMats(mat2, mat5, 2, 5);

            DoubleAddMats(mat3, mat3, 3, 3);
            DoubleAddMats(mat3, ConvertToDouble(mat4), 3, 4);
            DoubleAddMats(mat3, ConvertToDouble(mat4), 3, 5);

            IntAddMats(mat4, mat4, 4, 4);
            IntAddMats(mat4, mat5, 4, 5);

            IntAddMats(mat5, mat5, 5, 5);


        }

        //helper method that converts every element in an integer
        //array to type double
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


        //Used to fill matrix with values from file
        private static T[,] FillMat<T>(string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader(fileName);


                //reads first line for row size
                int rowSize = GetRowSize(fileName);

                //reads second line for column size
                int colSize = GetColSize(fileName);

                //reads third line for type
                char type = GetType(fileName);


                int[,] intMat = new int[rowSize, colSize];
                double[,] doubleMat = new double[rowSize, colSize];
                int valueInt;
                double valueDouble;

                //This ensures that the reader starts at the correct line since
                //the files first 3 lines are used for row size, column size, and
                //type, if removed it will cause issues
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();

                //uses type to determine what matrix to return
                switch (type)
                {
                    case 'i':
                        while (!reader.EndOfStream)
                        {

                            for (int i = 0; i < rowSize; i++)
                            {
                                //reads first line of matrix
                                var line = reader.ReadLine();
                                //splits the line into multiple pieces and removes spaces
                                //data becomes an array of strings
                                var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                                for (int j = 0; j < colSize; j++)
                                {
                                    //changes data to integers and asigns that to matrix
                                    valueInt = int.Parse(data[j]);
                                    intMat[i, j] = valueInt;

                                }

                            }

                        }
                        reader.Close();
                        return intMat as T[,];

                    //same as above except it creates double matrix
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

        //reads first line to get row size
        private static int GetRowSize(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            var line = reader.ReadLine();
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            reader.Close();
            return int.Parse(data[0]);
        }

        //reads second line to get column size
        private static int GetColSize(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            reader.ReadLine();
            var line = reader.ReadLine();
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return int.Parse(data[0]);
        }

        //reads third line to get type 
        private static char GetType(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            reader.ReadLine();
            reader.ReadLine();
            var line = reader.ReadLine();
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return char.Parse(data[0].ToLower());
        }

        //checks if rows and columns of both matrices match
        //matrix addition can only be done on matrices that are of
        //equal size
        private static bool ArithCheck<T>(T[,] mat1, T[,] mat2)
        {
            if (mat1.GetLength(0) == mat2.GetLength(0) && mat1.GetLength(1) == mat2.GetLength(1))
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
