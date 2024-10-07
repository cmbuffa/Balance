namespace Balance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal balance = 0m;
            foreach (string[] transaction in ReadRecords())
            {
                balance += transaction switch
                {
                [_, "DEPOSIT", _, var amount] => decimal.Parse(amount),
                [_, "WITHDRAWAL", .., var amount] => -decimal.Parse(amount),
                [_, "INTEREST", var amount] => decimal.Parse(amount),
                [_, "FEE", var fee] => -decimal.Parse(fee),
                    _ => throw new InvalidOperationException($"Record {string.Join(", ", transaction)} is not in the expected format!"),
                };
                Console.WriteLine($"Record: {string.Join(", ", transaction)}, New balance: {balance:C}");
            }
        }

        private static List<string[]> ReadRecords()
        {
            var list = new List<string[]>();
            var records = ReadCsv($"{Environment.CurrentDirectory}/Balance.csv");
            foreach (var record in records)
            {
                var csvRecord = record.Split(',').Select(x => x.Trim()).ToArray();
                list.Add(csvRecord);
            }

            return list;
        }

        private static string[] ReadCsv(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
