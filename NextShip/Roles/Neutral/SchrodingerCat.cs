using System;
using System.Collections.Generic;
using NextShip.Api.Interfaces;
using OtherAttribute;
using UnityEngine;

namespace NextShip.Roles.Neutral;

[FastAddRole]
public class SchrodingerCat : IRole
{
    public SchrodingerCat()
    {
        
    }
    
    public void Dispose()
    {
    }

    public Type RoleBaseType { get; set; } = typeof(SchrodingerCatBase);

    public Type RoleType { get; set; } = typeof(SchrodingerCat);
    
    public SimpleRoleInfo SimpleRoleInfo { get; set; } = new()
    {
        RoleColor = Color.gray,
        roleTeam = RoleTeam.Neutral,
        roleType = Api.Roles.RoleType.MainRole,
        Name = nameof(SchrodingerCat),
        RoleStringId = nameof(SchrodingerCat)
    };
    public List<RoleBase> RoleBase { get; set; } = [];
    public List<PlayerControl> Players { get; set; } = [];

    public bool EnableAssign { get; set; } = true;
    
    public Func<PlayerControl, RoleBase> CreateRoleBase { get; set; }
    
    public bool CanCreate(IRole role, PlayerControl player)
    {
        return true;
    }
    
    public class SchrodingerCatBase(PlayerControl player) : RoleBase(player)
    {
        
    }
}