namespace PHPMySQLWebSiteCreator
{
  using System;

  internal class Field
  {
    public Field()
    {
      // constructor
    }

    public Field(string name = "UnNamed Field")
    {
      this.Name = name;
    }

    public string Name { get; set; }
    
    public DataType DataType { get; set; }
    
    public int Length { get; set; }
    
    public bool IsPrimaryKey { get; set; }
    
    public bool IsAutoIncrement { get; set; }
    
    public bool IsUnique { get; set; }
    
    public bool CantBeEmpty { get; set; }
    
    public string Comments { get; set; }
    
    public string Guideline { get; set; }
  }
}