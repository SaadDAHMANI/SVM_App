//---------------------------------------------------------------------- 
// Required packages :
// dotnet add package Accord.MachineLearning --version 3.8.2-alpha
// dotnet add package Accord.MachineLearning.GPL --version 3.8.2-alpha
// 
// For genetic algorithm:
// dotnet add package Accord.Genetic --version 3.8.0 
//----------------------------------------------------------------------
// Written by : Saad Dahmani (s.dahmani@univ-bouira.dz)
//**********************************************************************

using System;
using System.Data;
using System.IO;
using System.Linq;
using Accord;
using Accord.IO;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;
using IOOperations;
using SupportVectorRegression;

namespace SVR_App2
{
    class Program
    {
         static SupportVectorMachine<IKernel> svm;
         static string fileName_LearningIn;
         static string fileName_LearningOut;
         static string fileName_TestingIn;
         static string fileName_TestingOut;
         static double[][] LearningIn;
         static double[] LearningOut;
         static double[][] TestingIn;
         static double[] TestingOut; 

         static string fileName_DST;
         static DataSerieTD DataSet;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello SVR!");

            // Loading Data :
             fileName_LearningIn ="QLearn_2.csv";
             fileName_LearningOut="CLearn_2.csv";
             fileName_TestingIn="QTest_2.csv";
             fileName_TestingOut="CTest_2.csv";
             fileName_DST="DST.csv";

             //LoadData();

              LoadDataDST2();

              //if (!Equals(DataSet, null)) { Console.WriteLine(DataSet.ToString());}  
             //EstimateSigmaAndComplexity();

            // Try SVR :
           Test_EOSVR();   
            

        }


        static void LoadDataDST()
        {
            string fileRoot = string.Format("{0}\\DataSource", System.IO.Directory.GetCurrentDirectory());

            string file1 = string.Format("{0}\\{1}", fileRoot, fileName_DST);

            if (System.IO.File.Exists(file1) == false)
            {
                Console.WriteLine("No file [{0}] found ...", file1);
                return;
            }

            CsvFileIO reader1 = new CsvFileIO(file1);
            DataSet = reader1.Read_DST();

            if (Equals(DataSet, null)) { Console.WriteLine("No data set .."); return; }
            if (Equals(DataSet.Data, null)) { Console.WriteLine("No data .."); return; }

            Console.WriteLine(DataSet.ToString());

            var x = DataSet.GetColumn(2);

            foreach (double itm in x)
            { Console.WriteLine(itm); }


            Console.WriteLine("There is {0} records in : {1}", DataSet.GetRowsCount(), DataSet.Name);
        }

        static void LoadDataDST2()
        {
            //string fileRoot = string.Format("{0}\\DataSource", System.IO.Directory.GetCurrentDirectory());


            string file1 = @"C:\Users\SD\Documents\Dataset_ANN_SVR\DataSet_Exemple.csv";    //Console.ReadLine();

            if (System.IO.File.Exists(file1) == false)
            {
                Console.WriteLine("No file [{0}] found ...", file1);
                return;
            }

            CsvFileIO reader1 = new CsvFileIO(file1);
            DataSet = reader1.Read_DST(false, false,true,false);

            if (Equals(DataSet, null)) { Console.WriteLine("No data set .."); return; }
            if (Equals(DataSet.Data, null)) { Console.WriteLine("No data .."); return; }

            //Console.WriteLine(DataSet.ToString());

            var x = DataSet.GetDataOfColumns(2,1);

            if (Equals(x, null)) { return; }
          
            foreach(double[] col in x)
            {
                //Console.WriteLine("Column : ");

                foreach (double value in col)
                {
                    Console.Write("{0}, ",value);
                }
                Console.WriteLine("");
            }
            

            Console.WriteLine("There is {0} records in : {1}", DataSet.GetRowsCount(), DataSet.Name);
        }




        #region Old_Voids

        static void EstimateSigmaAndComplexity()
        {
            DoubleRange range; // valid range will be returned as an out parameter
            Gaussian gaussian = Gaussian.Estimate(LearningIn, LearningIn.Length, out range);

             double numSigma = gaussian.Sigma;
             Console.WriteLine("Estimated Kernel Sigma ={0}", numSigma);
             Console.WriteLine(range.ToString());                

              // estimate complexity :
              
            var kernel = new Gaussian();
            double cl=kernel.EstimateComplexity(LearningIn);
            double ct=kernel.EstimateComplexity(TestingIn);
            Console.WriteLine("Complexity Learni ng ={0}, testing = {1}", cl, ct);

        }
        static void TrySVR(double[][]inputData, double []outputData)
        {
                     
             // Get only the input vector values (first column)
             double[][] inputs=inputData;

             // Get only the outputs (last column)
             double[] outputs=outputData; 

             // Sigma Kernel parameter
             double numSigma=1.222;
                    
             // Create the specified Kernel
             IKernel kernel = new Accord.Statistics.Kernels.Gaussian(numSigma);

             // SVM Params
             double numC=2; 
             double numT=0.001;
             double numEpsilon=0.001;   

              // Creates a new SMO for regression learning algorithm
             var teacher = new SequentialMinimalOptimizationRegression()
             {
                 // Set learning parameters
                 Complexity = numC,
                 Tolerance = numT,
                 Epsilon = numEpsilon,
                 Kernel = kernel
             };

            
             Console.WriteLine("Start SVM learning ...");
             
             // Use the teacher to create a machine
             svm = teacher.Learn(inputs, outputs);
              
              Console.WriteLine("SVM learning complete..");

                // Check if we got support vectors
            if (svm.SupportVectors.Length == 0)
            {
                Console.WriteLine("Sorry, No SVMs.");
                return;
            }

            // Show support vectors on the Support Vectors tab page
            double[][] supportVectorsWeights = svm.SupportVectors.InsertColumn(svm.Weights);
            
            Console.WriteLine(supportVectorsWeights.Length);

        }

