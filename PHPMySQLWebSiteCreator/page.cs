namespace PHPMySQLWebSiteCreator
{
  internal class Page
  {
    public Page(FileExtension fileextension = FileExtension.HTML, string name = "untitledPage")
    {
      this.Name = name;
      this.Code = string.Empty;
      this.FileExtension = fileextension;
    }

    public string Name { get; set; }

    public FileExtension FileExtension { get; set; }
    
    public string Code { get; set; }

    public void AddCode(string codeText)
    {
      this.Code += codeText;
    }
  }
}