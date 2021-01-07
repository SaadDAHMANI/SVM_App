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

private int _TrainingPourcentage; 
public int TrainingPourcentage
 {get {return _TrainingPourcentage;} set {_TrainingPourcentage=Math.Max(0, Math.Min(value, 100));}}

public int TestingPourcentage
{get {return 100-_TrainingPourcentage;}}

 public void Format(int targetColumnIndex)
 {
    if (targetColumnIndex<0){return;}
    if(_TrainingPourcentage<=0){ return;}
    if(Equals(DataSet,null)){return;}
    if(Equals(DataSet.Data, null)){return;}

  
 }











}
}