using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.TokenRate.Settings
{
    public class SettingsModel
    {
        [YamlProperty("TokenRate.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("TokenRate.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("TokenRate.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("TokenRate.PostgresConnectionString")]
        public string PostgresConnectionString { get; set; }
    }
}
