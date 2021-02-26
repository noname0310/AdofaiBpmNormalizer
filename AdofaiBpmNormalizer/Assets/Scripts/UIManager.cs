using System;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using NoName.AdofaiLevelIO;
using NoName.AdofaiLevelIO.Model.Actions;
using EventType = NoName.AdofaiLevelIO.Model.Actions.EventType;
using SetSpeed = NoName.AdofaiLevelIO.Model.Data.SetSpeed;

// ReSharper disable All

public class UIManager : MonoBehaviour
{
    public Text StartLabel;
    public Text EndLabel;
    public Text CustomPatternLabel;
    public InputField StartFloorInputField;
    public InputField EndFloorInputField;
    public InputField BPMInputField;
    public InputField CustomPatternInputField;
    public Button OpenLevelButton;
    public Text OpenLevelButtonLabel;
    public Button RunButton;
    public Text RunButtonLabel;
    public Text ConsoleText;
    public Text StartInputFieldPlaceholder;
    public Text EndInputFieldPlaceholder;
    public Text BPMInputFieldPlaceholder;
    public Text CustomPatternInputFieldPlaceholder;
    public Button UseBPMMultiplierButton;
    public Text UseBPMMultiplierButtonText;

    private AdofaiLevel _adofaiLevel;
    private string _levelInfo;
    private bool _dialogIsOpen;
    private bool _useBPMMultiplier;

    private void Start()
    {
        _adofaiLevel = null;
        _levelInfo = null;
        _dialogIsOpen = false;
        _useBPMMultiplier = true;
        ConsoleText.text = string.Empty;
        UseBPMMultiplierButton.onClick.AddListener(OnMultiplierButtonButtonClicked);
        OpenLevelButton.onClick.AddListener(OnOpenLevelButtonClicked);
        RunButton.onClick.AddListener(OnRunButtonButtonClicked);

        if (Application.systemLanguage == SystemLanguage.Korean)
        {
            StartLabel.text = "시작 타일 번호";
            EndLabel.text = "끝 타일 번호";
            CustomPatternLabel.text = "커스텀 패턴";
            OpenLevelButtonLabel.text = "레벨 열기";
            RunButtonLabel.text = "실행";
            StartInputFieldPlaceholder.text = "정수 입력..";
            EndInputFieldPlaceholder.text = "정수 입력..";
            BPMInputFieldPlaceholder.text = "소수 입력..\n(선택사항)";
            CustomPatternInputFieldPlaceholder.text = "e.g) 1, 2, 2, 1\n(선택사항)";
            UseBPMMultiplierButtonText.text = "BPM Multiplier 사용";
        }
    }

