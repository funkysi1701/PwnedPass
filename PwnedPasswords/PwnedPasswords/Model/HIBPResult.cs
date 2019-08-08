using System.Collections.Generic;

namespace PwnedPasswords.Model
{
    public class HIBPResult
    {
        public IList<HIBPModel> HIBP { get; set; }
        public string Exception { get; set; }
    }
}