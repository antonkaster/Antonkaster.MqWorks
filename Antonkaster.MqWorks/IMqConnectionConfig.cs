namespace Antonkaster.MqWorks
{
    public interface IMqConnectionConfig
    {
        string Host { get; set; }
        string Password { get; set; }
        string User { get; set; }
        string VHost { get; set; }
    }
}