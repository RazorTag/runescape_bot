using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunescapeBot.Networking
{
    public interface IUrlEncodable
    {
        /// <summary>
        /// Gets the object as a string, string dictionary for use in URl encoding.
        /// </summary>
        /// <returns>dictionary representation of this object</returns>
        Dictionary<string, string> GetUrlDictionary();
    }
}
