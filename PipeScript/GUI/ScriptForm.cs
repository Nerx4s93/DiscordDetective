using System;
using System.Windows.Forms;

namespace PipeScript.GUI;

public partial class ScriptForm : Form
{
    private readonly PipeScriptEngine _pipeScriptEngine;

    public ScriptForm(PipeScriptEngine pipeScriptEngine)
    {
        InitializeComponent();
        _pipeScriptEngine = pipeScriptEngine;
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
