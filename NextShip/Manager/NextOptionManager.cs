using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

public class NextOptionManager : INextOptionManager
{
    public List<OptionBase> AllOptionBases = [];

    public void RegisterOptions(IEnumerable<OptionBase> options)
    {
        var optionBases = options.ToList();
        foreach (var option in optionBases)
        {
            Register(option);
        }
        AllOptionBases.AddRange(optionBases);
    }
    
    public void RegisterOption<T>(T option) where T : OptionBase
    {
        
    }

    private void Register(OptionBase option)
    {
    }
    
    public void GetOption<T>(int id) where T : OptionBase
    {
        
    }
    
    public void GetOption<T>(string Title) where T : OptionBase
    {
        
    }
}