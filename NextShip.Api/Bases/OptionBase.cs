using NextShip.Api.Interfaces;
using UnityEngine;

namespace NextShip.Api.Bases;

public abstract class OptionBase(
    bool enableTranslation,
    OptionBehaviour optionBehaviour,
    IOptionInfo optionInfo,
    string title)
{
    public readonly bool EnableTranslation = enableTranslation;
    public readonly OptionBehaviour OptionBehaviour = optionBehaviour;
    public readonly IOptionInfo optionInfo = optionInfo;
    public readonly string Title = title;
    public int id;
    public Color nameColor;
    public string StringId;

    public void AddChildren(IOptionInfo info)
    {
        optionInfo.AddChildren(info);
    }

    public void RemoveChildren(IOptionInfo info)
    {
        optionInfo.RemoveChildren(info);
    }

    public void SetParent(IOptionInfo info)
    {
        optionInfo.setParent(info);
    }

    public abstract void Increase();

    public abstract void Decrease();

    public abstract int GetInt();
    public abstract float GetFloat();
    public abstract string GetValueString();
    public abstract OptionBase GetBase();

    public T GetBase<T>() where T : OptionBase => GetBase() as T;

    // 设置OptionBehaviour隐性转换
    public static explicit operator OptionBehaviour(OptionBase @base)
    {
        return @base.OptionBehaviour;
    }

    public static explicit operator string(OptionBase @base)
    {
        return @base.GetValueString();
    }


    public string GetTitleString()
    {
        return Title;
    }
}