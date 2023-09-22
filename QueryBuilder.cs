using Microsoft.Data.Sqlite;
using System.Data;
using System.Reflection;
using System.Text;

/**       
 *--------------------------------------------------------------------
 * 	   File name: QueryBuilder
 * 	Project name:CrowdisLab3
 *--------------------------------------------------------------------
 * Author’s name and email:	 kinsley crowdis crowdis@etsu.edu			
 *          Course-Section: CSCI 2150-800
 *           Creation Date:	09/14/2023
 * -------------------------------------------------------------------
 */

namespace CrowdisLab3
{
    public class QueryBuilder : IDisposable
    {
        SqliteConnection? connection;

        //opens everytime this class is used instad of opening and closing for every method
        public QueryBuilder(string dbPath)
        {
            connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();
        }

        /// <summary>
        /// Read command to retrieve a single record given a property called 'Id'
        /// - Data types that are in the DateTime format, SQLite has to store them in YYYY-MM-DD format
        /// - If not, null will be stored
        /// </summary>
        /// <param name="id">The Id value of the object's corresponding DB record</param>
        /// <returns>An object of type T</returns>
        public T Read<T>(int id) where T : new()
        {
            var command = connection!.CreateCommand();
            command.CommandText = $"SELECT * FROM {typeof(T).Name} where Id = {id}";

            var reader = command.ExecuteReader();

            T data = new T();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // convert integer data to int data type from SQLite's int64 default
                    if (typeof(T).GetProperty(reader.GetName(i)).PropertyType == typeof(int))
                    {
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, Convert.ToInt32(reader.GetValue(i)));
                    }
                    // if datetime, format the string correctly
                    else if (typeof(T).GetProperty(reader.GetName(i)).PropertyType == typeof(DateTime) && reader.GetValue(i).ToString().Split('-').Length == 3)
                    {
                        string[] dateString = reader.GetValue(i).ToString().Split('-');
                        int[] dateNum = new int[3];
                        for (int k = 0; k < 3; k++)
                        {
                            dateNum[k] = Convert.ToInt32(dateString[k]);
                        }
                        var dateTime = new DateTime(dateNum[0], dateNum[1], dateNum[2]);
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, dateTime);
                    }
                    // other data types
                    else
                    {
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, reader.GetValue(i));
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// ReadAll operation to retrieve all records from a SQLite table
        /// The same data type concerns from Read apply here, as well
        /// </summary>
        /// <returns>A list of T objects</returns>
        public List<T> ReadAll<T>() where T : new() //has a default constructor
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM {typeof(T).Name}";
            var reader = command.ExecuteReader();

            T data;
            var dataList = new List<T>();

