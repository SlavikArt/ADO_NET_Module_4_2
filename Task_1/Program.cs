using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string connectionString = ConfigurationManager.ConnectionStrings["TeaDB"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Tea", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Tea");

            // Task 1: insert, update, delete
            DataTable teaTable = dataSet.Tables["Tea"];
            DataRow newRow = teaTable.NewRow();
            newRow["name"] = "Чай Черная Смородина";
            newRow["description"] = "Чай Черная Смородина - это ароматный черный чай с нотками черной смородины. Отлично подходит для утреннего чаепития.";
            newRow["id_country"] = 2;
            newRow["cost"] = 8.99;
            newRow["weight"] = 1000;
            newRow["id_type"] = 1;
            teaTable.Rows.Add(newRow);

            DataRow editRow = teaTable.Rows[5];
            editRow["name"] = "Матча";

            DataRow deleteRow = teaTable.Rows[10];
            teaTable.Rows.Remove(deleteRow);

            adapter.Update(dataSet, "Tea");
        }
    }
}
