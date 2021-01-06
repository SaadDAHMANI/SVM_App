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



}
}