            while (reader.Read())
            {
                data = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    // same code as the Read
                    int id;
                    
                    // convert integer data to int data type from SQLite's int64 default
                    if (typeof(T).GetProperty(reader.GetName(i)).PropertyType == typeof(int))
                    {
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, Convert.ToInt32(reader.GetValue(i)));
                    }
                    // if datetime, format the string correctly
                    else if (typeof(T).GetProperty(reader.GetName(i)).PropertyType == typeof(DateTime) && reader.GetValue(i).ToString().Split('-').Length == 3)
                    {
                        string[] dateString = reader.GetValue(i).ToString().Split('-');
                        int[] dateNum = new int[3];
                        for (int k = 0; k < 3; k++)
                        {
                            dateNum[k] = Convert.ToInt32(dateString[k]);
                        }
                        var dateTime = new DateTime(dateNum[0], dateNum[1], dateNum[2]);
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, dateTime);
                    }
                    // other data types
                    else
                    {
                        typeof(T).GetProperty(reader.GetName(i)).SetValue(data, reader.GetValue(i));
                    }
                }
                dataList.Add(data);
            }
            return dataList;
        }

        ///<summary>
        ///the create method
        ///</summary>
        // TODO: Add the Create method
        public void Create<T>(T obj) where T : IClassModel
        {

            PropertyInfo[] properties = typeof(T).GetProperties();

            // get property values
            var values = new List<string>();
            var names = new List<string>();
            PropertyInfo property;

            for (int i = 1; i < properties.Length; i++)
            {
                property = properties[i];
                // format DateTime for DB (reverse the process in Read)
                // quotation marks are necessary for the date format
                if (property.PropertyType == typeof(DateTime))
                {
                    values.Add($"\"{((DateTime)property.GetValue(obj)).Year}-{((DateTime)property.GetValue(obj)).Month}-{((DateTime)property.GetValue(obj)).Day}\"");
                }
                // format string for DB
                // quotation marks are necessary for the text format
                else if (property.PropertyType == typeof(string))
                {
                    values.Add($"\"{property.GetValue(obj).ToString()}\"");
                }
                // format other data types for DB
                else
                {
                    values.Add(property.GetValue(obj).ToString());
                }
                names.Add(property.Name);
            }


            // build the insert statement
            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO [{typeof(T).Name}] ("); //opens the statement (with help from chatgpt)

            for (int i = 0; i < names.Count; i++)
            {
                if (i == values.Count - 1)
                {
                    sb.Append($"{names[i]}) ");
                }
                else
                {
                    sb.Append($"{names[i]}, ");
                }
            }

            sb.Append("VALUES (");
            for (int i = 0; i < values.Count; i++)  //just like the loop from the update method this makes sure the , goes in after each item and that it doesnt matter how many there are.
            {
                if (i == values.Count - 1)
                {
                    sb.Append($"{values[i]} ");
                }
                else
                {
                    sb.Append($"{values[i]}, ");
                }
            }

            sb.Append(");"); //closes

            // executes the insert statement
            var command = connection.CreateCommand();
            command.CommandText = sb.ToString();
            var reader = command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update operation to update a record in the SQLite database
        /// </summary>
        /// <param name="obj">Object of type T to be updated (its Id param will be used to find it)</param>
        public void Update<T>(T obj) where T : IClassModel
        {
            // this code will be the same as Create, minus the SQL command
            // get the property names of the object 
            PropertyInfo[] properties = typeof(T).GetProperties();

            // get property values
            var values = new List<string>();
            var names = new List<string>();
            PropertyInfo property;

            for (int i = 1; i < properties.Length; i++)
            {
                property = properties[i];
                // format DateTime for DB (reverse the process in Read)
                // quotation marks are necessary for the date format
                if (property.PropertyType == typeof(DateTime))
                {
                    values.Add($"\"{((DateTime)property.GetValue(obj)).Year}-{((DateTime)property.GetValue(obj)).Month}-{((DateTime)property.GetValue(obj)).Day}\"");
                }
                // format string for DB
                // quotation marks are necessary for the text format
                else if (property.PropertyType == typeof(string))
                {
                    values.Add($"\"{property.GetValue(obj).ToString()}\"");
                }
                // format other data types for DB
                else
                {
                    values.Add(property.GetValue(obj).ToString());
                }
                names.Add(property.Name);
            }

            // build the insert statement
            StringBuilder sb = new StringBuilder();

            // include commas in between values / names UNLESS you're on the last item
            for (int i = 0; i < values.Count; i++)
            {
                if (i == values.Count - 1)
                {
                    sb.Append($"{names[i]} = {values[i]}");
                }
                else
                {
                    sb.Append($"{names[i]} = {values[i]}, ");
                }
            }

            var command = connection.CreateCommand();
            command.CommandText = $"UPDATE {typeof(T).Name} SET {sb} WHERE Id = {obj.Id}";
            var reader = command.ExecuteNonQuery();
        }

        /// <summary>
        /// Delete operation to remove the parameter object from the SQLite database
        /// </summary>
        /// <param name="obj">Object of type T to be deleted</param>
        public void Delete<T>(T obj) where T : IClassModel
        {
            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM {typeof(T).Name} WHERE Id = {obj.Id}";
            var reader = command.ExecuteNonQuery();
        }

        /// <summary>
        /// Closes resources committed to reading SQLite file
        /// (required for a 'using' block)
        /// </summary>
        public void Dispose()
        {
            connection!.Dispose();
        }

        //TODO: Create DeleteAll method
        public void DeleteAll()
        {
            // help from bing ai. have to select all tables in order to drop them. (no GetSchema method :( )
            var command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM [sqlite_master] WHERE type='table';"; //call on master table because it holds all info 🧙‍ mwhahaha
            var reader = command.ExecuteReader();

            //adds the table names to a list
            List<string> tableNames = new List<string>();

            while (reader.Read()) //create a loop that adds the names from the command to the list
            {
                tableNames.Add(reader.GetString(0));
            }
            reader.Close();

            for (int i = 1; i < tableNames.Count; i++) //we have to skip the first table so that the master does not get deleted
            {
                command.CommandText = $"DELETE FROM {tableNames[i]};";
                command.ExecuteNonQuery();
            }
        }
    }
}
