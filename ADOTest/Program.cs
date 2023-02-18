using System.Data;
using System.Data.SQLite;
namespace ADOTest;
class Program
{
    static void Main(string[] args)
    {
        //string conStr = @"Data Source = /Users/kseniabelaevskaa/Projects/Bases/AdoTest.db"; //mac
        string conStr = @"Data Source =/tmp/TestDB.sqlite3"; //linux
        
        
        
        try
        {
            SQLiteConnection con = new SQLiteConnection(conStr);
            con.Open();
            

           
            var da = new SQLiteDataAdapter();
            
            var dt = new DataTable();
            Prepare(da,dt,con);
            da.Fill(dt);
            PrintTable(dt);
        
            
            //Пробуем модифицировать Row
            //InsertInTable(dt,"o1","p","g");
            //DeleteInTable(dt,1);
            UpdateRefill(dt,da);
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
    public static void InsertInTable(DataTable dt,string firstName,string lastName,string phoneNumber)
    {
        DataRow row = dt.NewRow();
        row["firstName"] = firstName;
        row["lastName"] = lastName;
        row["phoneNumber"]= phoneNumber;
        dt.Rows.Add(row);
    }
    public static void DeleteInTable(DataTable dt,int oid)
    {
        foreach(DataRow dr in dt.Rows)
        {
            if (Int32.Parse(dr["oid"].ToString())==oid){
                dr.Delete();
            }
        }
    }
    public static void UpdateRefill(DataTable dt,SQLiteDataAdapter da)
    {
        da.Update(dt);
        dt.Dispose();
        dt = new DataTable();
        da.Fill(dt);

    }
}

