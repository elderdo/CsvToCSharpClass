
DirectoryInfo directoryInfo = new DirectoryInfo("Data");
FileInfo[] fileInfo = directoryInfo.GetFiles("*.csv");

foreach (var file in fileInfo)
{
    string path = file.FullName;
    string fileName = file.Name;
    //var cSharpClass = CsvToClass.CSharpClassCodeFromCsvFile(@"Data\InputFile.csv", ",", "[DelimitedRecord(\",\")]", "[FieldOptional()]");
    var cSharpClass = CsvToClass.CSharpClassCodeFromCsvFile(path);
    File.WriteAllText(@"Data\" + fileName.Split(".")[0] + ".cs", cSharpClass);
}


