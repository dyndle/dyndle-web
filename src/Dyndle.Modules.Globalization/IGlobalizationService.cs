﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.Globalization
{
    public interface IGlobalizationService
    {
        Dictionary<string,string> GetCustomPublicationMetadata(int pubId);
    }
}
