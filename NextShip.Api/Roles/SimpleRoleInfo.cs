using System.Text.Json.Serialization;
using UnityEngine;

namespace NextShip.Api.Roles;

public sealed class SimpleRoleInfo
{
    [JsonPropertyName("Name"), JsonInclude]
    public string Name { get; set; }
    
    [JsonPropertyName("Color"), JsonInclude]
    public Color RoleColor { get; set; }
    
    [JsonPropertyName("EnumId"), JsonInclude]
    public RoleId roleId { get; set; }
    
    [JsonPropertyName("IntId"), JsonInclude]
    public int RoleIntId { get; set; }
    
    [JsonPropertyName("StringId"), JsonInclude]
    public string RoleStringId { get; set; }
    
    [JsonPropertyName("Team"), JsonInclude]
    public RoleTeam roleTeam { get; set; }
    
    [JsonPropertyName("Type"), JsonInclude]
    public RoleType roleType { get; set; }

    public SimpleRoleInfo() {}

    public SimpleRoleInfo(
        RoleId id,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleStringId,
        string roleName,
        int roleIntId)
    {
        Name = roleName;
        RoleColor = color;
        roleId = id;
        roleTeam = team;
        roleType = type;
        RoleStringId = roleStringId;
        RoleIntId = roleIntId;
    }
    
    public SimpleRoleInfo
    (
        RoleId id,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleName) : this(id, color, team, type, "", roleName, -1)
    {
    }

    public SimpleRoleInfo
    (
        Type classType,
        Color color,
        RoleTeam team,
        RoleType type,
        string roleName
    ) : this(RoleId.none, color, team, type, "", roleName, -1)
    {
    }
}