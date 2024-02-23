using NextShip.Api.Interfaces;

namespace NextShip.Options.Options;

public class RoleOptionBase(
    bool enableTranslation,
    OptionBehaviour optionBehaviour,
    IOptionInfo optionInfo,
    string title) : StringOptionBase(enableTranslation, optionBehaviour, optionInfo, title)
{
    public static string[] defaultChances =
        ["0% ", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"];


    public override RoleOptionBase GetBase()
    {
        return this;
    }
}