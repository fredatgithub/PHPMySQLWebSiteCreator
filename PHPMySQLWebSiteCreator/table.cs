namespace PHPMySQLWebSiteCreator
{
  using System;
  using System.Collections.Generic;

  internal class Table
  {
    public Table()
    {
      this.Listfields = new List<Field>();
      this.NumberOfField = 0;
    }

    public Table(string tableName = "unNamed Table", string description = "")
    {
      this.Listfields = new List<Field>();
      this.NumberOfField = 0;
      this.Name = tableName;
      this.Description = description;
    }

    public string Name { get; set; }

    public string Description { get; set; }
    
    public short NumberOfRecordsPerPage { get; set; }

    public bool AllowSorting { get; set; }

    public bool AllowFilter { get; set; }

    public bool AllowUsersToSaveFilters { get; set; }
    
    public bool AllowSavingDataToCsvFiles { get; set; }
    
    public int NumberOfField { get; set; }

    public List<Field> Listfields { get; set; }

    public void Add(Field newField)
    {
      this.NumberOfField++;
      this.Listfields.Add(newField);
    }
  }
}