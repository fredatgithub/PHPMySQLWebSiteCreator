namespace PHPMySQLWebSiteCreator
{
  using System.Collections.Generic;

  internal class Database
  {
    public Database()
    {
      this.ListTable = new List<Table>();
      this.NumberOfTable = 0;
    }

    public string Name { get; set; }
    
    public List<Table> ListTable { get; set; }
    
    public int NumberOfTable { get; set; }

    public void Add(Table newTable)
    {
      this.NumberOfTable++;
      this.ListTable.Add(newTable);
    }
  }
}