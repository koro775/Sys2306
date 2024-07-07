using Microsoft.Data.SqlClient;

namespace Helper03.SqlServerHelpers
{
      public class ReaderHelper
      {
            /// <summary>
            /// Action指定Reader
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="parameters"></param>
            /// <param name="action"></param>
            /// <param name="cn"></param>
            public static void Query(
                                            string sql,
                                            SqlParameter[] parameters,
                                            Action<SqlDataReader> action,
                                            string cn) {
                  using var connection = new SqlConnection(cn);
                  using var command = new SqlCommand(sql, connection);
                  connection.Open();
                  if (parameters != null) {
                        command.Parameters.AddRange(parameters);
                  }
                  using var reader = command.ExecuteReader();
                  while (reader.Read()) {
                        action(reader);
                  }
            }
            /// <summary>
            ///    パラメタ無しAction指定Reader
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="action"></param>
            /// <param name="cn"></param>
            public static void Query(
                                                string sql,
                                                Action<SqlDataReader> action,
                                                string cn) {
                  using var connection = new SqlConnection(cn);
                  using var command = new SqlCommand(sql, connection);
                  connection.Open();
                  using var reader = command.ExecuteReader();
                  while (reader.Read()) {
                        action(reader);
                  }
            }

            /// <summary>
            ///     ストアド版 Action指定Reader
            /// </summary>
            /// <param name="stored"></param>
            /// <param name="parameters"></param>
            /// <param name="action"></param>
            /// <param name="cn"></param>
            public static void QueryByStored(
                                                string stored,
                                                SqlParameter[] parameters,
                                                Action<SqlDataReader> action,
                                                string cn) {
                  using var connection = new SqlConnection(cn);
                  using var command = new SqlCommand(stored, connection);
                  connection.Open();
                  if (parameters != null) {
                        command.Parameters.AddRange(parameters);
                  }
                  command.CommandType = System.Data.CommandType.StoredProcedure;
                  using var reader = command.ExecuteReader();
                  while (reader.Read()) {
                        action(reader);
                  }
            }

            /// <summary>
            ///   パラメタあり   Execute
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="parameters"></param>
            /// <param name="cn"></param>
            public static void ExecuteSql(string sql, SqlParameter[] parameters, string cn) {
                  using var connection =
                  new SqlConnection(cn);
                  using var command = new SqlCommand(sql, connection);
                  connection.Open();
                  if (parameters != null) {
                        command.Parameters.AddRange(parameters);
                  }
                  command.ExecuteNonQuery();
            }

            /// <summary>
            ///     パラメタ無し   Execute
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="cn"></param>
            public static void ExecuteSql(string sql, string cn) {
                  using var connection =
                  new SqlConnection(cn);
                  using var command = new SqlCommand(sql, connection);
                  connection.Open();
                  command.ExecuteNonQuery();
            }

            /// <summary>
            /// SqlOarameterの配列返却
            /// </summary>
            /// <param name="paramName"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public static SqlParameter[] GetParamList(string paramName, object param) {
                  return new List<SqlParameter> { new SqlParameter(paramName, param) }.ToArray();
            }

            /// <summary>
            /// ？？？？？
            /// </summary>
            /// <param name="select"></param>
            /// <param name="where"></param>
            /// <returns></returns>
            public static SqlParameter[] GetParameta2(string select, string where) {
                  var parameters2 = new SqlParameter[2] { new SqlParameter("@Select", select),
                                                                              new SqlParameter("@Where", where)};

                  return parameters2;
            }

            /// <summary>
            /// ？？？？？
            /// </summary>
            /// <param name="select"></param>
            /// <param name="where"></param>
            /// <returns></returns>
            public static SqlParameter[] GetParameta1(string where) {
                  var parameters1 = new SqlParameter[1] { new SqlParameter("@Where", where) };

                  return parameters1;
            }
      }
}