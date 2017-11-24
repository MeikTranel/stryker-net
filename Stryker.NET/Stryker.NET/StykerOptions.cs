namespace Stryker.NET
{
    public class StykerOptions : IStykerOptions
    {
        public string FileFilter { get; set; }

        public StykerOptions(string filter)
        {
            FileFilter = filter;
        }

        public StykerOptions() : this("*.cs")
        {

        }
    }
}