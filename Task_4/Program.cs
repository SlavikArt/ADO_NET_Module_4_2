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
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM Tea " +
                "JOIN Country ON Tea.id_country = Country.id_country " +
                "JOIN TeaType ON Tea.id_type = TeaType.id_type",
                connection
            );
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Tea");

            // Task 4
            DataTable teaTable = dataSet.Tables["Tea"];

            var top3CountriesByTeaVariety = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(3);

            Console.WriteLine("Топ-3 країн за кількістю сортів чаю:");
            foreach (var item in top3CountriesByTeaVariety)
                Console.WriteLine(
                    $"Країна: {item.Country,-8}" +
                    $"Кількість сортів чаю: {item.Count,-8}"
                );

            var top3CountriesByTeaWeight = teaTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, TotalWeight = g.Sum(r => r.Field<double>("weight")) })
                .OrderByDescending(g => g.TotalWeight)
                .Take(3);

            Console.WriteLine("\n\nТоп-3 країн за кількістю грамів чаю:");
            foreach (var item in top3CountriesByTeaWeight)
                Console.WriteLine(
                    $"Країна: {item.Country,-8}" +
                    $"Загальна кількість грамів: {item.TotalWeight,-8}"
                );

            Console.WriteLine();

            string[] teaTypes = { "Зеленый чай", "Черный чай" };
            foreach (var teaType in teaTypes)
            {
                var top3TeaByWeight = teaTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == teaType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nТоп-3 види чаю \"{teaType}\" за кількістю грамів:");
                foreach (var row in top3TeaByWeight)
                    Console.WriteLine(
                        $"Назва: {row.Field<string>("name"),-20}" +
                        $"Вага: {row.Field<double>("weight"),-10}"
                    );
            }
            Console.WriteLine();

            var allTeaTypes = teaTable.AsEnumerable()
                .Select(r => r.Field<string>("type_name"))
                .Distinct();

            foreach (var teaType in allTeaTypes)
            {
                var top3TeaByWeight = teaTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == teaType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nТоп-3 види чаю \"{teaType}\" за кількістю грамів:");
                foreach (var row in top3TeaByWeight)
                    Console.WriteLine(
                        $"Назва: {row.Field<string>("name"),-20}" +
                        $"Вага: {row.Field<double>("weight"),-10}"
                    );
            }
        }
    }
}
