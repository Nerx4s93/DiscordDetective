using System;
using System.Windows.Forms;

namespace PipeScript.GUI;

public sealed partial class ScriptForm : Form
{
    private readonly PipeScriptEngine _pipeScriptEngine;
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

    public void AppendLine(string text)
    {
        InvokeIfRequired(() =>
        {
            richTextBoxOutput.AppendText(text + Environment.NewLine);
        });
    }

    public void AppendText(string text)
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
