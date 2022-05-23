using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VRCExtended.Utils;

internal static class UiUtils
{
    private static Transform _selectedUserMenuQm;
    public static Transform SelectedUserMenuQm => _selectedUserMenuQm ??= Resources.FindObjectsOfTypeAll<VRC.UI.Elements.Menus.SelectedUserMenuQM>()[1].transform;
    private static Button _baseButton;
    private static Button BaseButton => _baseButton ??= new Func<Button>(() =>
    {
        var button = UnityEngine.Object.Instantiate( SelectedUserMenuQm
                .Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_AvatarActions/Button_AddToFavorites"))
            .GetComponent<Button>();
        UnityEngine.Object.DestroyImmediate(button.transform.Find("Favorite Disabled Button").gameObject);
        button.onClick = new Button.ButtonClickedEvent();
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Placeholder";
        button.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = "Placeholder";
        button.name = "Button_Base";
        return button;
    }).Invoke();
    internal enum Menu { InteractMenu }
    public static Button CreateButton(Menu menu, string uiButtonText, string uiTooltip, Action onClick) => CreateButton(menu switch
    {
        Menu.InteractMenu => SelectedUserMenuQm.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions"),
        _ => throw new ArgumentException("Menu not found.", nameof(menu))
    }, uiButtonText, uiTooltip, onClick);
    public static Button CreateButton(Transform parent, string uiButtonText, string uiTooltip, Action onClick)
    {
        var button = UnityEngine.Object.Instantiate(BaseButton, parent);
        button.onClick.AddListener(onClick);
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = uiButtonText;
        button.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = uiTooltip;
        button.name = "Button_" + uiButtonText.Split(' ').Aggregate("", (current, str) => current + str);
        return button;
    }
}