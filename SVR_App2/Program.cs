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
        
         static string fileName_DST;
         static DataSerieTD DataSet;
         static DataFormater df;   
        static void Main(string[] args)
        {
             Console.WriteLine("Hello SVR!");
             string file = @"C:\Users\SD\Documents\Dataset_ANN_SVR\DataSet_Exemple.csv";    //Console.ReadLine();
   
              LoadData(file);


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

        static void LoadData(string file)
        {
            //string fileRoot = string.Format("{0}\\DataSource", System.IO.Directory.GetCurrentDirectory());


            //string file = @"C:\Users\SD\Documents\Dataset_ANN_SVR\DataSet_Exemple.csv";    //Console.ReadLine();

            if (System.IO.File.Exists(file) == false)
            {
                Console.WriteLine("No file [{0}] found ...", file);
                return;
            }

            CsvFileIO reader1 = new CsvFileIO(file);
            DataSet = reader1.Read_DST(false, false,true,false);

            if (Equals(DataSet, null)) { Console.WriteLine("No data set .."); return; }
            if (Equals(DataSet.Data, null)) { Console.WriteLine("No data .."); return; }

            Console.WriteLine("There is {0} records in : {1}", DataSet.GetRowsCount(), DataSet.Name);

            //Console.WriteLine(DataSet.ToString());

            df = new DataFormater(DataSet);
            df.TrainingPourcentage = 70;

            df.Format(0, 0, 1, 2);
            
            if (!Equals(df.TrainingInput, null)) { Console.WriteLine("Training = {0}", df.TrainingInput.Length); }

            Console.WriteLine("Training : ");

            foreach (double value in df.TrainingOutput)
            {
                Console.Write("{0}, ", value);
            }

            
            Console.WriteLine("Testing : ");

            foreach (double value in df.TestingOutput)
            {
                Console.Write("{0}, ", value);
            }


        }


    }
}        
