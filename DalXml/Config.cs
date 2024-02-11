namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextTaskId", value);
    }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextDependencyId", value);
    }
}
