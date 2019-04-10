using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    static class Logic
    {
        private static string connString = @"Server = TRANSFORMER1\SQLEXPRESS2016; Database = AdventureWorks; Trusted_Connection=True";


        public static void CheckStuff(string nameStyle, string title, string firstName, string middleName, string lastName, string sufix, string emailProm)
        {
            int NameStyle = (nameStyle.Equals(0)) ? 0 : 1;
            int EmailPromotion = Convert.ToInt32(emailProm);
            DoNothing(connString, NameStyle, title, firstName, middleName, lastName, sufix, EmailPromotion);
        }

        static void DoNothing(string connString, int nameStyle, string title, string firstName, string middleName, string lastName, string sufix, int emailProm)
        {
            string sqlQuery = "insert into [Person].[BusinessEntity] (rowguid,ModifiedDate) " +
                              " values(@rowguid1,GETDATE()) " +
                              " INSERT INTO Person.Person(BusinessEntityID, PersonType," +
                              " NameStyle, Title, FirstName, MiddleName, LastName, Suffix, EmailPromotion, AdditionalContactInfo," +
                              " Demographics,rowguid, ModifiedDate)" +
                              " VALUES((select MAX(BusinessEntityID) from[Person].[BusinessEntity]),@personType," +
                              " @nameStyle,@title,@firstName, @middleName, @lastName, @suffix, @emailPromotion, @additionalInfo," +
                              " @demographics,@rowguid,GETDATE())";
            try
            {

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.Add("@rowguid1", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                        command.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                        command.Parameters.Add("@personType", SqlDbType.NVarChar).Value = "EM";
                        command.Parameters.Add("@nameStyle", SqlDbType.Bit).Value = nameStyle;
                        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = title;
                        command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = firstName;
                        command.Parameters.Add("@middleName", SqlDbType.NVarChar).Value = middleName;
                        command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = lastName;
                        command.Parameters.Add("@suffix", SqlDbType.NVarChar).Value = sufix;
                        command.Parameters.Add("@emailPromotion", SqlDbType.Int).Value = emailProm;
                        command.Parameters.Add("@additionalInfo", SqlDbType.Xml).Value = "";
                        command.Parameters.Add("@demographics", SqlDbType.Xml).Value = "";


                        bool success = (command.ExecuteNonQuery() > 0);


                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error ocured while quering " + ex.Message);

                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error ocured while connecting " + e.Message);

            }
        }

        public static List<int> giveID()
        {

            List<int> IDList = new List<int>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("select BusinessEntityID from [Person].[BusinessEntity]", connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IDList.Add(reader.GetInt32(0));
                    }
                }
            }
            return IDList;

        }

        public static bool DeletePerson(int id)
        {
            string query = @"delete  from Person.Person where BusinessEntityID = @ID";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Error ocured at Quering {e.Message }");
                    return false;
                }
            }
        }

        public static Dictionary<int,string> SelectPerson(int id)
        {
            Dictionary<int,string> valueList = new Dictionary<int, string>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(@"select * from Person.Person where BusinessEntityID = @ID", connection);
                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        for (int i = 2; i <= 8; i++)
                        {
                            
                            valueList[i]=(reader.GetValue(i).ToString());
                        }
                    }
                    return valueList;
                }
            }
        }

        public static bool Update(int id, string nameStyle, string title, string firstName, string middleName, string lastName, string suffix, string emailPrio)
        {
            if (nameStyle.Equals("true") || nameStyle.Equals("1"))
            {
                nameStyle = "1";
            }
            else
            {
                nameStyle = "0";
            }
                string query = @"update Person.Person
                                set NameStyle=@nameStyle, Title=@title,FirstName=@firstName,MiddleName=@middleName,LastName=@lastName,
                                Suffix=@suffix,EmailPromotion=@email
                                where BusinessEntityID=@ID ";

            try
            {

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.Add("@nameStyle", SqlDbType.Bit).Value = Convert.ToInt32(nameStyle);
                        command.Parameters.Add("@title", SqlDbType.NVarChar).Value = title;
                        command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = firstName;
                        command.Parameters.Add("@middleName", SqlDbType.NVarChar).Value = middleName;
                        command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = lastName;
                        command.Parameters.Add("@suffix", SqlDbType.NVarChar).Value = suffix;
                        command.Parameters.Add("@email", SqlDbType.Int).Value = Convert.ToInt32(emailPrio);
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = id;


                        if (command.ExecuteNonQuery() > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error ocured while quering " + ex.Message);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error ocured while connecting " + e.Message);
                return false;
            }


        }
    }
}
