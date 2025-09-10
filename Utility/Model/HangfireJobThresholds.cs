using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Model
{
    public class HangfireJobThresholds
    {
        public int Default { get; set; } = 5000;
        public Dictionary<string, int> Jobs { get; set; } = new();
    }

}
