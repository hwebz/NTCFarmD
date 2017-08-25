using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;

namespace Gro.ViewModels.Navigation
{
    public class InternalNavigationItemModel
    {
        public InternalNavigationItemModel()
        {
            SubItems = new List<InternalNavigationItemModel>();
        }
        public PageData Page { get; set; }
        public string CssClass { get; set; }
        public List<InternalNavigationItemModel> SubItems { get; set; }        
        public bool IsActive { get; set; } 
    }
}