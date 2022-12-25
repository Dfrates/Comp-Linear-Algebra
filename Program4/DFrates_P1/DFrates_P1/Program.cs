using System;
using System.Collections.Generic;
using System.IO;

namespace DFrates_P1
{
    class Program
    {
        static List<double> input = new List<double>();
        static List<double[,]> triangleList = new List<double[,]>();
        static int colSize = 9;
        static double[,] inputMatrix;
       
        

        static void Main(string[] args)
        {

            Control();
            
        }

        static void Control()
        {
            string[] inputFiles = LoadInputFiles();
            ReadFile(inputFiles[0]);

            //create input matrix from input list structure 
            inputMatrix = CreateInputMatrix();

            // checks if matrix has correct column length
            if(inputMatrix.GetLength(1) == 9)
            {
                Console.WriteLine("****************** INPUT *****************");
                Display(inputMatrix);


                int rowCount = inputMatrix.GetLength(0);
                int triangles = rowCount;
                LoadTriangleList(triangles, inputMatrix);


                //======================================
                // PART A
                //======================================

                Console.WriteLine("****************** PART A *****************");
                PartA(triangleList);

                //======================================
                // PART B
                //======================================
                Console.WriteLine("****************** PART B *****************");
                PartB(triangleList);

                //======================================
                // PART C
                //======================================
                Console.WriteLine("****************** PART C *****************");
                PartC();
            } else
            {
                Console.WriteLine("Invalid matrix dimension of Nx{0}," +
                                  " input must be in form of Nx{1}",inputMatrix.GetLength(1),9);
            }

            
        }

        //======================================
        // PART A Methods
        //======================================
        #region Part A

        // Runs Part A
        //
        static void PartA(List<double[,]> triangleList)
        {
            // determines whether to cull or not on each triangle
            foreach (var t in triangleList)
            {
                Culling(t);
            }
        }

        // Performs culling on a triangle
        //
        static void Culling(double[,] triangle)
        {
            
            double[,] eyeLocat = EyeLocation(inputMatrix);
            double cull;
            double[,] centroid = FindCentroid(triangle);
            double[,] vector = new double[3, 1];
            double[,] viewVector = new double[3, 1];
            double[,] p = Shave(triangle, 0);
            double[,] q = Shave(triangle, 1);
            double[,] r = Shave(triangle, 2);
            double[,] u = Vector(p, q);
            double[,] w = Vector(p, r);
            
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                vector[i, 0] = eyeLocat[i, 0] - centroid[i, 0];
            }

            double mag1 = Magnitude(vector);


            for (int i = 0; i < viewVector.GetLength(0); i++)
            {
                viewVector[i, 0] = vector[i, 0] / mag1;
            }

            double[,] n = Normal(u, w);
            

            cull = VectorDot(n, viewVector);

            if(cull < 0)
            {
                Console.Write("Cullling output: {0}\n",0);
                Intensity(n);
               
                
            } else
            {
                Console.WriteLine("Culling output: {0}\n", 1);
                
            }
            

        }

        // Finds the intensity from each normal
        //
        static void Intensity(double[,] normal)
        {
            
            double[,] lightDir = LightDirection(inputMatrix);

            
            double magD = Magnitude(lightDir);
            double magN = Magnitude(normal);
            double denom = magD * magN;
            for (int i = 0; i < lightDir.GetLength(0); i++)
            {
                lightDir[i, 0] = -lightDir[i, 0];
            }
            double dot = VectorDot(lightDir, normal);

            double intense = dot / denom;
            Console.Write("Light Intesity: max({0:0.00},0)\n\n", intense);
           

        }

        // Gets normal from 2 vectors
        //
        static double[,] Normal(double[,] u, double[,] w)
        {
            double[,] cross = Cross(u, w);
            double mag2 = Magnitude(cross);
            double[,] n = new double[cross.GetLength(0), 1];

            for (int i = 0; i < cross.GetLength(0); i++)
            {
                n[i, 0] = cross[i, 0] / mag2;

            }
            return n;
        }

        // Performs vector dot product
        //
        static double VectorDot(double[,] v1, double[,] v2)
        {
            double result = 0;

            for (int i = 0; i < v1.GetLength(0); i++)
            {
                result += v1[i, 0] * v2[i, 0];
            }

            return result;
        }

