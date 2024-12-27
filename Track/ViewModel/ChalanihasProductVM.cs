using System.Collections.Generic;
using Track.Models;

namespace Track.ViewModel
{
    public class ChalanihasProductVM
    {
        public ChalanihasProductClass? Class { get; set; }
        public List<Text_value>? Serial_no { get; set; }

        public int? chalani_no { get; set; }
    }
}
