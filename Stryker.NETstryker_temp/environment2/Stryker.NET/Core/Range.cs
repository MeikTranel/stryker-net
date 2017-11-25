using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stryker.NET.Core
{
    /// <summary>The Range class.</summary>
    /// <typeparam name="T">Generic parameter.</typeparam>
    public class Range<T> where T : IComparable<T>
    {
        public Range(T fromInclusive, T toExclusive)
        {
            FromInclusive = fromInclusive;
            ToExclusive = toExclusive;
        }

        /// <summary>Minimum value of the range.</summary>
        public T FromInclusive { get; set; }

        /// <summary>Maximum value of the range.</summary>
        public T ToExclusive { get; set; }

        /// <summary>Presents the Range in readable format.</summary>
        /// <returns>String representation of the Range</returns>
        public override string ToString()
        {
            return string.Format("[{0} - {1}]", this.FromInclusive, this.ToExclusive);
        }

        /// <summary>Determines if the range is valid.</summary>
        /// <returns>True if range is valid, else false</returns>
        public bool IsValid()
        {
            return this.FromInclusive.CompareTo(this.ToExclusive) <= 0;
        }

        /// <summary>Determines if the provided value is inside the range.</summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public bool ContainsValue(T value)
        {
            return (this.FromInclusive.CompareTo(value) <= 0) && (value.CompareTo(this.ToExclusive) <= 0);
        }

        /// <summary>Determines if this Range is inside the bounds of another range.</summary>
        /// <param name="Range">The parent range to test on</param>
        /// <returns>True if range is inclusive, else false</returns>
        public bool IsInsideRange(Range<T> range)
        {
            return this.IsValid() && range.IsValid() && range.ContainsValue(this.FromInclusive) && range.ContainsValue(this.ToExclusive);
        }

        /// <summary>Determines if another range is inside the bounds of this range.</summary>
        /// <param name="Range">The child range to test</param>
        /// <returns>True if range is inside, else false</returns>
        public bool ContainsRange(Range<T> range)
        {
            return this.IsValid() && range.IsValid() && this.ContainsValue(range.FromInclusive) && this.ContainsValue(range.ToExclusive);
        }
    }
}