        static void LoadData()
    {
        string fileRoot =string.Format("{0}\\DataSource",System.IO.Directory.GetCurrentDirectory());

        string file1 = string.Format("{0}\\{1}",fileRoot, fileName_LearningIn);
        string file2 = string.Format("{0}\\{1}",fileRoot, fileName_LearningOut);
        
        string file3 = string.Format("{0}\\{1}",fileRoot, fileName_TestingIn);  
        string file4 = string.Format("{0}\\{1}",fileRoot, fileName_TestingOut);

        if (System.IO.File.Exists(file1)==false)
        {
            Console.WriteLine("No file [{0}] found ...", file1);
            return;
        }

          if (System.IO.File.Exists(file2)==false)
        {
            Console.WriteLine("No file [{0}] found ...", file1);
            return;
        }
         
          if (System.IO.File.Exists(file3)==false)
        {
            Console.WriteLine("No file [{0}] found ...", file1);
            return;
        }

          if (System.IO.File.Exists(file4)==false)
        {
            Console.WriteLine("No file [{0}] found ...", file1);
            return;
        }

         CsvFileIO reader1 =new CsvFileIO(file1);
         DataSerie1D ds1 = reader1.Read_DS1(file1);
          DataSerie1D ds2 = reader1.Read_DS1(file2);
         DataSerie1D ds3 = reader1.Read_DS1(file3);
         DataSerie1D ds4 = reader1.Read_DS1(file4);
            
         if (Equals(ds1,null)){Console.WriteLine("No data set .."); return;}
         if (Equals(ds1.Data, null)){Console.WriteLine("No data ..");return;}
         LearningIn = ds1.GetArray();
         Console.WriteLine("There is {0} records in LEARNING inputs.", LearningIn.Length);   

         if (Equals(ds2,null)){Console.WriteLine("No data set .."); return;}
         if (Equals(ds2.Data, null)){Console.WriteLine("No data ..");return;}
         LearningOut = ds2.GetArray1D();   
         Console.WriteLine("There is {0} records in LEARNING outputs.", LearningOut.Length);   
        
         if (Equals(ds3,null)){Console.WriteLine("No data set .."); return;}
         if (Equals(ds3.Data, null)){Console.WriteLine("No data ..");return;}            
         TestingIn = ds3.GetArray();
         Console.WriteLine("There is {0} records in TESTING inputs.", TestingIn.Length);    

         if (Equals(ds4,null)){Console.WriteLine("No data set .."); return;}
         if (Equals(ds4.Data, null)){Console.WriteLine("No data ..");return;}
         TestingOut = ds4.GetArray1D();   
         Console.WriteLine("There is {0} records in TESTING outputs.", TestingOut.Length);   
      }

        static void Test_EOSVR()
    {
        // initilize svr :
       EOSVR eo_SVR = new EOSVR(LearningIn, LearningOut, TestingIn, TestingOut);
        
        // Setting params :
        eo_SVR.Sigma_Kernel=5.222;
        eo_SVR.Param_Complexity=1.222;
        eo_SVR.Param_Epsilon=0.001;
        eo_SVR.Param_Epsilon=0.001;
        eo_SVR.Param_Tolerance=0.001;

        // learning and testing :
        eo_SVR.Learn();
        
        if (Equals(eo_SVR.SupportVectorsWeights, null)){return;}
        Console.WriteLine("There is {0} vecotors.", eo_SVR.SupportVectorsWeights.Length);

        Statistics statL = new Statistics(LearningOut, eo_SVR.Computed_LearningOutputs);
        Console.WriteLine("Learing indexes : {0}", statL.ToString(4));

        Statistics statT = new Statistics(TestingOut, eo_SVR.Computed_TestingOutputs);
        Console.WriteLine("Testing indexes : {0}", statT.ToString(4));

        Console.WriteLine("________________________________________________________");

        eo_SVR.LearnEO();
        Console.WriteLine("Best index (R2)= {0}", eo_SVR.BestScore);
        Console.WriteLine("Best learning index= {0} | Best testing index = {1}", eo_SVR.BestLearningScore, eo_SVR.BestTestingScore);
        
        foreach(double value in eo_SVR.BestSolution)
        {
            Console.WriteLine("Param = {0}", value);
        }
    }

        #endregion


    }
}        
