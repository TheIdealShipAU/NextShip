namespace NextShip.Api.Interfaces;

public interface ILang
{
    public SupportedLangs LangId { get; protected set; }

    public string LangName { get; set; }
    
    public string Author { get; set; }

    public string GetString(string Key);

    public string GetStringOfIndex(int Index);

    public void Init();
}