using System;
using NextShip.Api.Attributes;
using NextShip.Api.Bases;
using TMPro;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
public class ShipOptionBehaviour : MonoBehaviour
{
    public SpriteRenderer BackGround_Sprite;
    public TextMeshPro ValueText;
    public TextMeshPro TitleText;
    public PassiveButton PreviousButton;
    public PassiveButton NextButton;

    public Action<ShipOptionBehaviour> OnOptionValueChanged;
    public OptionBase OptionBase { private set; get; }

    public void OnEnable()
    {
        if (OptionBase == null) return;
        TitleText.text = OptionBase.GetTitleString();
        ValueText.text = OptionBase.GetValueString();
    }

    public void SetOptionBase(OptionBase optionBase)
    {
        OptionBase = optionBase;
    }

    /*internal static GameObject GenerateOption(Transform Parent, string Name = "Temp")
    {
        var Option = new GameObject(Name);
        Option.transform.SetParent(Parent);
        Option.transform.localPosition = new Vector3(0, 0, -5);
        var behaviour = Option.AddComponent<ShipOptionBehaviour>();

        var BackGroundObj = new GameObject("BackGround");
        var BackGround = behaviour.BackGround_Sprite = BackGroundObj.AddComponent<SpriteRenderer>();
        BackGround.sprite = ObjetUtils.Find<Sprite>("smallButtonBorder");

        var ValueText = behaviour.ValueText = new GameObject("ValueText").AddComponent<TextMeshPro>();
        var TitleText = behaviour.TitleText = new GameObject("TitleText").AddComponent<TextMeshPro>();

        ValueText.text = "ValueText";
        TitleText.text = "TitleText";

        ValueText.transform.SetParent(Option.transform);
        TitleText.transform.SetParent(Option.transform);
        BackGround.transform.SetParent(Option.transform);

        var leftButtonSprite = SpriteUtils.LoadSpriteFromResources("left.png");
        var rightButtonSprite = SpriteUtils.LoadSpriteFromResources("right.png");

        var LeftButton = new GameObject("LeftButton");
        LeftButton.transform.SetParent(Option.transform);
        var LeftButton_Sprite = LeftButton.AddComponent<SpriteRenderer>();
        LeftButton_Sprite.sprite = leftButtonSprite;
        LeftButton.CreatePassiveButton(() =>
        {
            behaviour.OptionBase.Decrease();
            behaviour.OnOptionValueChanged(behaviour);
        });

        var RightButton = new GameObject("RightButton");
        RightButton.transform.SetParent(Option.transform);
        var RightButton_Sprite = RightButton.AddComponent<SpriteRenderer>();
        RightButton_Sprite.sprite = rightButtonSprite;
        RightButton.CreatePassiveButton(() =>
        {
            behaviour.OptionBase.Increase();
            behaviour.OnOptionValueChanged(behaviour);
        });

        return Option;
    }*/
}