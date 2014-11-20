using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Shaders
{
    /// <summary>
    /// Flag type enum to indicate which type of include we want to accept
    /// </summary>
    public enum ShaderIncludeFlags
    {
        /// <summary>
        /// Will not accept include
        /// </summary>
        None = 0,
        /// <summary>
        /// Accepts local includes
        /// </summary>
        Local = 1,
        /// <summary>
        /// Accepts system includes
        /// </summary>
        System = 2
    }
}
