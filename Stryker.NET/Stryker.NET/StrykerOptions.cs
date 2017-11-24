namespace Stryker.NET
{
    public class StrykerOptions : IStykerOptions
    {
        public string FileFilter { get; set; }

        public StrykerOptions(string filter)
        {
            FileFilter = filter;
        }

        public StrykerOptions() : this("*.cs")
        {

        }
    }
}