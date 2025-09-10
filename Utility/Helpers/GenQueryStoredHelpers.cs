using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Helpers
{
    public static class GenQueryStoredHelpers
    {
        public static string GenQuerySearch(List<object> lstParam, string storedName)
        {
            if (lstParam is null || lstParam.Count == 0) return string.Empty;

            string sql = $@"rollback;begin;SELECT {storedName}('v_out', {GenParam(lstParam)});fetch all from v_out;end;";

            return sql;
        }

        #region Utility
        private static string GenParam(List<object> lstParam)
        {
            string query = string.Join(", ", lstParam.Select(item => $"'{item ?? ""}'::{ToPgType(item?.GetType())}"));
            return query;
        }

        public static string ToPgType(Type? type)
        {
            if (type == typeof(short) || type == typeof(Int16))
                return "int2";
            if (type == typeof(int) || type == typeof(Int32))
                return "int4";
            if (type == typeof(long) || type == typeof(Int64))
                return "int8";
            if (type == typeof(decimal))
                return "numeric";
            if (type == typeof(float) || type == typeof(Single))
                return "float4";
            if (type == typeof(double))
                return "float8";
            if (type == typeof(bool) || type == typeof(Boolean))
                return "bool";
            if (type == typeof(string))
                return "text";
            if (type == typeof(DateTime))
                return "timestamp";
            if (type == typeof(Guid))
                return "uuid";
            if (type == typeof(byte[]))
                return "bytea";

            return "text";
        }
        #endregion
    }
}
