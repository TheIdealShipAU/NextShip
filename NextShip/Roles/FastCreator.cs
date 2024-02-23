using System;
using System.Collections.Generic;
using System.Linq;
using NextShip.Api.Interfaces;
using NextShip.Manager;

namespace NextShip.Roles;

public class FastCreator : IRoleCreator
{
    private readonly NextRoleManager _RoleManager = Main._Service.Get<NextRoleManager>();
    public readonly List<IRole> AllRole = [];

    private List<IRole> enableRoles = [];

    public void Dispose()
    {
        Clear();
    }

    public T Create<T>(IRole role) where T : RoleBase
    {
        return (T)role.RoleBase[0];
    }

    public T GetRole<T>(PlayerControl player) where T : class
    {
        return (AllRole.FirstOrDefault(n => n.Players.Contains(player)) as T)!;
    }

    public void Clear()
    {
        AllRole.Clear();
    }

    public IRole GetAssign()
    {
        SetList();
        var random = new Random();
        var list = enableRoles;
        return list[random.Next(list.Count)];
    }

    private void SetList()
    {
        enableRoles = _RoleManager.Roles.Where(n => n.EnableAssign).ToList();
    }
}