namespace MyApp.Data
{
    using System.Data;

    public interface IDataBind
    {
        void DataBind(DataRow row);
    }
}
