namespace PHPMySQLWebSiteCreator
{
  using System.Collections.Generic;

  internal class WebSite
  {
    public WebSite()
    {
      this.ListDb = new List<Database>();
    }

    public string Name { get; set; }
    
    public List<Database> ListDb { get; set; }
  }
}