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
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Tea JOIN Country ON Tea.id_country = Country.id_country", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Tea");

            // Task 3
            DataTable teaTable = dataSet.Tables["Tea"];

            var teaByCountry = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() });

            Console.WriteLine("Назва країни, та кількість сортів чаю:");
            foreach (var item in teaByCountry)
                Console.WriteLine(
                    $"Країна: {item.Country,-10}" +
                    $"Кількість сортів чаю: {item.Count,-10}"
                );

            var averageWeightByCountry = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, AverageWeight = g.Average(r => r.Field<double>("weight")) });

            Console.WriteLine("\nСередня кількість грамів чаю по кожній країні:");
            foreach (var item in averageWeightByCountry)
                Console.WriteLine(
                    $"Країна: {item.Country,-10}" +
                    $"Середня кількість гр.: {Math.Round(item.AverageWeight, 2),-10}"
                );

            var cheapestTeaByCountry = teaTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "Китай")
                .OrderBy(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдешевші сорти чаю (Китай):");
            foreach (var row in cheapestTeaByCountry)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-20}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var mostExpensiveTeaByCountry = teaTable.AsEnumerable()
                .Where(r => r.Field<string>("country_name") == "Египет")
                .OrderByDescending(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдорожчі сорти чаю (Египет):");
            foreach (var row in mostExpensiveTeaByCountry)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-20}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var cheapestTea = teaTable.AsEnumerable()
                .OrderBy(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдешевші сорти чаю (всі країни):");
            foreach (var row in cheapestTea)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-20}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var mostExpensiveTea = teaTable.AsEnumerable()
                .OrderByDescending(r => r.Field<decimal>("cost"))
                .Take(3);

            Console.WriteLine("\nТри найдорожчі сорти чаю (всі країни):");
            foreach (var row in mostExpensiveTea)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-20}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );
        }
    }
}
