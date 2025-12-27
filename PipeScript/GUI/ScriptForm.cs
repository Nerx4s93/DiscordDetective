using System;
using System.Windows.Forms;
using PipeScript.API;

namespace PipeScript.GUI;

public sealed partial class ScriptForm : Form, IScriptHost
{
    private readonly PipeScriptEngine _pipeScriptEngine;
    private readonly string _scriptName;
    private readonly string _code;

    public ScriptForm(string scriptName, string code)
    {
        InitializeComponent();
        Text = scriptName;

        _pipeScriptEngine = new PipeScriptEngine(scriptName)
        {
            Context =
            {
                Host = new WinFormsHost(this)
            }
        };
        _scriptName = scriptName;
        _code = code;
    }

    private void buttonStart_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Start(_code);
    }

    private void buttonStop_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Stop();
    }

    private void buttonRestart_Click(object sender, EventArgs e)
    {
        _pipeScriptEngine.Restart(_code);
    }

    public void WriteLine(string text)
    {
        InvokeIfRequired(() =>
        {
            richTextBoxOutput.AppendText(text + Environment.NewLine);
        });
    }

    public void Write(string text)
    {
        InvokeIfRequired(() =>
        {
            richTextBoxOutput.AppendText(text);
        });
    }

    private void InvokeIfRequired(Action action)
    {
        if (InvokeRequired)
        {
            BeginInvoke(action);
        }
        else
        {
            action();
        }
    }
}
