using GemBox.Spreadsheet;
using StackExchange.Redis;

namespace DataTransit
{
    public static class DataTransit
    {
        public static async Task AddExcelDataToRedis(FileStream file, string redisConnectionString)
        {
            var _redisDb = ConnectionMultiplexer.Connect(redisConnectionString).GetDatabase();
            ExcelFile workbook = ExcelFile.Load(file);
            ExcelWorksheet worksheet = workbook.Worksheets[0];
            // Iterate through all rows in a worksheet.
            foreach (ExcelRow row in worksheet.Rows)
            {
                string value = row.Cells[0].Value?.ToString() ?? "EMPTY";
                await _redisDb.SetAddAsync($"ExcelData:row:{row.Index}:col:A", value, CommandFlags.FireAndForget);
            }
        }
        public static async Task AddRedisDataToSql(string redisConnectionString,int dbNumber, string pattern)
        {
            var db = ConnectionMultiplexer.Connect(redisConnectionString).GetDatabase();
            var server = ConnectionMultiplexer.Connect(redisConnectionString).GetServer(redisConnectionString);

            var keys = server.Keys(dbNumber, pattern);
            using (var _dbContext = new AppDbContext())
            {
                foreach (var key in keys)
                {
                    var value = await db.SetPopAsync(key);

                    _dbContext.ExcelDatas.Add(new ExcelData
                    {
                        Id = key,
                        data = value
                    });
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}