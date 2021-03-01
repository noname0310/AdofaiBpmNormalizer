using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using NoName.AdofaiLevelIO;
using NoName.AdofaiLevelIO.Model;
using NoName.AdofaiLevelIO.Model.Actions;
using EventType = NoName.AdofaiLevelIO.Model.Actions.EventType;
using SetSpeed = NoName.AdofaiLevelIO.Model.Data.SetSpeed;
using Twirl = NoName.AdofaiLevelIO.Model.Data.Twirl;

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
    public Button TwirlOptionButton;
    public Text TwirlOptionButtonText;

    private AdofaiLevel _adofaiLevel;
    private string _levelInfo;
    private bool _dialogIsOpen;
    private bool _useBPMMultiplier;
    private TwirlOption _twirlOption;
    private const double Rad2DegF64 = 360.0 / (Math.PI * 2.0);

    private enum TwirlOption
    {
        DoNotOverride,
        InteriorAngle,
        ExteriorAangle
    }

    private void Start()
    {
        _adofaiLevel = null;
        _levelInfo = null;
        _dialogIsOpen = false;
        _useBPMMultiplier = true;
        _twirlOption = TwirlOption.InteriorAngle;
        ConsoleText.text = string.Empty;
        TwirlOptionButton.onClick.AddListener(OnTwirlOptionButtonButtonClicked);
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
            StartInputFieldPlaceholder.text = "정수 입력..\n(선택사항)";
            EndInputFieldPlaceholder.text = "정수 입력..\n(선택사항)";
            BPMInputFieldPlaceholder.text = "소수 입력..\n(선택사항)";
            CustomPatternInputFieldPlaceholder.text = "e.g) 1, 2, 2, 1\n(선택사항)";
            UseBPMMultiplierButtonText.text = "BPM Multiplier 사용";
            TwirlOptionButtonText.text = "내각 회전 적용";
        }
    }

    private void OnTwirlOptionButtonButtonClicked()
    {
        switch (_twirlOption)
        {
            case TwirlOption.DoNotOverride:
                _twirlOption = TwirlOption.InteriorAngle;
                if (Application.systemLanguage == SystemLanguage.Korean)
                    TwirlOptionButtonText.text = "내각 회전 적용";
                else
                    TwirlOptionButtonText.text = "Set Twirl as InteriorAngle";
                break;
            case TwirlOption.InteriorAngle:
                _twirlOption = TwirlOption.ExteriorAangle;
                if (Application.systemLanguage == SystemLanguage.Korean)
                    TwirlOptionButtonText.text = "외각 회전 적용";
                else
                    TwirlOptionButtonText.text = "Set Twirl as ExteriorAngle";
                break;
            case TwirlOption.ExteriorAangle:
                _twirlOption = TwirlOption.DoNotOverride;
                if (Application.systemLanguage == SystemLanguage.Korean)
                    TwirlOptionButtonText.text = "회전 덮어쓰기 안함";
                else
                    TwirlOptionButtonText.text = "Do not overwrite Twirl";
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
            if (_adofaiLevel != null)
            {
                var startFloorResult = 0;
                var endFloorResult = _adofaiLevel.Floors.Count - 1;

                if (int.TryParse(StartFloorInputField.text, out var startFloorParseResult) && 0 <= startFloorParseResult)
                    startFloorResult = startFloorParseResult;

                if (int.TryParse(EndFloorInputField.text, out var endFloorParseResult) && startFloorResult < endFloorParseResult)
                {
                    endFloorResult = endFloorParseResult;
                    if (_adofaiLevel.Floors.Count <= endFloorResult)
                        endFloorResult = _adofaiLevel.Floors.Count - 1;
                }

                if (CustomPatternInputField.text.Trim() != string.Empty)
                {
                    var parsedInts = CustomPatternInputField.text.Split(',').Select(item => { return int.Parse(item.Trim()); }).ToList();

                    if (_useBPMMultiplier)
                    {
                        LinkedListNode<Floor> startNode = _adofaiLevel.Floors[startFloorResult];

                        if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                        {
                            var startfloor = startNode.Value;
                            SetTwirlasOption(startfloor);
                            startfloor.Bpm = (float) (startfloor.RelativeAngle * Rad2DegF64 / 180 * (parsedInts[0] * bpmResult));
                            if (startfloor.IsMidSpin)
                            {
                                startFloorResult -= 1;
                                endFloorResult -= 1;
                            }
                        }
                        else
                            bpmResult = startNode.Value.Bpm;
                        var currentbpm = (double)startNode.Value.Bpm;

                        LinkedListNode<Floor> currentNode = _adofaiLevel.Floors[startFloorResult + 1];

                        for (var i = startFloorResult + 1; i < endFloorResult; i++)
                        {
                            if (currentNode.Value.IsMidSpin)
                            {
                                i -= 1;
                                endFloorResult -= 1;
                            }
                            var floor = currentNode.Value;
                            SetTwirlasOption(floor);
                            var constbpmvalue = floor.RelativeAngle * Rad2DegF64 / 180 * (parsedInts[(i - startFloorResult) % parsedInts.Count] * bpmResult);
                            if (Mathf.Approximately((float) constbpmvalue, (float)currentbpm) != true)
                                floor.Actions.Add(new SetSpeed(SpeedType.Multiplier, (float)(constbpmvalue / currentbpm)));
                            else
                                floor.Actions.Remove(EventType.SetSpeed);
                            currentbpm = constbpmvalue;

                            currentNode = currentNode.Next;
                        }
                    }
                    else if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                    {
                        LinkedListNode<Floor> currentNode = _adofaiLevel.Floors[startFloorResult];
                        for (var i = startFloorResult; i < endFloorResult; i++)
                        {
                            if (currentNode.Value.IsMidSpin)
                            {
                                i -= 1;
                                endFloorResult -= 1;
                            }
                            var floor = currentNode.Value;
                            SetTwirlasOption(floor);
                            floor.Bpm = (float) (floor.RelativeAngle * Rad2DegF64 / 180 * (parsedInts[(i - startFloorResult) % parsedInts.Count] * bpmResult));
                            currentNode = currentNode.Next;
                        }
                    }
                    else
                    {
                        if (Application.systemLanguage == SystemLanguage.Korean)
                            ConsoleText.text = _levelInfo + $"\n<color=red>입력값이 부족하거나 유효하지 않습니다.</color>";
                        else
                            ConsoleText.text = _levelInfo + "\n<color=red>Input value is not valid.</color>";
                        return;
                    }

                    _adofaiLevel.SaveLevel();
                    if (Application.systemLanguage == SystemLanguage.Korean)
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} {CustomPatternInputField.text} {_twirlOption} 성공적으로 정규화가 되었습니다.</color>";
                    else
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} {CustomPatternInputField.text} {_twirlOption} floors bpm has been normalized.</color>";
                }
                else
                {
                    if (_useBPMMultiplier)
                    {
                        LinkedListNode<Floor> startNode = _adofaiLevel.Floors[startFloorResult];
                        if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                        {
                            var startfloor = startNode.Value;
                            SetTwirlasOption(startfloor);
                            startfloor.Bpm = (float)(startfloor.RelativeAngle * Rad2DegF64 / 180 * bpmResult);
                        }
                        else
                            bpmResult = startNode.Value.Bpm;
                        var currentbpm = (double)startNode.Value.Bpm;
                        
                        LinkedListNode<Floor> currentNode = _adofaiLevel.Floors[startFloorResult + 1];
                        for (var i = startFloorResult + 1; i < endFloorResult; i++)
                        {
                            var floor = currentNode.Value;
                            SetTwirlasOption(floor);
                            var constbpmvalue = (float)(floor.RelativeAngle * Rad2DegF64 / 180 * bpmResult);
                            if (Mathf.Approximately((float)constbpmvalue, (float)currentbpm) != true)
                                floor.Actions.Add(new SetSpeed(SpeedType.Multiplier, (float)(constbpmvalue / currentbpm)));
                            else
                                floor.Actions.Remove(EventType.SetSpeed);
                            currentbpm = constbpmvalue;

                            currentNode = currentNode.Next;
                        }
                    }
                    else if (float.TryParse(BPMInputField.text, out var bpmResult) && 0 < bpmResult)
                    {
                        LinkedListNode<Floor> currentNode = _adofaiLevel.Floors[startFloorResult];
                        for (var i = startFloorResult; i < endFloorResult; i++)
                        {
                            var floor = currentNode.Value;
                            SetTwirlasOption(floor);
                            floor.Bpm = (float) (floor.RelativeAngle * Rad2DegF64 / 180 * bpmResult);

                            currentNode = currentNode.Next;
                        }
                    }
                    else
                    {
                        if (Application.systemLanguage == SystemLanguage.Korean)
                            ConsoleText.text = _levelInfo + $"\n<color=red>입력값이 부족하거나 유효하지 않습니다.</color>";
                        else
                            ConsoleText.text = _levelInfo + "\n<color=red>Input value is not valid.</color>";
                        return;
                    }

                    _adofaiLevel.SaveLevel();
                    if (Application.systemLanguage == SystemLanguage.Korean)
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} {_twirlOption} 성공적으로 정규화가 되었습니다.</color>";
                    else
                        ConsoleText.text = _levelInfo +
                                           $"\n<color=green>{startFloorResult}~{endFloorResult} {_twirlOption} floors bpm has been normalized.</color>";
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

    private void SetTwirlasOption(Floor floor)
    {
        switch (_twirlOption)
        {
            case TwirlOption.DoNotOverride:
                break;
            case TwirlOption.InteriorAngle:
                if (floor.Index == 0)
                    break;
                if (Math.PI < floor.RelativeAngle)
                {
                    var isTwirlExist = false;
                    foreach (var item in floor.Actions)
                    {
                        if (item.EventType == EventType.Twirl)
                            isTwirlExist = true;
                    }

                    if (isTwirlExist)
                        floor.Actions.Remove(EventType.Twirl);
                    else
                        floor.Actions.Add(new Twirl());
                }
                break;
            case TwirlOption.ExteriorAangle:
                if (floor.Index == 0)
                    break;
                if (Math.PI > floor.RelativeAngle)
                {
                    var isTwirlExist = false;
                    foreach (var item in floor.Actions)
                    {
                        if (item.EventType == EventType.Twirl)
                            isTwirlExist = true;
                    }

                    if (isTwirlExist)
                        floor.Actions.Remove(EventType.Twirl);
                    else
                        floor.Actions.Add(new Twirl());
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
