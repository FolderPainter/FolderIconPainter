using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FIP.App.Helpers.Settings;

[JsonSourceGenerationOptions(WriteIndented = true, GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(Dictionary<string, System.Text.Json.JsonElement>))]
[JsonSerializable(typeof(SettingsHelper))]
internal partial class SettingsJsonContext : JsonSerializerContext
{
}