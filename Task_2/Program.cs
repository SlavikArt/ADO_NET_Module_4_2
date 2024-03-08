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

            // Task 2
            DataTable teaTable = dataSet.Tables["Tea"];

            var cherryTea = teaTable.AsEnumerable()
                .Where(r => r.Field<string>("description")
                .Contains("вишни"));

            Console.WriteLine("Чай, в описі якої зустрічається вишня:");
            foreach (var row in cherryTea)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name")}\n" +
                    $"Опис: {row.Field<string>("description")}"
                );

            var costRangeTea = teaTable.AsEnumerable()
                .Where(r => r.Field<decimal>("cost") >= 10
                && r.Field<decimal>("cost") <= 20);

            Console.WriteLine("\nЧай з собівартістю у вказаному діапазоні:");
            foreach (var row in costRangeTea)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-20}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var weightRangeTea = teaTable.AsEnumerable()
                .Where(r => r.Field<double>("weight") >= 200
                && r.Field<double>("weight") <= 500);

            Console.WriteLine("\nЧай з кількістю грамів у вказаному діапазоні:");
            foreach (var row in weightRangeTea)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-20}" +
                    $"Вага: {row.Field<double>("weight"),-10}"
                );

            var specificCountryTea = teaTable.AsEnumerable()
                .Where(r => r.Field<int>("id_country") == 1
                || r.Field<int>("id_country") == 2);

            Console.WriteLine("\nЧай із зазначених країн:");
            foreach (var row in specificCountryTea)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-20}" +
                    $"ID країни: {row.Field<int>("id_country"),-10}"
                );
        }
    }
}
