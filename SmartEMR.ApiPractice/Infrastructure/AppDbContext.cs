using System.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace SmartEMR.ApiPractice.Infrastructure;

public class AppDbContext : DbContext, IAppDbContext
{
    public int retCount { get; set; }
    public string? retMessage { get; set; }
    public bool retIsSuccess { get; set; }

    private static readonly Dictionary<string, object?> outputParameterDefaults = new (StringComparer.OrdinalIgnoreCase)
    {
        ["TotalQuery"] = 0,
        ["sVal"] = string.Empty
    };

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public async Task<T?> GetItemAsync<T>(eSP sp, T item, CancellationToken ct) where T : class, new()
    {
        var result = await ExecuteQueryAsync<T>(sp, item, ct);
        if (result is null || result.Count == 0)
        {
            return default!;
        }

        return result.First();
    }

    public async Task<List<T>> GetItemsAsync<T>(eSP sp, T item, CancellationToken ct) where T : class, new()
    {
        var result = await ExecuteQueryAsync<T>(sp, item, ct);
        if (result is null)
        {
            return default!;
        }

        return result;
    }

    private async Task<List<T>?> ExecuteQueryAsync<T>(eSP sp, T item, CancellationToken ct) where T : class, new()
    {
        ClearData();

        var result = new List<T>();
        var connection = Database.GetDbConnection();
        var shouldClose = false;

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(ct);

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

            if (command is not SqlCommand sqlCommand)
            {
                throw new InvalidOperationException("The configured database provider must be SQL Server.");
            }

            SqlCommandBuilder.DeriveParameters(sqlCommand);

            var propertiesByName = props.ToDictionary(prop => prop.Name, StringComparer.OrdinalIgnoreCase);
            for (var index = sqlCommand.Parameters.Count - 1; index >= 0; index--)
            {
                var parameter = (SqlParameter)sqlCommand.Parameters[index];
                var parameterName = parameter.ParameterName.TrimStart('@');

                if (parameter.Direction is ParameterDirection.Output or ParameterDirection.InputOutput && outputParameterDefaults.TryGetValue(parameterName, out var defaultValue))
                {
                    parameter.Value = defaultValue ?? DBNull.Value;
                    continue;
                }

                if (parameter.Direction is not (ParameterDirection.Input or ParameterDirection.InputOutput))
                    continue;

                if (!propertiesByName.TryGetValue(parameterName, out var prop))
                    continue;

                var value = prop.GetValue(item);
                if (value is null)
                {
                    sqlCommand.Parameters.Remove(parameter);
                    continue;
                }

                parameter.Value = value;
            }

            using (var reader = await sqlCommand.ExecuteReaderAsync(ct))
            {
                while (await reader.ReadAsync(ct))
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

            var totalCount = GetOutputParameterValue<int>(sqlCommand, "TotalQuery");
            var sVal = GetOutputParameterValue<string>(sqlCommand, "sVal") ?? string.Empty;

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

    /// <summary>
    /// 지정된 이름의 출력 매개변수 값을 찾아 요청한 형식으로 변환합니다.
    /// </summary>
    private static T? GetOutputParameterValue<T>(SqlCommand command, string parameterName)
    {
        var parameter = command.Parameters
                               .Cast<SqlParameter>()
                               .FirstOrDefault(candidate => candidate.Direction is ParameterDirection.Output or ParameterDirection.InputOutput && string.Equals(candidate.ParameterName.TrimStart('@'), parameterName, StringComparison.OrdinalIgnoreCase));

        if (parameter is null || parameter.Value is DBNull)
        {
            return default;
        }

        return (T)Convert.ChangeType(parameter.Value, typeof(T));
    }

    private void ClearData()
    {
        retCount = 0;
        retMessage = "";
        retIsSuccess = false;
    }
}
