using DGT.Model.AttributeClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGT.DBOperation
{
    public static class DataTranslateHelper
    {
        public static DataTable GennerateTable<T>(List<T> lists)
        {
            DataTable table = new DataTable(typeof(T).Name);

            int columsCount;
            columsCount = GennerateObjects<T>(lists[0]).Count();

            for (int i = 1; i <= columsCount; i++)
            {
                table.Columns.Add(new DataColumn());
            }


            foreach (var item in lists)
            {
                T t = item;

                DataRow row = table.NewRow();
                row.ItemArray = GennerateObjects<T>(t);
                table.Rows.Add(row);

            }

            return table;

        }

        public static object[] GennerateObjects<T>(T t)
        {
            var pro = typeof(T).GetProperties();
            object[] returnobjects = new object[] { };
            // List<Object> list = new List<Object>();
            Dictionary<int, object> dicts = new Dictionary<int, object>();
            foreach (var p in pro)
            {
                var a = p.GetCustomAttributes(typeof(ColumOderAttribute), false);
                var len = p.GetCustomAttributes(typeof(ColumLengthAttribute), false);
                if (a.Count() != 0)
                {

                    int order = (a.GetValue(0) as ColumOderAttribute).Order;
                    // string protype = p.PropertyType.FullName;
                    object c = p.GetValue(t);
                    if (len.Count() != 0 && c != null)
                    {
                        if (c.ToString().Length > ((ColumLengthAttribute)len[0]).Maxlength)
                        {
                            c = c.ToString().Substring(0, ((ColumLengthAttribute)len[0]).Maxlength);
                        }
                    }

                    dicts.Add(order, c);
                    //  list.Add(c);
                }

            }
            returnobjects = dicts.OrderBy(p => p.Key).Select(p => p.Value).ToArray(); ;
            //  returnobjects = list.ToArray();

            return returnobjects;
        }

    }
}
