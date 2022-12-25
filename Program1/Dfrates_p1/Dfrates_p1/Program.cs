using System;
using System.IO;


namespace Dfrates_p1
{
    class Program
    {

        public const string LASTNAME = "Frates";
        public const string FIRSTNAME = "Daniel";


        static void Main(string[] args)
        {
            RunP1();

        }



        private static void RunP1()
        {
            int matRow = 6;
            int matCol = 13;

            //Initializes matrices
            int[,] mat1 = new int[FIRSTNAME.Length, LASTNAME.Length];
            int[,] mat2 = new int[LASTNAME.Length, FIRSTNAME.Length];
            double[,] mat3 = new double[FIRSTNAME.Length, LASTNAME.Length];
            int[,] mat4 = new int[matRow, matCol];
            int[,] mat5 = new int[matRow, matCol];


            BuildAllMats(mat1, mat2, mat3, mat4, mat5);
        }




        //Fills all matrices and then outputs those matrices to files with respective names
        private static void BuildAllMats(int[,] mat1, int[,] mat2, double[,] mat3, int[,] mat4, int[,] mat5)
        {
            FillMat1(mat1);
            FillMat2(mat2);
            FillMat3(mat3);
            FillMat4(mat4);
            FillMat5(mat5);
            OutputMatToFile(mat1, "1");
            OutputMatToFile(mat2, "2");
            OutputMatToFile(mat3, "3");
            OutputMatToFile(mat4, "4");
            OutputMatToFile(mat5, "5");
            Console.WriteLine("Succesfully created text files");
        }


        //Fills matrix with starting value of 1 and increases each value by 1
        private static void FillMat1(int[,] mat)
        {
            int counter = 1;
            for (int row = 0; row < mat.GetLength(0); row++)
            {
                for (int col = 0; col < mat.GetLength(1); col++)
                {
                    mat[row, col] = counter;
                    counter++;
                }
            }
        }

        //Fills matrix with starting value of 3 and increases each value by 3
        private static void FillMat2(int[,] mat)
        {
            int counter = 3;
            for (int col = 0; col < mat.GetLength(1); col++)
            {
                for (int row = 0; row < mat.GetLength(0); row++)
                {
                    mat[col, row] = counter;
                    counter += 3;
                }
            }
        }

        //Fills matrix with starting value of 0.4 and increases each value by 0.3
        private static void FillMat3(double[,] mat)
        {
            double counter = .4;
            for (int col = 0; col < mat.GetLength(1); col++)
            {
                for (int row = 0; row < mat.GetLength(0); row++)
                {
                    mat[col, row] = counter;
                    counter += .3;
                }
            }
        }

        //Fills matrix with starting value of 2 and increases each value by 2
        private static void FillMat4(int[,] mat)
        {
            int counter = 2;
            for (int col = 0; col < mat.GetLength(1); col++)
            {
                for (int row = 0; row < mat.GetLength(0); row++)
                {
                    mat[row, col] = counter;
                    counter += 2;
                }
            }
        }

        //Fills matrix with starting value of -7 and increases each value by 1
        private static void FillMat5(int[,] mat)
        {
            int counter = -7;
            for (int row = 0; row < mat.GetLength(0); row++)
            {
                for (int col = 0; col < mat.GetLength(1); col++)
                {
                    mat[row, col] = counter;
                    counter++;
                }
            }
        }


        //Writes matrix to file and uses matNum as an identifier for naming the file
        private static void OutputMatToFile<T>(T[,] mat, string matNum)
        {


            try
            {
                //File name gets user profile and uses identifier matNum to name file
                //ex: mat1 would have identifier of 1
                string fileName = String.Format(@"{0}/Program1/Matrices/dfrates_{1}mat",
                            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), matNum);

                StreamWriter writer = new StreamWriter(fileName);

                //Flushes buffer of stream writer
                //If removed, nothing will be written to file
                writer.AutoFlush = true;

                //writes size row on first line
                writer.WriteLine(mat.GetLength(0));

                //writes size of column on second line
                writer.WriteLine(mat.GetLength(1));

                //checks if mat is type integer or double to have proper formatting
                if (mat.GetType() == typeof(int[,]))
                {
                    //writes type to file, "I" for integer
                    writer.WriteLine("I");


                    for (int row = 0; row < mat.GetLength(0); row++)
                    {
                        for (int col = 0; col < mat.GetLength(1); col++)
                        {
                            writer.Write((String.Format("{0,-10}", mat[row, col])));
                        }
                        writer.WriteLine();

                    }
                }
                // checks if double to format to 0.00
                else
                {
                    //writes type to file, "D" for double
                    writer.WriteLine("D");

                    for (int row = 0; row < mat.GetLength(0); row++)
                    {
                        for (int col = 0; col < mat.GetLength(1); col++)
                        {
                            writer.Write(String.Format("{0,-10:0.00}", mat[row, col]));
                        }
                        writer.WriteLine();

                    }
                }
                Console.WriteLine("Created matrix file at: {0}", fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }

    }
}
