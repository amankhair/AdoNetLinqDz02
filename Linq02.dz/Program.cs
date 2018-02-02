using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Linq02.dz
{
    class Program
    {
        static string connectionString = "Data Source=AMANKELDI-PC;initial catalog=CRCMS_new;Integrated Security=True";
        static string sql = "SELECT * FROM Area";

        static List<Area> areas = new List<Area>();


        static void Main(string[] args)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
                    DataTable areaTable = new DataTable("Area");
                    adapter.Fill(areaTable);

                    foreach (DataRow row in areaTable.Rows)
                    {
                        Area ar = new Area();

                        ar.AreaId = (int)row["AreaId"];
                        ar.TypeArea = row["TypeArea"] as int?;
                        ar.Name = row["Name"].ToString();
                        ar.ParentId = row["ParentId"] as int?;
                        ar.NoSplit = row["NoSplit"] as bool?;
                        ar.AssemblyArea = row["AssemblyArea"] as bool?;
                        ar.FullName = row["FullName"].ToString();
                        ar.MultipleOrders = row["MultipleOrders"] as bool?;
                        ar.HiddenArea = row["HiddenArea"] as bool?;
                        ar.IP = row["IP"].ToString();
                        ar.PavilionId = (int)row["PavilionId"];
                        ar.TypeId = (int)row["TypeId"];
                        ar.OrderExecution = row["OrderExecution"] as int?;
                        ar.Dependence = row["Dependence"] as int?;
                        ar.WorkingPeople = row["WorkingPeople"] as int?;
                        ar.ComponentTypeId = row["ComponentTypeId"] as int?;
                        ar.GroupId = row["GroupId"] as int?;
                        ar.Segment = row["Segment"] as int?;

                        areas.Add(ar);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Task1();
            Task4();
            Console.WriteLine("=======================================");
            Task5();
            Console.WriteLine("=======================================");
            Task9();
            Console.WriteLine("=======================================");
            Task10();

        }

        static void Task1()
        {



        }

        static void Task2()
        {
            //Создать метод, который возвращает данные в виде Array.

            var data = areas.ToArray();
        }

        //static Array AreaClass()
        //{
        //    //Создать метод, который возвращает данные в виде Array.
        //    return areas.ToArray();
        //}

        static void Task3()
        {
            //Создать метод, который возвращает данные в виде List<Area> 
            var data = areas.ToList();
        }

        //static List<Area> Areas()
        //{
        //    //Создать метод, который возвращает данные в виде List<Area> 
        //    return areas.ToList();
        //}

        static void Task4()
        {
            //Реализовать справочник, который возвращает ID зоны/участка, и IP адрес данной зоны/участка. 
            //Так же необходимо исключить зоны/участки у которых не заполнено поле IP
            var data = areas.Where(w => (!string.IsNullOrEmpty(w.IP)))
                .Select(s => new
                {
                    s.AreaId,
                    s.IP
                });

            var data1 = from a in areas
                        where (string.IsNullOrEmpty(a.IP))
                        select new
                        {
                            a.AreaId,
                            a.IP
                        };

            Console.WriteLine("Task 4:");
            foreach (var v in data)
            {
                Console.WriteLine("\tAreaId: {0}\tIP: {1}", v.AreaId, v.IP);
            }
        }

        static void Task5()
        {
            //Реализовать справочник, который возвращает IP адрес и касс Area, исключить все зоны / участки, 
            //у которых отсутствует IPадрес, а так же исключить все дочерние зоны/ участки(ParentId != 0)
            var data = areas.Where(w => (!string.IsNullOrEmpty(w.IP)) && w.ParentId != 0)
                .Select(s => new
                {
                    s.IP,
                    //класс Area?
                });

            var data1 = from a in areas
                        where ((!string.IsNullOrEmpty(a.IP)) && a.ParentId != 0)
                        select new
                        {
                            a.IP
                        };

            Console.WriteLine("Task 5:");
            foreach (var v in data)
            {
                Console.WriteLine("\tIP: {0}", v.IP);
            }
        }

        //static void Task6()
        //{
        //    //Используя коллекцию Lookup, вернуть следующие данные. 
        //    //В качестве ключа использовать IP адрес, в качестве значения использовать класс Area
        //    var data = areas.ToLookup(k => k.IP,);


        //}

        static void Task7()
        {
            //Вернуть первую запись из последовательности, где HiddenArea=1

            var data = areas.FirstOrDefault(f => f.HiddenArea == true);

        }

        static void Task8()
        {
            //Вернуть последнюю запись из таблицы Area, указав следующий фильтр – PavilionId = 1

            var data = areas.LastOrDefault(l => l.PavilionId == 1);
        }

        static void Task9()
        {
            //Используя квантификаторы, вывесит на экран значения следующих фильтров:
            //a.Есть ли в таблице зоны / участки для PavilionId = 1 и IP = 10.53.34.85, 10.53.34.77, 10.53.34.53
            //b.Содержатся ли данные в таблице Area с наименованием зон / участков - PT disassembly, Engine testing
            string[] IP = new string[] { "10.53.34.85", "10.53.34.77", "10.53.34.53" };

            bool result1 = areas.Any(a => (a.PavilionId == 1) && (IP.Contains(a.IP)));

            string[] Names = new string[] { "PT disassembly", "Engine testing" };

            bool result2 = areas.Any(a => (Names.Contains(a.Name)));


            Console.WriteLine("Task 9");
            Console.WriteLine("\tЕсть ли в таблице зоны / участки для PavilionId = 1 и IP = 10.53.34.85, 10.53.34.77, 10.53.34.53");
            Console.WriteLine("\t" + result1);

            Console.WriteLine("\n\tСодержатся ли данные в таблице Area с наименованием зон / участков - PT disassembly, Engine testing");
            Console.WriteLine("\t" + result2);
        }

        static void Task10()
        {
            //Вывести сумму всех работающих работников (WorkingPeople) на зонах

            var data = areas.Sum(s => s.WorkingPeople);

            Console.WriteLine("Task 10:");

            Console.WriteLine("\t" + data);

        }
    }
}
