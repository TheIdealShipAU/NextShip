namespace NextShip.Api.Interfaces;

public interface ILangManager
{
    public void RegisterLang(ILang lang);

    public void UnRegisterLang(ILang lang);

    public void Set(SupportedLangs langId, ILang lang);

    public void SetCurrentLang(SupportedLangs langId);
}