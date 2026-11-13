using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace SmartEMR.ApiPractice.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public int retCount { get; set; }
    public string? retMessage { get; set; }
    public bool retIsSuccess { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public async Task<T?> GetItemAsync<T>(eSP sp, T item) where T : class, new()
    {
        var result = await ExecuteQueryAsync<T>(sp, item);
        if (result is null || result.Count == 0)
        {
            return default!;
        }

        return result.First();
    }

    public async Task<List<T>> GetItemsAsync<T>(eSP sp, T item) where T : class, new()
    {
        var result = await ExecuteQueryAsync<T>(sp, item);
        if (result is null)
        {
            return default!;
        }

        return result;
    }

    private async Task<List<T>?> ExecuteQueryAsync<T>(eSP sp, T item) where T : class, new()
    {
        ClearData();

        var result = new List<T>();
        var connection = Database.GetDbConnection();
        var shouldClose = false;

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();

                shouldClose = true;
            }

            var procName = sp.ToString();
            if (string.IsNullOrWhiteSpace(procName))
            {
                return null;
            }

            using var command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procName;

            var props = item?.GetType().GetProperties();

            if (props is null)
                return null;

            foreach (var prop in props)
            {
                var propName = prop.Name;
                var value = prop.GetValue(item);

                if (value is null)
                    continue;

                var paramItem = command.CreateParameter();
                paramItem.ParameterName = $"@{propName}";
                paramItem.Value = value;
                paramItem.DbType = GetDbType(prop.PropertyType);

                command.Parameters.Add(paramItem);
            }

            // OUTPUT 변수
            var totalQueryParam = command.CreateParameter();
            totalQueryParam.ParameterName = "@TotalQuery";
            totalQueryParam.DbType = DbType.Int32;
            totalQueryParam.Direction = ParameterDirection.Output;

            command.Parameters.Add(totalQueryParam);

            var sValParam = command.CreateParameter();
            sValParam.ParameterName = "@sVal";
            sValParam.DbType = DbType.String;
            sValParam.Size = 250;
            sValParam.Direction = ParameterDirection.Output;

            command.Parameters.Add(sValParam);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var ret = new T();
                    var columns = Enumerable.Range(0, reader.FieldCount)
                                            .ToDictionary(
                                                reader.GetName,
                                                i => i,
                                                StringComparer.OrdinalIgnoreCase
                                            );

                    foreach (var prop in props)
                    {
                        if (!columns.TryGetValue(prop.Name, out var ordinal))
                            continue;
                        
                        if (reader.IsDBNull(ordinal))
                        {
                            if (!prop.PropertyType.IsValueType || Nullable.GetUnderlyingType(prop.PropertyType) != null)
                            {
                                prop.SetValue(ret, null);
                            }

                            continue;
                        }
                        
                        object? retValue = prop.PropertyType switch
                        {
                            Type t when t == typeof(int) => reader.GetInt32(ordinal),
                            Type t when t == typeof(double) => reader.GetDouble(ordinal),
                            Type t when t == typeof(float) => reader.GetFloat(ordinal),
                            Type t when t == typeof(decimal) => reader.GetDecimal(ordinal),
                            Type t when t == typeof(string) => reader.GetString(ordinal),
                            Type t when t == typeof(bool) => reader.GetBoolean(ordinal),
                            _ => reader.GetValue(ordinal)                            
                        };

                        prop.SetValue(ret, retValue);
                    }

                    result.Add(ret);
                }
            }

            var totalCount = totalQueryParam.Value != DBNull.Value ? Convert.ToInt32(totalQueryParam.Value) : 0;
            var sVal = sValParam.Value != DBNull.Value ? sValParam.Value?.ToString() : string.Empty;

            retCount = totalCount;
            retMessage = sVal;
            retIsSuccess = true;

            return result;
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.WriteLine(e.StackTrace);
        }
        finally
        {
            if (shouldClose && connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }

        return result;
    }

    private static DbType GetDbType(Type propType)
    {
        return propType switch
        {
            Type t when t == typeof(int) => DbType.Int32,
            Type t when t == typeof(double) => DbType.Double,
            Type t when t == typeof(float) => DbType.Single,
            Type t when t == typeof(decimal) => DbType.Decimal,
            Type t when t == typeof(string) => DbType.String,
            Type t when t == typeof(bool) => DbType.Boolean,
            Type t when t == typeof(DateTime) => DbType.DateTime,
            _ => DbType.Object
        };
    }

    private void ClearData()
    {
        retCount = 0;
        retMessage = "";
        retIsSuccess = false;
    }
}
