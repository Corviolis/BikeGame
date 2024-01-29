namespace LilBikerBoi.resources;

using Godot;
using GC = Godot.Collections;
using System;

public partial class DialogicSharp: Node
{
  private static GodotObject _dialogic;
  //private const String DEFAULT_DIALOG_RESOURCE = "res://addons/dialogic/Dialog.tscn";

  static DialogicSharp()
  {
    GDScript script = GD.Load<GDScript>("res://addons/dialogic/Other/DialogicGameHandler.gd");
    _dialogic = (GodotObject) script.New();
  }

  public static String CurrentTimeline
  {
    get
    {
      return (String) _dialogic.Call("get_current_timeline");
    }
    set
    {
      _dialogic.Call("set_current_timeline", value);
    }
  }

  public static GC.Dictionary Definitions
  {
    get
    {
      return (GC.Dictionary) _dialogic.Call("get_definitions");
    }
  }

  public static GC.Dictionary DefaultDefinitions
  {
    get
    {
      return (GC.Dictionary)_dialogic.Call("get_default_definitions");
    }
  }

  public static Node Start(String timeline, bool resetSaves = true, bool debugMode = false)
  {
    return Start<Node>(timeline, resetSaves, debugMode);
  }

  public static T Start<T>(String timeline, bool resetSaves = true, bool debugMode = false) where T : Node
  {
    return (T) _dialogic.Call("start", timeline);
  }

  /*public static Node StartFromSave(String timeline, bool debugMode = false)
  {
    return StartFromSave<Node>(timeline, DEFAULT_DIALOG_RESOURCE, debugMode);
  }*/

  public static T StartFromSave<T>(String timeline, String dialogScenePath, bool debugMode = false) where T : Node
  {
    return (T) _dialogic.Call("start", timeline, dialogScenePath, debugMode);
  }

  public static String GetVariable(String name)
  {
    return (String) _dialogic.Call("get_variable", name);
  }

  public static void SetVariable(String name, String value)
  {
    _dialogic.Call("set_variable", name, value);
  }

  public static GC.Dictionary GetGlossary(String name)
  {
    return (GC.Dictionary)_dialogic.Call("get_glossary", name);
  }

  public static void SetGlossary(String name, String title, String text, String extra)
  {
    _dialogic.Call("set_glossary", name, title, text, extra);
  }
}