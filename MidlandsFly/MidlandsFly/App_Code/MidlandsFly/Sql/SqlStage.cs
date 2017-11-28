using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Database.Sql;
using Database.Enums;
using System.Data.SqlClient;
using MidlandsFly.Sql;

/// <summary>
/// Provides methods for manipulating aircraft-related tables.
/// </summary>
namespace MidlandsFly
{
    namespace Sql
    {
        public class SqlStage
        {
            private static SqlStage instance;
            private static Random random = new Random();

            public static SqlStage Instance
            {
                get
                {
                    if (instance == null)
                        instance = new SqlStage();
                    return instance;
                }
            }
            public void Insert(string regNumber, byte columnNumber = 0)
            {
                SqlMidlandsFly.Instance.AddCommand(InsertCmd(regNumber));
            }
            public SqlCommand InsertCmd(string regNumber)
            {
                string text;
                text = String.Format("INSERT INTO {0} VALUES('{1}',{2},{3},{4},{5});",
                    SqlMidlandsFly.Instance.Table_Stage.Name,
                    regNumber,
                    0,
                    0,
                    "SYSDATETIME()",
                    random.Next(1,18));
                return new SqlCommand(text);
            }
        }
    }
}