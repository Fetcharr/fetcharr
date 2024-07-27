using System.ComponentModel;

using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Configuration>))]
public class Configuration : Enumeration
{
    public static Configuration Debug = new Configuration { Value = nameof(Debug) };
    public static Configuration Release = new Configuration { Value = nameof(Release) };

    /// <summary>
    ///   Gets whether the current configuration is set to Debug.
    /// </summary>
    public bool IsDebug => this.Equals(Debug);

    /// <summary>
    ///   Gets whether the current configuration is set to Release.
    /// </summary>
    public bool IsRelease => this.Equals(Release);

    public static implicit operator string(Configuration configuration)
        => configuration.Value;
}