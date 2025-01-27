using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.ViewModels
{
    public class ViewGroupCount
    {

        public ViewGroupCount()
        {
            
        }
        public ViewGroupCount(string label, int value)
        {
            this.label = label;
            this.value = value;
        }

        public string label {get;set;}
        public int value { get;set;}
    }
}
