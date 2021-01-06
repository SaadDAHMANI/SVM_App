using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using IOOperations;

namespace SupportVectorRegression
{

public class DataFormater
{
public DataFormater(DataSerieTD dataset )
{
    DataSet=dataset;
}


public DataSerieTD DataSet {get; set;}

private int _TrainingCount; 
public int TrainingCount
 {get {return _TrainingCount;} set {_TrainingCount=Math.Max(0, Math.Min(value, 100));}}



}
}