        // Shaves matrix to column elements
        //
        static double[,] Shave(double[,] mat, int row)
        {
            double[,] result = new double[mat.GetLength(1), 1];

            for (int i = 0; i < mat.GetLength(1); i++)
            {
                result[i, 0] = mat[row, i];
            }
            return result;
        }

        // Performs cross poduct on vectors
        //
        static double[,] Cross(double[,] u, double[,] w)
        {


            double crossX = u[1, 0] * w[2, 0] - u[2, 0] * w[1, 0];
            double crossY = u[2, 0] * w[0, 0] - u[0, 0] * w[2, 0];
            double crossZ = u[0, 0] * w[1, 0] - u[1, 0] * w[0, 0];

            double[,] result = { { crossX }, { crossY }, { crossZ } };


            return result;
        }

        // Creates vector from 2 points
        //
        static double[,] Vector(double[,] pnt1, double[,] pnt2)
        {
            double[,] result = new double[pnt1.GetLength(0), 1];

            for (int row = 0; row < pnt1.GetLength(0); row++)
            {
                result[row, 0] = pnt2[row, 0] - pnt1[row, 0];
            }

            return result;
        }

        // Gets the magnitude of the given vector
        //
        static double Magnitude(double[,] vector)
        {
            return Math.Abs(Math.Sqrt(Math.Pow(vector[0, 0], 2) + Math.Pow(vector[1, 0], 2) + Math.Pow(vector[2, 0], 2)));
        }

        // Finds the centroid of the given triangle
        // format of 3x1 matrix
        //
        static double[,] FindCentroid(double[,] triangle)
        {
            double[,] result = new double[3, 1];

            for (int row = 0; row < result.Length; row++)
            {

                result[row, 0] = ((triangle[row, 0] + triangle[row, 1] + triangle[row, 2]) / 3);
            }
            return result;
        }

