using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Polyfill
{
    interface ICloneable
    {
        object Clone();
    }
}

namespace System
{
}