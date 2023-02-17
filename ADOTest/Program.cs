using System.Data;
using System.Data.SQLite;
namespace ADOTest;
class Program
{
    static void Main(string[] args)
    {
        string conStr = @"Data Source = /Users/kseniabelaevskaa/Projects/Bases/AdoTest.db"; //mac
        //string conStr = @"Data Source =/tmp/TestDB.sqlite3"; //linux
        
        
        
        try
        {
            SQLiteConnection con = new SQLiteConnection(conStr);
            con.Open();
            

           using (SQLiteCommand sqlcom = new SQLiteCommand(con))
           {
               sqlcom.CommandText = @"INSERT INTO Workers (firstName,lastName,phoneNumber) VALUES ('раз','dva','8800553535')";
               //sqlcom.ExecuteNonQuery();
           }
            var da = new SQLiteDataAdapter(@"SELECT * FROM Workers", con);
            var dt = new DataTable();
            Prepare(da, dt, con);
            da.Fill(dt);
            
            System.Console.WriteLine("/n");
            
            //Пробуем модифицировать Row
            InsertInTable(dt,"o1","p","g",da);
            PrintTable(dt);
            DeleteInTable(dt,da,16);
            
            PrintTable(dt);

            UpdateInTable(dt, da, 10, "kek", "lol", "666666");
            PrintTable(dt);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.WriteLine("DOne");
        }
    }


    #region PrintTable
    public static void PrintTable(DataTable dt)
    {
        foreach (DataColumn column in dt.Columns)
            Console.Write($"{column.ColumnName}\t     ");
        Console.WriteLine();

        foreach (DataRow row in dt.Rows)
        {

            var cells = row.ItemArray;
            foreach (object cell in cells)
                Console.Write($"{cell}\t\t");
            Console.WriteLine();
        }
    }
    #endregion
    #region Prepare
    public static void Prepare (SQLiteDataAdapter da,DataTable dt,SQLiteConnection con)
    {
        //SELECT
        da.SelectCommand = new SQLiteCommand("SELECT * FROM Workers",con);
        //INSERT
        string sql = @"INSERT INTO Workers (firstName,lastName,phoneNumber)
        VALUES (@firstName,@lastName,@phoneNumber);";
        da.InsertCommand = new SQLiteCommand(sql,con);
        da.InsertCommand.Parameters.Add("@oid",DbType.Int32,4,"oid");
        da.InsertCommand.Parameters.Add("@firstName",DbType.String,10,"firstName");
        da.InsertCommand.Parameters.Add("@lastName",DbType.String,10,"lastName");
        da.InsertCommand.Parameters.Add("@phoneNumber",DbType.String,4,"phoneNumber");
        //UPDATE
        sql = @"UPDATE Workers SET
        firstName = @firstName,
        lastName = @lastName,
        phoneNumber = @phoneNumber
        WHERE oid = @oid;";
        da.UpdateCommand = new SQLiteCommand(sql,con);
        da.UpdateCommand.Parameters.Add("@oid",DbType.Int32,4,"oid");
        da.UpdateCommand.Parameters.Add("@firstName",DbType.String,10,"firstName");
        da.UpdateCommand.Parameters.Add("@lastName",DbType.String,10,"lastName");
        da.UpdateCommand.Parameters.Add("@phoneNumber",DbType.String,4,"phoneNumber");
        //DELETE
        sql = @"DELETE FROM WORKERS WHERE oid = @oid";
        da.DeleteCommand = new SQLiteCommand(sql,con);
        da.DeleteCommand.Parameters.Add("@oid",DbType.Int32,4,"oid");
    }
    #endregion
    #region InsertInTable
    public static void InsertInTable(DataTable dt,string firstName,string lastName,string phoneNumber,SQLiteDataAdapter da)
    {
        DataRow row = dt.NewRow();
        
        row["oid"] = LastOidInTable(dt)+1;
        row["firstName"] = firstName;
        row["lastName"] = lastName;
        row["phoneNumber"]= phoneNumber;
        dt.Rows.Add(row);
        UpdateRefill(dt, da);
    }
    #endregion
    #region DeleteInTable 
    public static void DeleteInTable(DataTable dt,SQLiteDataAdapter da,int oid)
    {
        foreach(DataRow dr in dt.Rows)
        {
            if (Int32.Parse(dr["oid"].ToString())==oid){
                dr.Delete();
                UpdateRefill(dt, da);
                
            }
        }
        
    }
    #endregion
    #region UpdateInTable
    public static void UpdateInTable(DataTable dt,SQLiteDataAdapter da,int oid,string firstName,string lastName,string phoneNumber)
    {
        foreach (DataRow dr in dt.Rows)
        {
            if (Int32.Parse(dr["oid"].ToString()) == oid)
            {
                dr["firstName"] = firstName;
                dr["lastname"] = lastName;
                dr["phoneNumber"] = phoneNumber;
                UpdateRefill(dt, da);

            }
        }
    }
    #endregion
    #region LastOidInTable
    public static int LastOidInTable(DataTable dt)
    {
        int res = 0;
        foreach (DataRow dr in dt.Rows)
        {
            res = Int32.Parse(dr["oid"].ToString());
        }
        return res;

    }
    #endregion
    #region UpdateRefill
    public static void UpdateRefill(DataTable dt,SQLiteDataAdapter da)
    {
        da.Update(dt);
        

    }
    #endregion
}

