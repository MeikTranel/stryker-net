using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET.Core
{
    public struct Location
    {
        public Location(Position start, Position end)
        {
            Start = start;
            End = end;
        }

        public Position Start { get; set; }
        public Position End { get; set; }
    }
}
