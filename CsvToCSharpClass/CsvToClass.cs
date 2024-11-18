

namespace CsvToCSharpClass.Library
{
    public class CsvToClass
    {
        public static string ConvertToUpperCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Split the string by underscore and convert each word to title case
            var words = input.Split('_').Select(word => System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower()));

            // Join the words back together
            string result = string.Join("", words);

            return result;
        }

        public static string CSharpClassCodeFromCsvFile(string filePath, string delimiter = ",", 
            string classAttribute = "", string propertyAttribute = "")
        {
            if (string.IsNullOrWhiteSpace(propertyAttribute) == false)
                propertyAttribute += "\n\t";
            if (string.IsNullOrWhiteSpace(propertyAttribute) == false)
                classAttribute += "\n";

            string[] lines = File.ReadAllLines(filePath);
            string[] columnNames = lines.First().Split(',').Select(str => str.Trim()).ToArray();
            // Using LINQ to convert each string in the list
            var resultList = columnNames.Select(ConvertToUpperCamelCase).ToList(); 
            string[] data = lines.Skip(1).ToArray();

            string className = Path.GetFileNameWithoutExtension(filePath);
            // use StringBuilder for better performance
            string code = String.Format("{0}public class {1} {{ \n", classAttribute, className);

            int columnIndex = 0;
            foreach (var columnName in resultList)
            {
                var name = Regex.Replace(columnName, @"[\s\.]", string.Empty, RegexOptions.IgnoreCase);
                if (string.IsNullOrEmpty(name))
                    name = "Column" + (columnIndex + 1);
                code += "\t" + GetVariableDeclaration(data, columnIndex, columnName, propertyAttribute) + "\n\n";
                columnIndex++;
            }

            code += "}\n";
            return code;
        }

        public static string GetVariableDeclaration(string[] data, int columnIndex, string columnName, string attribute = null)
        {
            string[] columnValues = data.Select(line => line.Split(',')[columnIndex].Trim()).ToArray();
            string typeAsString;

            if (AllDateTimeValues(columnValues))
            {
                typeAsString = "DateTime";
            }
            else if (AllIntValues(columnValues))
            {
                typeAsString = "int";
            }
            else if (AllDoubleValues(columnValues))
            {
                typeAsString = "double";
            }
            else
            {
                typeAsString = "string";
            }

            string declaration = String.Format("{0}public {1} {2} {{ get; set; }}", attribute, typeAsString, columnName);
            return declaration;
        }

        public static bool AllDoubleValues(string[] values)
        {
            double d;
            return values.All(val => double.TryParse(val, out d));
        }

        public static bool AllIntValues(string[] values)
        {
            int d;
            return values.All(val => int.TryParse(val, out d));
        }

        public static bool AllDateTimeValues(string[] values)
        {
            DateTime d;
            return values.All(val => DateTime.TryParse(val, out d));
        }

        // add other types if you need...
    }
}
