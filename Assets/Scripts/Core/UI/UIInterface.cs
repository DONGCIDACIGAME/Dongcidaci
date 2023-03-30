using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public static class UIInterface
{
    public static string GetInputFieldString(TMP_InputField inputField)
    {
        //if (go == null)
        //{
        //    Log.Error(ErrorLevel.Critical, "GetInputFieldString Error, go is null!");
        //    return string.Empty;
        //}

        //var inputField = go.GetComponent<TMP_InputField>();
        if(inputField == null)
        {
            Log.Error(LogLevel.Critical, "GetInputFieldString Error, InputField component is required!");
            return string.Empty;
        }

        return inputField.text;
    }

    public static void SetInputFieldString(GameObject go,string str)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "SetInputFieldString Error, go is null!");
            return;
        }

        var inputField = go.GetComponent<TMP_InputField>();
        if (inputField == null)
        {
            Log.Error(LogLevel.Critical, "SetInputFieldString Error, InputField component is required!");
            return;
        }

        inputField.text = str;
    }

    public static int GetDropDownSelection(GameObject go)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "GetDropDownSelection Error, go is null!");
            return 0;
        }

        var dropDown = go.GetComponent<TMP_Dropdown>();
        if (dropDown == null)
        {
            Log.Error(LogLevel.Critical, "GetDropDownSelection Error, Dropdown component is required!");
            return 0;
        }

        return dropDown.value;
    }

    public static void SetDropDownSelection(GameObject go,int selection)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "SetDropDownSelection Error, go is null!");
            return;
        }

        var dropDown = go.GetComponent<TMP_Dropdown>();
        if (dropDown == null)
        {
            Log.Error(LogLevel.Critical, "SetDropDownSelection Error, Dropdown component is required!");
            return;
        }

        dropDown.SetValueWithoutNotify(selection);
    }

    public static void AddButtonAction(GameObject go, UnityAction call)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "AddButtonAction Error, go is null!");
            return;
        }

        var btn = go.GetComponent<Button>();
        if (btn == null)
        {
            Log.Error(LogLevel.Critical, "AddButtonAction Error, Button component is required!");
            return;
        }

        btn.onClick.AddListener(call);
    }

    public static void SetSilderValue(GameObject go, float value)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "SetSilderValue Error, go is null!");
            return;
        }

        var slider = go.GetComponent<Slider>();
        if (slider == null)
        {
            Log.Error(LogLevel.Critical, "SetSilderValue Error, Slider component is required!");
            return;
        }

        slider.SetValueWithoutNotify(value);
    }

    public static void AddSilderChangeAction(GameObject go, UnityAction<float> call)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "AddSilderChangeAction Error, go is null!");
            return;
        }

        var slider = go.GetComponent<Slider>();
        if (slider == null)
        {
            Log.Error(LogLevel.Critical, "AddSilderChangeAction Error, Slider component is required!");
            return;
        }

        slider.onValueChanged.AddListener(call);
    }


    public static void SetGraphicSize(GameObject go,float width,float height)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "SetGraphicSize Error, go is null!");
            return;
        }

        RectTransform rt = go.transform as RectTransform;

        if(rt != null)
        {
            rt.sizeDelta = new Vector2(width, height);
        }
    }

    public static void SetTextString(GameObject go,string str)
    {
        if (go == null)
        {
            Log.Error(LogLevel.Critical, "SetTextString Error, go is null!");
            return;
        }

        var text = go.GetComponent<TMP_Text>();
        if (text == null)
        {
            Log.Error(LogLevel.Critical, "SetTextString Error, TMP_Text component is required!");
            return;
        }

        text.text = str;
    }

    public static void SetGraphicColor(GameObject go,Color color)
    {
        var graphic = go.GetComponent<Graphic>();
        if (graphic == null)
        {
            Log.Error(LogLevel.Normal, "SetGraphicColor Error,Graphic component is required!");
            return;
        }

        graphic.color = color;
    }

}