        // Gets eye location from first row of matrix
        // format of 3x1 matrix
        //
        static double[,] EyeLocation(double[,] mat)
        {
            double[,] result = new double[3, 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i, 0] = mat[0, i];
            }
            return result;
        }

        // Gets light direction from first row of matrix
        // format of 3x1 matrix
        //
        static double[,] LightDirection(double[,] mat)
        {
            double[,] result = new double[3, 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i, 0] = mat[0, i + result.Length];
            }
            return result;
        }

        #endregion


        //======================================
        // PART B Methods
        //======================================
        #region Part B

        // Runs part b from triangles
        //
        static void PartB(List<double[,]> triangleList)
        {
            double[,] point = PointOnPlane(0);
            double[,] normal = NormalToPlane(0);
            double[,] projectDir = ProjectionDirection();
            Console.WriteLine();
            Console.WriteLine("Parallel Projection of each point");
            Console.WriteLine("=================================");
            ParallelProjection(point, normal, projectDir);

            Console.WriteLine("\nPerspective Projection of each point");
            Console.WriteLine("====================================");
            PerspectiveProjection(point, normal);
            Console.WriteLine();
        }


        // Option A Parallel Projection of each point
        //
        static void ParallelProjection(double[,] point, double[,] normal, double[,] projectDir)
        {

            List<double[,]> points = LoadPoints(inputMatrix, 'a');
            List<double[,]> paraProjections = new List<double[,]>();

            foreach (var p in points)
            {
                double[,] x1 = new double[3, 1];
                double[,] qMinX = VectorSubtract(point, p);
                double dotN = VectorDot(qMinX, normal);
                double vDotN = VectorDot(projectDir, normal);
                for (int i = 0; i < point.GetLength(0); i++)
                {

                    x1[i, 0] = point[i, 0] + (dotN / vDotN) * projectDir[i, 0];
                }
                paraProjections.Add(x1);
            }


            double[,] paraProject = ToMatrix(paraProjections, inputMatrix.GetLength(0) - 1, inputMatrix.GetLength(1));
            Display(paraProject);

        }


        // Option B Perspective Projection of each point
        //
        static void PerspectiveProjection(double[,] point, double[,] normal)
        {
            List<double[,]> points = LoadPoints(inputMatrix, 'b');
            List<double[,]> perProjections = new List<double[,]>();
            double[,] x1 = new double[3, 1];
            double qDotN = 0;
            double xDotN = 0;
            double[,] matrixResult = new double[inputMatrix.GetLength(0),inputMatrix.GetLength(1)];


            foreach (var p in points)
            {
                x1 = new double[3, 1];
                qDotN = VectorDot(point, normal);
                xDotN = VectorDot(p, normal);
                for (int i = 0; i < point.GetLength(0); i++)
                {

                    x1[i, 0] = (qDotN / xDotN) * point[i, 0];
                }
                perProjections.Add(x1);

            }
            double[,] perProject = ToMatrix(perProjections,inputMatrix.GetLength(0),inputMatrix.GetLength(1));
            Display(perProject);
            
        }

        

        // Takes a list of 3x1 matrices and creates a matrix 
        //
        static double[,] ToMatrix(List<double[,]> list, int rowSize, int colSize)
        {
            double[,] result = new double[rowSize, colSize];
            int row = 0;
            int col = 0;

            foreach (var t in list)
            {
               
                for (int i = 0; i < t.GetLength(0); i++)
                {
                    result[row, col] = t[i, 0];
                    col++;
                    if (col == 9)
                    {
                        col = 0;
                        row++;
                    }
                }
                
            }

            return result;
        }

        // Loads points from input matrix 
        // option 'A' loads points from row 1-n
        // option 'B' loads points from row 0 at 7-9 to row n
        // points are in a list of 3x1 matrices
        //
        static List<double[,]> LoadPoints(double[,] input, char option)
        {
            List<double[,]> points = new List<double[,]>();

            switch (Char.ToLower(option))
            {
                //option for parallel projection
                case 'a':

                    double[,] point = new double[3, 1];
                    int count = 0;
                    for (int row = 1; row < input.GetLength(0); row++)
                    {
                        for (int col = 0; col < input.GetLength(1); col++)
                        {
                            point[count, 0] = input[row, col];
                            count++;
                            if (count == 3)
                            {
                                count = 0;
                                points.Add(point);
                                point = new double[3, 1];
                            }
                        }
                    }
                    return points;

                //option for perspective projection
                case 'b':

                    point = new double[3, 1];
                    for (int i = 0; i < point.GetLength(0); i++)
                    {
                        point[i, 0] = input[0, i + 6];
                    }
                    points.Add(point);
                    point = new double[3, 1];
                    count = 0;
                    for (int row = 1; row < input.GetLength(0); row++)
                    {
                        for (int col = 0; col < input.GetLength(1); col++)
                        {
                            point[count, 0] = input[row, col];
                            count++;
                            if (count == 3)
                            {
                                count = 0;
                                points.Add(point);
                                point = new double[3, 1];
                            }
                        }
                    }
                    for (int i = 0; i < point.GetLength(0); i++)
                    {
                        point[i, 0] = input[0, i + 6];
                    }


                    return points;

                default:
                    Console.WriteLine("error in case");
                    break;
            }

            return points;
        }

        // Performs vector subtraction on 2 vectors
        //
        static double[,] VectorSubtract(double[,] v1, double[,] v2)
        {
            double[,] result = new double[v1.GetLength(0), 1];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i, 0] = v1[i, 0] - v2[i, 0];
            }
            return result;
        }

        // Gets point on plane from a row of matrix
        // format of 3x1 matrix
        //
        static double[,] PointOnPlane(int row)
        {
            double[,] result = new double[3, 1];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i, 0] = inputMatrix[row, i];
            }
            return result;
        }
        
        // Gets projection direction from first row of matrix
        // format of 3x1 matrix
        //
        static double[,] ProjectionDirection()
        {
            double[,] result = new double[3, 1];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i, 0] = inputMatrix[0, i + 6];
            }
            return result;
        }



        #endregion

        //======================================
        // PART C Methods
        //======================================
        #region Part C

        // Runs part C
        //
        static void PartC()
        {
            Distance();
            Intersection();
        }

        // Finds the distance from the point to the plane
        //
        static void Distance()
        {
            double nDotQ = 0;
            double nDotX = 0;
            double nDotN = 0;
            double magN = 0;
            double t = 0;
            double distance = 0;

            //checks every point in matrix
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                double[,] inputLine = GetLine(i);
                double[,] pntPlane = PointOnPlane(i);
                double[,] normal = NormalToPlane(i);
                double[,] point = Point(inputLine);
                double[,] negateN = Negate(normal);

                magN = Magnitude(normal);
                nDotQ = VectorDot(negateN, pntPlane);
                nDotX = VectorDot(normal, point);
                nDotN = VectorDot(normal, normal);



                if (nDotN != 0)
                {
                    t = (nDotQ + nDotX) / nDotN;
                    distance = t * magN;
                    distance = Math.Abs(distance);

                    Console.WriteLine("Distance {0}: {1:0.000}\n", i + 1, distance);
                    
                }
                else
                {
                    Console.WriteLine("Division by Zero");
                }

            }





        }

        // Finds if point intersects with triangle
        //
        static void Intersection()
        {
            double[,] x = GetPoint1();
            double[,] y = GetPoint2();
            // Checks every triangle in matrix
            for (int i = 1; i < inputMatrix.GetLength(0); i++)
            {
                double[,] triangle = CreateTriangleVertices(inputMatrix, i);
                double[,] p1 = GetVertice(triangle, 0);
                double[,] p2 = GetVertice(triangle, 1);
                double[,] p3 = GetVertice(triangle, 2);
               
                double[,] v = Vector(x, y);
                double[,] w = Vector(p1, p2);
                double[,] z = Vector(p1, p3);
                double[,] negV = Negate(v);
                double[,] matrix = MatrixForm(w, z, negV);
                double[,] vector = VectorSubtract(x, p1);
                double[,] g2 = GaussElim(matrix, vector);
                double u1 = g2[0, 0];
                double u2 = g2[1, 0];
                double t = g2[2, 0];
                //determines if triangle intersects
                if(u1 > 0 && u1 < 1 && u2 > 0 && u2 < 1 && (u1 + u2) < 1)
                {
                    Console.Write("u1 = {0:0.##}\nu2 = {1:0.##}\nt = {2:0.##}\nIntersection\n\n",u1, u2, t);
                } else
                {

                    Console.Write("u1 = {0:0.##}\nu2 = {1:0.##}\nt = {2:0.##}\nNo Intersection\n\n", u1, u2, t);
                }


            }

            

        }

        // Puts the vectors into matrix form as column vectors
        //
        static double[,] MatrixForm(double[,] v1, double[,] v2, double[,] v3)
        {
            double[,] result = new double[3, 3];

            for (int row = 0; row < result.GetLength(0); row++)
            {
                result[row, 0] = v1[row, 0];
                result[row, 1] = v2[row, 0];
                result[row, 2] = v3[row, 0];
            }


            return result;
        }

        // Performs gaussian elimination
        // format of 3x1 matrix
        //
        static double[,] GaussElim(double[,] matrix, double[,] vector)
        {
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            
            int size = matrix.GetLength(0);
            double[,] a = matrix;
            double[,] b = vector;

            // creates zeroes in the last columns
            for (int p = 0; p < size; p++)
            {
                for (int row = p+1; row <size; row++)
                {
                    double m = a[row, p] / a[p, p];

                    // performs the row operation
                    for (int col = 0; col < size; col++)
                    {
                        a[row, col] = a[row, col] - (m * a[p, col]);
                    }
                    b[row, 0] = b[row, 0] - (m * b[p, 0]);
                }
            }

            double temp = 0;
            double u = 0;
            double[,] x = new double[size, 1];

            // forward elimination
            for (int row = size-1; row >= 0; row--)
            {
                for (int col = size-1; col >= 0 ; col--)
                { 
                    if (row == col)
                    {
                        u = a[row, col];
                        break;
                    }
                    else
                    {
                        temp += a[row, col] * b[col, 0];
                    }
                }
                b[row, 0] = (b[row, 0] - temp) / u;
                x[row, 0] = b[row,0];
                temp = 0;
            }

            return x;
        }

        // Creates the equation from the specified row of matrix and vector
        // format of nx1 matrix
        //
        static double[,] CreateEquation(double[,] matrix, double[,] b, int row) 
        {
            double[,] result = new double[matrix.GetLength(1) + b.GetLength(1), 1];

            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                result[i, 0] = matrix[row, i];
            }
            result[result.GetLength(0) - 1, 0] = b[row, 0];

            return result;
        }

        // Performs vector addition on 2 vectors
        // format of nx1 matrix
        //
        static double[,] AddVector(double[,] v1, double[,] v2)
        {
            double[,] result = new double[v1.GetLength(0), 1];

            for (int row = 0; row < result.GetLength(0); row++)
            {
                result[row, 0] = v1[row, 0] + v2[row, 0];
            }

            return result;
        }

        // Performs matrix dot product on 2 matrices
        //
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

        // Gets the point from column 0-2
        // format of 3x1 matrix
        //
        static double[,] GetPoint1()
        {
            double[,] result = new double[3, 1];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i, 0] = inputMatrix[0, i];
            }

            return result;
        }

        // Gets the point from column 3-6
        // format of 3x1 matrix
        //
        static double[,] GetPoint2()
        {
            double[,] result = new double[3, 1];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i, 0] = inputMatrix[0, i + 3];
            }

            return result;
        }

        // Gets the vertice of a specified triangle and a row
        // from the triangle. format of 3x1 matrix
        //
        static double[,] GetVertice(double[,] triangle, int row)
        {
            double[,] result = new double[3, 1];

            for (int i = 0; i < triangle.GetLength(1); i++)
            {
                result[i, 0] = triangle[row, i];
            }

            return result;
        }

        // Turns all elements of vector to their negative value
        //
        static double[,] Negate(double[,] vector)
        {
            double[,] result = new double[vector.GetLength(0), vector.GetLength(1)];

            for (int i = 0; i < vector.GetLength(0); i++)
            {
                for (int j = 0; j < vector.GetLength(1); j++)
                {
                    result[i, j] = -vector[i, j];
                }
            }

            return result;
        }

        // Gets a row from matrix
        // format of 9x1 matrix
        //
        static double[,] GetLine(int row)
        {
            double[,] result = new double[1, inputMatrix.GetLength(1)];

            for (int i = 0; i < result.GetLength(1); i++)
            {
                result[0, i] = inputMatrix[row, i];
            }

            return result;
        }

        

        // Gets the normal to the plane from a specified row
        // format of 3x1 matrix
        //
        static double[,] NormalToPlane(int row)
        {
            int offset = 3;
            double[,] result = new double[3, 1];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i, 0] = inputMatrix[row, i + offset];
            }
            return result;
        }

        // Gets the point from the specified row
        // format of 3x1 matrix
        //
        static double[,] Point(double[,] line)
        {
            int offset = 6;
            double[,] result = new double[3, 1];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                result[i, 0] = line[0, i + offset];
            }
            return result;
        }

        #endregion

        #region Helper Methods

        // Creates a list of triangles from the vertices
        // format of 3x3 matrix
        //
        static void LoadTriangleList(int triangleCount, double[,] mat)
        {
            for (int i = 1; i < triangleCount; i++)
            {
                triangleList.Add(CreateTriangleVertices(mat, i));
            }
        }

        // Creates the triangle vertice points from the matrix and a
        // specified index. format of 3x1 matrix
        //
        static double[,] CreateTriangleVertices(double[,] mat, int rowIndex)
        {
            double[,] result = new double[3, 3];

            if (rowIndex != 0)
            {
                int counter = 0;
                for (int row = 0; row < result.GetLength(0); row++)
                {
                    for (int col = 0; col < result.GetLength(1); col++)
                    {
                        result[row, col] = mat[rowIndex, counter];
                        counter++;
                    }
                }
            }
            else
            {
                Console.WriteLine("Cannot use row zero for triangles");
            }

            return result;
        }

        // Creates the input matrix from the 1d array
        //
        static double[,] CreateInputMatrix()
        {
            int rowSize = input.Count / colSize;
            double[,] result = new double[rowSize, colSize];

            int counter = 0;


            for (int row = 0; row < rowSize; row++)
            {
                for (int col = 0; col < colSize; col++)
                {
                    result[row, col] = input[counter];
                    counter++;
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
            string inputFile1 = "input_1.txt";
            string inputFile2 = "input_2.txt";
            string inputFile3 = "input_3.txt";


            string[] inputFiles = { inputFile1, inputFile2, inputFile3};
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

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var data = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < data.Length; i++)
                    {
                        value = double.Parse(data[i]);

                        input.Add(value);

                    }

                }

                reader.Close();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found!");

            }

        }
        // Displays a list of 2d arrays
        //
        static void DisplayList(List<double[,]> list)
        {
            double[,] result = new double[inputMatrix.GetLength(0) - 1, inputMatrix.GetLength(1)];
            
            foreach (var item in list)
            {
                //Display(item);
                Console.WriteLine();
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
                    
                    Console.Write("{0,-8:0.##}", arr[row, col]);
                }
                Console.WriteLine();
                
            }
            
        }

    }

    #endregion

}

