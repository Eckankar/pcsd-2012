using System;
using System.Collections.Generic;

namespace KeyValueBase.Interfaces
{
  public class Configuration
  {
    private readonly List<Uri> slaveWsdlUrls;

    public Configuration()
    {
      this.slaveWsdlUrls = new List<Uri>();
      slaveWsdlUrls.Add(new Uri("http://localhost:33376/KeyValueBaseSlaveService.svc"));
    }

    public Uri MasterWsdlUrl { get; set; }

    public List<Uri> SlaveWsdlUrls
    {
      get { return this.slaveWsdlUrls; }
    }
  }
}
