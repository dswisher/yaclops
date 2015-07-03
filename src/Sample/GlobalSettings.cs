using System.ComponentModel;
using Yaclops.Attributes;


namespace Sample
{
    public class GlobalSettings
    {
        [NamedParameter]
        [Description("Show the sample logo")]
        public bool ShowLogo { get; set; }
    }
}
