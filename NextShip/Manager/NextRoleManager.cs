using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Interfaces;
using NextShip.Roles;

namespace NextShip.Manager;

#nullable enable
public sealed class NextRoleManager : IRoleManager
{
    public readonly List<IRole> Roles = [];
    public IRoleCreator? CurrentCreator { get; private set; }

    public void Register(IRole role)
    {
        Roles.Add(role);
    }

    public void UnRegister(IRole role)
    {
        Roles.Remove(role);
    }


    public void AssignRole(PlayerControl player, IRole role)
    {
    }

    public void Clear()
    {
    }

    public void SetCreator(IRoleCreator creator)
    {
        CurrentCreator?.Dispose();
        CurrentCreator = null;
        CurrentCreator = creator;
    }

    public void CheckRoles()
    {
    }

    public IRoleCreator GetCreator() => CurrentCreator ?? FastGetCreator();
    
    public FastCreator FastGetCreator()
    {
        if (CurrentCreator is FastCreator creator)
            return creator;

        var newCreator = new FastCreator();
        SetCreator(newCreator);

        return newCreator;
    }

    public T? GetRole<T>() where T : class, IRole
    {
        return Roles.FirstOrDefault(n => n is T) as T;
    }

    public IEnumerable<T> GetRoles<T>() where T : IRole
    {
        return Roles.FindAll(n => n is T).Select(n =>　(T)n);
    }
}