    private void OnMultiplierButtonButtonClicked()
    {
        if (_useBPMMultiplier)
        {
            _useBPMMultiplier = false;
            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                UseBPMMultiplierButtonText.text = "BPM 고정값 사용";
                BPMInputFieldPlaceholder.text = "소수 입력..";
            }
            else
            {
                UseBPMMultiplierButtonText.text = "Use Const BPM Value";
                BPMInputFieldPlaceholder.text = "Enter decimal...";
            }
        }
        else
        {
            _useBPMMultiplier = true;
            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                UseBPMMultiplierButtonText.text = "승수 사용";
                BPMInputFieldPlaceholder.text = "소수 입력..\n(선택사항)";
            }
            else
            {
                UseBPMMultiplierButtonText.text = "Use BPM Multiplier";
                BPMInputFieldPlaceholder.text = "Enter decimal...\n(optional)";
            }
        }
    }

    private void OnOpenLevelButtonClicked()
    {
        try
        {
            if (_dialogIsOpen)
                return;
            _dialogIsOpen = true;
            FileBrowser.ShowLoadDialog(
                (string[] path) =>
                {
                    if (path.Length != 0)
                    {
                        _adofaiLevel = new AdofaiLevel(path[0]);

                        var stringBuilder = new StringBuilder();
                        if (Application.systemLanguage == SystemLanguage.Korean)
                        {
                            stringBuilder.AppendLine($"아티스트: {_adofaiLevel.LevelInfo.Artist}");
                            stringBuilder.AppendLine($"만든이: {_adofaiLevel.LevelInfo.Author}");
                            stringBuilder.AppendLine($"곡명: {_adofaiLevel.LevelInfo.Song}");
                            stringBuilder.AppendLine($"Bpm: {_adofaiLevel.LevelInfo.Bpm}");
                            stringBuilder.AppendLine($"타일개수: {_adofaiLevel.Floors.Count}");
                        }
                        else
                        {
                            stringBuilder.AppendLine($"Artist: {_adofaiLevel.LevelInfo.Artist}");
                            stringBuilder.AppendLine($"Author: {_adofaiLevel.LevelInfo.Author}");
                            stringBuilder.AppendLine($"Song: {_adofaiLevel.LevelInfo.Song}");
                            stringBuilder.AppendLine($"Bpm: {_adofaiLevel.LevelInfo.Bpm}");
                            stringBuilder.AppendLine($"Floors.Count: {_adofaiLevel.Floors.Count}");
                        }

                        _levelInfo = stringBuilder.ToString();

                        ConsoleText.text = _levelInfo;
                    }
                    _dialogIsOpen = false;
                },
                () => {
                    _dialogIsOpen = false;
                },
                FileBrowser.PickMode.Files,
                false);
            
        }
        catch (Exception e)
        {
            ConsoleText.text = e.ToString();
        }
    }

    private void OnRunButtonButtonClicked()
    {
        try
        {
            if (_adofaiLevel != null
                && int.TryParse(StartFloorInputField.text, out var startFloorResult) && 0 <= startFloorResult
                && int.TryParse(EndFloorInputField.text, out var endFloorResult) && startFloorResult < endFloorResult)
            {
                if (_adofaiLevel.Floors.Count <= endFloorResult)
                    endFloorResult = _adofaiLevel.Floors.Count - 1;

                if (CustomPatternInputField.text.Trim() != string.Empty)
                {
                    var parsedInts = CustomPatternInputField.text.Split(',').Select(item => { return int.Parse(item.Trim()); }).ToList();

                    if (_useBPMMultiplier)
                    {
                        if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                            _adofaiLevel.Floors[startFloorResult].Bpm = bpmResult;
                        else
                            bpmResult = _adofaiLevel.Floors[startFloorResult].Bpm;
                        var currentbpm = (double)_adofaiLevel.Floors[startFloorResult].Bpm;

                        for (var i = startFloorResult + 1; i < endFloorResult; i++)
                        {
                            var floor = _adofaiLevel.Floors[i];

                            var constbpmvalue = floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * (parsedInts[i % parsedInts.Count] * bpmResult);
                            if (Mathf.Approximately((float) constbpmvalue, (float)currentbpm) != true)
                                floor.Actions.Add(new SetSpeed(SpeedType.Multiplier, (float)(constbpmvalue / currentbpm)));
                            else
                                floor.Actions.Remove(EventType.SetSpeed);
                            currentbpm = constbpmvalue;
                        }
                    }
                    else if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                    {
                        for (var i = startFloorResult; i < endFloorResult; i++)
                        {
                            var floor = _adofaiLevel.Floors[i];
                            floor.Bpm = (float) (floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * (parsedInts[i % parsedInts.Count] * bpmResult));
                        }
                    }
                    else
                    {
                        if (Application.systemLanguage == SystemLanguage.Korean)
                            ConsoleText.text = _levelInfo + $"\n<color=red>입력값이 부족하거나 유효하지 않습니다.</color>";
                        else
                            ConsoleText.text = _levelInfo + "\n<color=red>Input value is not valid.</color>";
                    }

                    _adofaiLevel.SaveLevel();
                    if (Application.systemLanguage == SystemLanguage.Korean)
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} {CustomPatternInputField.text} 성공적으로 정규화가 되었습니다.</color>";
                    else
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} {CustomPatternInputField.text} floors bpm has been normalized.</color>";
                }
                else
                {
                    if (_useBPMMultiplier)
                    {
                        if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                            _adofaiLevel.Floors[startFloorResult].Bpm = bpmResult;
                        else
                            bpmResult = _adofaiLevel.Floors[startFloorResult].Bpm;
                        var currentbpm = (double)_adofaiLevel.Floors[startFloorResult].Bpm;

                        for (var i = startFloorResult + 1; i < endFloorResult; i++)
                        {
                            var floor = _adofaiLevel.Floors[i];
                            var constbpmvalue = (float)(floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * bpmResult);
                            if (Mathf.Approximately((float)constbpmvalue, (float)currentbpm) != true)
                                floor.Actions.Add(new SetSpeed(SpeedType.Multiplier, (float)(constbpmvalue / currentbpm)));
                            else
                                floor.Actions.Remove(EventType.SetSpeed);
                            currentbpm = constbpmvalue;
                        }
                    }
                    else if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                    {
                        for (var i = startFloorResult; i < endFloorResult; i++)
                        {
                            var floor = _adofaiLevel.Floors[i];
                            floor.Bpm = (float) (floor.RelativeAngle * 360.0 / (Math.PI * 2.0) / 180 * bpmResult);
                        }
                    }
                    else
                    {
                        if (Application.systemLanguage == SystemLanguage.Korean)
                            ConsoleText.text = _levelInfo + $"\n<color=red>입력값이 부족하거나 유효하지 않습니다.</color>";
                        else
                            ConsoleText.text = _levelInfo + "\n<color=red>Input value is not valid.</color>";
                    }

                    _adofaiLevel.SaveLevel();
                    if (Application.systemLanguage == SystemLanguage.Korean)
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} 성공적으로 정규화가 되었습니다.</color>";
                    else
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} floors bpm has been normalized.</color>";
                }
            }
            else
            {
                if (Application.systemLanguage == SystemLanguage.Korean)
                    ConsoleText.text = _levelInfo + $"\n<color=red>입력값이 부족하거나 유효하지 않습니다.</color>";
                else
                    ConsoleText.text = _levelInfo + "\n<color=red>Input value is not valid.</color>";
            }
        }
        catch (Exception e)
        {
            ConsoleText.text = e.ToString();
        }
    }
}
