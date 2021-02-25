using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using NoName.AdofaiLevelIO;
using UnityEditor;

// ReSharper disable All

public class UIManager : MonoBehaviour
{
    public InputField StartFloorInputField;
    public InputField EndFloorInputField;
    public InputField BPMInputField;
    public Button OpenLevelButton;
    public Button RunButton;
    public Text ConsoleText;

    private AdofaiLevel _adofaiLevel;
    private string _levelInfo;

    private void Start()
    {
        _adofaiLevel = null;
        _levelInfo = null;
        ConsoleText.text = String.Empty;
        OpenLevelButton.onClick.AddListener(OnOpenLevelButtonClicked);
        RunButton.onClick.AddListener(OnRunButtonButtonClicked);
    }

    private void OnOpenLevelButtonClicked()
    {
        try
        {
            string path = EditorUtility.OpenFilePanel("Open Adofai Level", "", "adofai");
            if (path.Length != 0)
            {
                _adofaiLevel = new AdofaiLevel(path);

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Artist: {_adofaiLevel.LevelInfo.Artist}");
                stringBuilder.AppendLine($"Author: {_adofaiLevel.LevelInfo.Author}");
                stringBuilder.AppendLine($"Song: {_adofaiLevel.LevelInfo.Song}");
                stringBuilder.AppendLine($"Bpm: {_adofaiLevel.LevelInfo.Bpm}");
                stringBuilder.AppendLine($"Floors.Count: {_adofaiLevel.Floors.Count}");
                _levelInfo = stringBuilder.ToString();

                ConsoleText.text = _levelInfo;
            }
        }
        catch (Exception e)
        {
            ConsoleText.text = e.Message;
        }
    }

    private void OnRunButtonButtonClicked()
    {
        try
        {
            if (_adofaiLevel != null
                && int.TryParse(StartFloorInputField.text, out var startFloorResult) && 0 < startFloorResult
                && int.TryParse(EndFloorInputField.text, out var endFloorResult) && startFloorResult < endFloorResult
                && float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
            {
                if (_adofaiLevel.Floors.Count <= endFloorResult)
                    endFloorResult = _adofaiLevel.Floors.Count - 1;

                for (var i = startFloorResult; i < endFloorResult; i++)
                {
                    var floor = _adofaiLevel.Floors[i];
                    floor.Bpm = (float) (floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * bpmResult);
                }

                ConsoleText.text =
                    _levelInfo + $"\n\n {startFloorResult}~{endFloorResult} floors bpm has been normalized.";
            }
            else
                ConsoleText.text = _levelInfo + "\n\nInput value is not valid.";
        }
        catch (Exception e)
        {
            ConsoleText.text = e.Message;
        }
    }
}
